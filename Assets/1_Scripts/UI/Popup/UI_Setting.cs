using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Setting : UI_Popup
{
    enum Images
    {
        BGImage,
    }

    enum Texts
    {
    
    }

    enum buttons
    {


    }

    enum GameObjects
    {
    
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));

        GetImage((int)Images.BGImage).gameObject.BindEvent(() => { ClosePopupUI(); });

        return true;
    }

}
