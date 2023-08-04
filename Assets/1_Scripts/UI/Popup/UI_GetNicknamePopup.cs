using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_GetNicknamePopup : UI_Popup
{
    enum GameObjects
    {
        UserName,
    }

    enum Texts
    {
        UserNameText,
    }

    enum Buttons
    {
        Save,
        Next,
        ExitBtn,
    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.Save).gameObject.BindEvent(() => OnClickedSaveBtn());
        GetButton((int)Buttons.Next).gameObject.BindEvent(() => NextPopup());
        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(() => Managers.UI.ClosePopupUI(this));
        return true;
    }

    void OnClickedSaveBtn()
    {
        Managers.DBManager.SetNickname(GetText((int)Texts.UserNameText).text);
    }
    
    void NextPopup()
    {
        Managers.UI.ShowPopupUI<UI_TestInfo>();
    }
}
