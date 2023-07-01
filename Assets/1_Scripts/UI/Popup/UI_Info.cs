using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        
        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(()=> Managers.UI.ClosePopupUI(this));

        return true;
    }
}
