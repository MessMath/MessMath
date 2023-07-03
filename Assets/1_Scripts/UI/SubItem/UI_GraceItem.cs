using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public class GraceInfo
//{
//    public string _name;
//    public string _description;
//    public Image _image;
//}

public class UI_GraceItem : UI_Base
{
    public string _description = "";
    public string _name = "";

   enum GameObjects
    {
        Grace,
    }

    enum Images
    {

    }

    enum Texts
    {
        GraceIconText,

    }

    public override bool Init()
    {

        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        return true;
    }

   
}
