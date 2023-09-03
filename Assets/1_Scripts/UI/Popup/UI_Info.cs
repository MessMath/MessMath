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
        CustomizingObject1,
        CustomizingObject2,
        CustomizingObject3,
    }

    enum Texts
    {
        UserNameText,
        Save,
        InfoText,
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
    }

    string PlayerName;
    string PlayerMessage;

    public override bool Init()
    {
        if (base.Init() == false)
            if (base.Init() == false)
                return false;

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        #region CustomizingObject Clear
        GetObject((int)GameObjects.CustomizingObject1).gameObject.GetComponent<Image>().sprite = null;
        GetObject((int)GameObjects.CustomizingObject2).gameObject.GetComponent<Image>().sprite = null;
        GetObject((int)GameObjects.CustomizingObject3).gameObject.GetComponent<Image>().sprite = null;
        #endregion
        if (Managers.Game.Name != null)
            GetText((int)Texts.UserNameText).text = Managers.Game.Name;
        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ClosePopupUI(this); });
        GetText((int)Texts.Save).gameObject.BindEvent(() => OnClickedSaveBtn());
        GetText((int)Texts.Save).text = I18n.Get(I18nDefine.INFO_SAVE);
        GetText((int)Texts.InfoText).text = I18n.Get(I18nDefine.INFO_TEXT);
        GetImage((int)Images.UserImage).gameObject.BindEvent(() => OnClickedProfile());

        InitGetNickName();
        InitGetMessage();

        Managers.DBManager.reference.Child("Users").Child(Managers.GoogleSignIn.GetUID()).ValueChanged += HandleValueChanged;

        // placeholder
        TextMeshProUGUI placeholder = (TextMeshProUGUI)GetObject((int)GameObjects.UserName).gameObject.GetComponentInChildren<TMP_InputField>().placeholder;
        placeholder.text = I18n.Get(I18nDefine.INFO_NICKNAME);
        placeholder = (TextMeshProUGUI)GetObject((int)GameObjects.UserMessage).gameObject.GetComponentInChildren<TMP_InputField>().placeholder;
        placeholder.text = I18n.Get(I18nDefine.INFO_MESSAGE);

        GetObject((int)GameObjects.UserMessage).gameObject.GetComponentInChildren<TMP_InputField>().text = PlayerMessage;

        return true;
    }

    void InitGetNickName()
    {
        var GettingNickName = Managers.DBManager.GetNickName(Managers.GoogleSignIn.GetUID()).GetAwaiter();
        GettingNickName.OnCompleted(() => {
            GetObject((int)GameObjects.UserName).gameObject.GetComponentInChildren<TMP_InputField>().text = GettingNickName.GetResult();
        });
    }

    async void InitGetMessage()
    {
        PlayerMessage = await Managers.DBManager.GetUserMessage(Managers.GoogleSignIn.GetUID());
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

}
