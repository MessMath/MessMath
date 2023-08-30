using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CollectionItem : UI_Base
{
    public string _description = "";
    public string _name = "";
    public string _img = "";
    public string _price = "";

    enum GameObjects
    {
        Collection,
    }

    enum Texts
    {
        CollectionText,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));

        return true;
    }

}
