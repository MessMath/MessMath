using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MessMathI18n;

public class UI_Info : UI_Popup
{
    enum GameObjects
    {
        UserName,
        UserMessage,
    }

    enum Texts
    {
        UserNameText,
        UIDText,
        TierText,
    }

    enum Buttons
    {
        ExitBtn,
    }

    enum Images
    {
        BG,
        UserImageBG,
        UserImage,
        TierImage,
    }

    string PlayerName;
    string PlayerMessage;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        if (Managers.Game.Name != null)
            GetText((int)Texts.UserNameText).text = Managers.Game.Name;
        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(() => { OnClickedSaveBtn(); Managers.UI.ClosePopupUI(this); });
        GetImage((int)Images.UserImage).gameObject.BindEvent(() => OnClickedProfile());
        GetText((int)Texts.UIDText).text = Managers.GoogleSignIn.GetUID().ToString().Substring(0, 8);

        Managers.DBManager.reference.Child("Users").Child(Managers.GoogleSignIn.GetUID()).ValueChanged += HandleValueChanged;

        // placeholder
        TextMeshProUGUI placeholder = (TextMeshProUGUI)GetObject((int)GameObjects.UserName).gameObject.GetComponentInChildren<TMP_InputField>().placeholder;
        placeholder.text = I18n.Get(I18nDefine.INFO_NICKNAME);
        placeholder = (TextMeshProUGUI)GetObject((int)GameObjects.UserMessage).gameObject.GetComponentInChildren<TMP_InputField>().placeholder;
        placeholder.text = I18n.Get(I18nDefine.INFO_MESSAGE);

        InitGetNickName();
        InitGetMessage();
        SetTier();

        return true;
    }

    async void InitGetNickName()
    {
        PlayerName = await Managers.DBManager.GetNickName(Managers.GoogleSignIn.GetUID());
        GetObject((int)GameObjects.UserName).gameObject.GetComponentInChildren<TMP_InputField>().text = PlayerName;
    }

    async void InitGetMessage()
    {
        PlayerMessage = await Managers.DBManager.GetUserMessage(Managers.GoogleSignIn.GetUID());
        GetObject((int)GameObjects.UserMessage).gameObject.GetComponentInChildren<TMP_InputField>().text = PlayerMessage;
    }

    private void Update()
    {
        if (PlayerPrefs.GetString("Profile") != "")
        {
            GetImage((int)Images.UserImage).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("Profile"));
        }
    }

    void OnClickedSaveBtn()
    {
        Managers.Sound.Play("ClickBtnEff");
        Managers.DBManager.SetNickname(GetObject((int)GameObjects.UserName).gameObject.GetComponentInChildren<TMP_InputField>().text);
        Managers.DBManager.SetUserMessage(GetObject((int)GameObjects.UserMessage).gameObject.GetComponentInChildren<TMP_InputField>().text);
    }

    void OnClickedProfile()
    {
         Managers.Sound.Play("ClickBtnEff");
        Managers.UI.ShowPopupUI<UI_SelectProfile>();
    }
    async void SetTier()
    {
        int score = await Managers.DBManager.GetScore(Managers.GoogleSignIn.GetUID());

        if (score < 100)
        {
            GetImage((int)Images.TierImage).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Tier/T1");
            GetText((int)Texts.TierText).text = "수학 입문자";
        }
        else if (score >= 101 && score < 200)
        {
            GetImage((int)Images.TierImage).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Tier/T2");
            GetText((int)Texts.TierText).text = "수학 초보자";
        }
        else if (score >= 201 && score < 300)
        {
            GetImage((int)Images.TierImage).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Tier/T3");
            GetText((int)Texts.TierText).text = "수학 숙련자";
        }
        else if (score >= 301 && score < 400)
        {
            GetImage((int)Images.TierImage).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Tier/T4");
            GetText((int)Texts.TierText).text = "D급 마법사";
        }
        else if (score >= 401 && score < 500)
        {
            GetImage((int)Images.TierImage).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Tier/T5");
            GetText((int)Texts.TierText).text = "C급 마법사";
        }
        else if (score >= 501 && score < 600)
        {
            GetImage((int)Images.TierImage).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Tier/T6");
            GetText((int)Texts.TierText).text = "B급 마법사";
        }
        else if (score >= 601 && score < 700)
        {
            GetImage((int)Images.TierImage).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Tier/T7");
            GetText((int)Texts.TierText).text = "A급 마법사";
        }
        else if (score >= 701 && score < 800)
        {
            GetImage((int)Images.TierImage).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Tier/T8");
            GetText((int)Texts.TierText).text = "S급 마법사";
        }
        else
        {
            GetImage((int)Images.TierImage).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Tier/T9");
            GetText((int)Texts.TierText).text = "초월자";
        }
    }
}
