using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Setting : UI_Popup
{
    enum Images
    {
        BGImage,
        Panel,
    }

    enum Texts
    {
    
    }

    enum Buttons
    {
        ContinueBtn,
        SetSoundBtn,
        ChangeLeftRightBtn,
        ExitBtn,
    }

    enum GameObjects
    {
    
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindButton(typeof(Buttons));

        GetImage((int)Images.Panel).gameObject.BindEvent(() => { ClosePopupUI(); });

        GetButton((int)Buttons.ContinueBtn).gameObject.BindEvent(() => { ClosePopupUI(); });
        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(() => { Managers.Scene.ChangeScene(Define.Scene.LobbyScene); });

        return true;
    }

}
