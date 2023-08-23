using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        Save,
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

    public override bool Init()
    {
        if (base.Init() == false)
            if (base.Init() == false)
                return false;

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        if (Managers.Game.Name != null)
            GetText((int)Texts.UserNameText).text = Managers.Game.Name;
        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ClosePopupUI(this); });
        GetText((int)Texts.Save).gameObject.BindEvent(() => OnClickedSaveBtn());
        GetImage((int)Images.UserImage).gameObject.BindEvent(() => OnClickedProfile());

        Managers.DBManager.reference.Child("Users").Child(Managers.UserMng.user.UID).ValueChanged += HandleValueChanged;

        GetObject((int)GameObjects.UserName).gameObject.GetComponentInChildren<TMP_InputField>().text = Managers.UserMng.GetNickname();
        GetObject((int)GameObjects.UserMessage).gameObject.GetComponentInChildren<TMP_InputField>().text = Managers.UserMng.GetMessage();
        GetText((int)Texts.UIDText).text = Managers.UserMng.user.UID.ToString().Substring(0, 8);

        //Tier
        SetTier();

        return true;
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

    void SetTier()
    {
        int score = Managers.UserMng.GetScore();

        if (score < 100)
        {
            GetImage((int)Images.TierImage).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Tier/T1");
            GetText((int)Texts.TierText).text = "수학 입문자";
        }
        else if(score >= 101 && score < 200)
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
