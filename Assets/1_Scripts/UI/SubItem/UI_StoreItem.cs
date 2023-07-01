using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StoreDatas;

public class UI_StoreItem : UI_Base
{
    Sprite img;
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
        return true;
    }

    void OnClickBtn()
    {
        UI_Purchase purchasePopup = Managers.UI.ShowPopupUI<UI_Purchase>();
        if(purchasePopup.Init())purchasePopup.SetPopup(_storeData.name, _storeData.explanation, _storeData.price,_storeData.img);
        if(PlayerPrefs.HasKey(_storeData.img))  GetButton((int)Buttons.StoreItemButton).gameObject.BindEvent(()=>CancelPurChase(_storeData.img));
    }

    void CancelPurChase(string name)
    {
        PlayerPrefs.DeleteKey(name);
        GetButton((int)Buttons.StoreItemButton).gameObject.BindEvent(OnClickBtn);
    }

    public void SetInfo(StoreData storeData)
    {
        _storeData = storeData;
        img = Resources.Load("Sprites/Grace/" + _storeData.img, typeof(Sprite)) as Sprite;
        GetText((int)Texts.NameTMP).text = _storeData.name;
        GetText((int)Texts.PriceTMP).text = _storeData.price.ToString();
        GetImage((int)Images.ItemImage).sprite = img;
        GetButton((int)Buttons.StoreItemButton).gameObject.BindEvent(OnClickBtn);
    }
}