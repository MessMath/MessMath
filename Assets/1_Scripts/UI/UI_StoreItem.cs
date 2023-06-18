using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StoreDatas;

public class UI_StoreItem : UI_Base
{
    StoreData _storeData;
    enum Buttons
    {
        StoreItemButton,
    }
    enum Images
    {
        ItemImage,
    }
    enum Texts
    {
        NameTMP,
        PriceTMP,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        Debug.Log(GetText((int)Texts.NameTMP).name);
        return true;
    }

    void OnClickBtn()
    {
        Debug.Log(_storeData.explanation);
    }

    public void SetInfo(StoreData storeData)
    {
        _storeData = storeData;
        Debug.Log(GetText((int)Texts.NameTMP).name);
        GetText((int)Texts.NameTMP).text = _storeData.name;
        GetText((int)Texts.PriceTMP).text = _storeData.price.ToString();
        GetImage((int)Images.ItemImage).sprite = Resources.Load("Sprites/Grace/" + _storeData.img, typeof(Sprite)) as Sprite;
        GetButton((int)Buttons.StoreItemButton).gameObject.BindEvent(OnClickBtn);
    }
}
