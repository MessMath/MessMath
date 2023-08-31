using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ClothesItem : UI_Base
{
    public string _description = "";
    public string _name = "";
    public string _img = "";
    public string _price = "";
    public string _fullImg = "";

    enum GameObjects
    {
        Clothes,
    }

    enum Texts
    {
        ClothesIconText,
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
