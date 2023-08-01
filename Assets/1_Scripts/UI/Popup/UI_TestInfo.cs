using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TestInfo : UI_Popup
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
        UserNamePlaceholder,
        UserNameText,
        UserMessagePlaceholder,
        UserMessageText,
    }

    enum Buttons
    {
        SaveBtn,
        ExitBtn,
    }

    enum Images
    {
        BG,
        UserImageBG,
        UserImage,

    }

    public override bool Init()
    {
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

        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(()=> Managers.UI.ClosePopupUI(this));
        GetImage((int)Images.UserImageBG).gameObject.BindEvent(() => OnClickedUserImgBG());
        GetButton((int)Buttons.SaveBtn).gameObject.BindEvent(() => OnClickedSaveBtn());

        return true;
    }
    
    void OnClickedSaveBtn()
    {
        Managers.DBManager.SetNickname(GetText((int)Texts.UserNameText).text);
        Managers.DBManager.SetUserMessage(GetText((int)Texts.UserMessageText).text);
    }
    void OnClickedUserImgBG()
    {
        Debug.Log(Managers.DBManager.ReadData(Managers.UserMng.user.UID, "nickname"));
        GetObject((int)GameObjects.UserName).gameObject.GetComponentInChildren<TMP_InputField>().text = Managers.DBManager.ReadData(Managers.UserMng.user.UID, "nickname");
        GetText((int)Texts.UserNamePlaceholder).gameObject.SetActive(false);
    }
    
}
