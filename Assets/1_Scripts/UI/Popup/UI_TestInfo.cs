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
        Grace1,
        Grace2,
        Grace3,
    }

    enum Texts
    {
        UserNamePlaceholder,
        UserNameText,
        UserMessagePlaceholder,
        UserMessageText,
        Grace1Text,
        Grace2Text,
        Grace3Text,
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

        Managers.DBManager.reference.Child("Users").Child(Managers.UserMng.user.UID).ValueChanged += HandleValueChanged;

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
        GetButton((int)Buttons.SaveBtn).gameObject.BindEvent(() => OnClickedSaveBtn());

        return true;
    }

    void OnClickedSaveBtn()
    {
        string grace1 = GetText((int)Texts.Grace1Text).text;
        string grace2 = GetText((int)Texts.Grace2Text).text;
        string grace3 = GetText((int)Texts.Grace3Text).text;

        Debug.Log(grace1);
        Debug.Log(grace2);
        Debug.Log(grace3);

        Managers.DBManager.SetNickname(GetText((int)Texts.UserNameText).text);
        Managers.DBManager.SetUserMessage(GetText((int)Texts.UserMessageText).text);
        Managers.DBManager.SetStoryGrace(grace1, grace2, grace3);
    }

    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError("DatabaseError: " + args.DatabaseError.Message);
            return;
        }

        if (args.Snapshot != null && args.Snapshot.Exists)
        {
            string newNickname = args.Snapshot.Child("nickname").Value.ToString();
            GetObject((int)GameObjects.UserName).gameObject.GetComponentInChildren<TMP_InputField>().text = newNickname;
        }
        else
        {
            Debug.LogWarning("Data not found in the database.");
        }
    }
}
