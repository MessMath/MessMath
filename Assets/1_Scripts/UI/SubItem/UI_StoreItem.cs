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
    enum GameObjects
    {
        Have,
    }
    enum Buttons
    {
        StoreItemButton,
    }
    enum Images
    {
        ItemImage,
        ModeImage,
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

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindText(typeof(Texts));
        return true;
    }
    void Update()
    {
        SetInfo(_storeData);
    }

    void OnClickBtn()
    {
        if (PlayerPrefs.HasKey(_storeData.img)) return;
        UI_Purchase purchasePopup = Managers.UI.ShowPopupUI<UI_Purchase>();
        if (purchasePopup.Init()) purchasePopup.SetPopup(_storeData.name, _storeData.explanation, _storeData.price, _storeData.img);
        //GetButton((int)Buttons.StoreItemButton).gameObject.BindEvent(()=>CancelPurChase(_storeData.img));
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
        GetImage((int)Images.ItemImage).sprite = img;
        SetModeImage(storeData);
        if (PlayerPrefs.HasKey(_storeData.img))
        {
            GetObject((int)GameObjects.Have).SetActive(true);
            GetText((int)Texts.PriceTMP).gameObject.SetActive(false);
            GetImage((int)Images.ItemImage).color = new Color(100 / 255f, 100 / 255f, 100 / 255f, 255 / 255);
        }
        else
        {
            GetObject((int)GameObjects.Have).SetActive(false);
            GetText((int)Texts.PriceTMP).text = _storeData.price.ToString();
            GetButton((int)Buttons.StoreItemButton).gameObject.BindEvent(OnClickBtn);
        }
    }

    public void SetModeImage(StoreData storeData)
    {
        _storeData = storeData;
        if (_storeData.mode == "Both") GetImage((int)Images.ModeImage).sprite = Resources.Load<Sprite>("Sprites/UI/StoreUI/Mode_Both");
        if (_storeData.mode == "Story") GetImage((int)Images.ModeImage).sprite = Resources.Load<Sprite>("Sprites/UI/StoreUI/Mode_Story"); ;
        if (_storeData.mode == "OneToOne") GetImage((int)Images.ModeImage).sprite = Resources.Load<Sprite>("Sprites/UI/StoreUI/Mode_OneToOne");

    }
}