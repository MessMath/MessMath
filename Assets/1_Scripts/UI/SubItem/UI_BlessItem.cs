using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BlessItem : UI_Base
{
   enum GameObjects
    {
        Bless,
    }

    enum Images
    {
        BlessIcon,
    }

    enum Texts
    {
        BlessIconText,
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
