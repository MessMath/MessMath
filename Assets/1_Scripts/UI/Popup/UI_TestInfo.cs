using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
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
        //Grace1,
        //Grace2,
        //Grace3,
    }

    enum Texts
    {
        UserNamePlaceholder,
        UserNameText,
        UserMessagePlaceholder,
        UserMessageText,
        GraceText1,
        GraceText2,
        GraceText3,
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

        Managers.DBManager.reference.Child("Users").Child(Managers.UserMng.user.UID).ValueChanged += HandleValueChanged;
        #region CustomizingObject Clear
        GetObject((int)GameObjects.CustomizingObject1).gameObject.GetComponent<Image>().sprite = null;
        GetObject((int)GameObjects.CustomizingObject2).gameObject.GetComponent<Image>().sprite = null;
        GetObject((int)GameObjects.CustomizingObject3).gameObject.GetComponent<Image>().sprite = null;
        #endregion

        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(()=> Managers.UI.ClosePopupUI(this));
        GetButton((int)Buttons.SaveBtn).gameObject.BindEvent(() => OnClickedSaveBtn());

        GetObject((int)GameObjects.UserName).gameObject.GetComponentInChildren<TMP_InputField>().text = Managers.UserMng.user.nickname;
        return true;
    }

    void OnClickedSaveBtn()
    {
        string grace1 = GetText((int)Texts.GraceText1).text;
        string grace2 = GetText((int)Texts.GraceText2).text;
        string grace3 = GetText((int)Texts.GraceText3).text;

        Debug.Log(grace1);
        Debug.Log(grace2);
        Debug.Log(grace3);

        Managers.DBManager.SetNickname(GetText((int)Texts.UserNameText).text);
        Managers.DBManager.SetUserMessage(GetText((int)Texts.UserMessageText).text);
        Managers.DBManager.SetStoryGrace(grace1, grace2, grace3);
    }

}
