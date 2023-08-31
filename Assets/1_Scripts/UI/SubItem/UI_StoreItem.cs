using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StoreDatas;
using MessMathI18n;

public class UI_StoreItem : UI_Base
{
    Sprite img;
    public StoreData _storeData;
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
        HaveTMP,
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

    }

    void OnClickBtn()
    {
        if (PlayerPrefs.HasKey(_storeData.img))
            return;
        UI_Purchase purchasePopup = Managers.UI.ShowPopupUI<UI_Purchase>();
        if (purchasePopup.Init())
            purchasePopup.SetPopup(_storeData);
    }

    void CancelPurChase(string name)
    {
        PlayerPrefs.DeleteKey(name);
        //GetButton((int)Buttons.StoreItemButton).gameObject.BindEvent(OnClickBtn);
    }

    public void SetInfo(StoreData storeData)
    {
        _storeData = storeData;
        img = Resources.Load("Sprites/Grace/" + _storeData.img, typeof(Sprite)) as Sprite;
        if (_storeData.mode == "collection") img = Resources.Load("Sprites/Collections/" + _storeData.img, typeof(Sprite)) as Sprite;
        if (_storeData.mode == "clothes") img = Resources.Load("Sprites/Clothes/" + _storeData.img, typeof(Sprite)) as Sprite;
        GetText((int)Texts.NameTMP).text = _storeData.name;
        GetImage((int)Images.ItemImage).sprite = img;
        SetModeImage(storeData);
        if (IsHave(storeData))
        {
            GetObject((int)GameObjects.Have).SetActive(true);
            GetText((int)Texts.HaveTMP).text = I18n.Get(I18nDefine.STORE_PURCHASED);
            GetText((int)Texts.PriceTMP).gameObject.SetActive(false);
            GetImage((int)Images.ItemImage).color = new Color(100 / 255f, 100 / 255f, 100 / 255f, 255 / 255);
        }
        else
        {
            GetObject((int)GameObjects.Have).SetActive(false);
            GetText((int)Texts.PriceTMP).text = _storeData.price.ToString();
            GetButton((int)Buttons.StoreItemButton).gameObject.BindEvent(() => OnClickBtn());

        }
    }

    public bool IsHave(StoreData storeData)
    {
        if (storeData.mode == "clothes" && Managers.UserMng.GetObtainedClothes() != null)
            for (int i = 0; i < Managers.UserMng.GetObtainedClothes().Count - 1; i++)
            {
                if (Managers.UserMng.GetObtainedClothes()[i] == storeData.img) return true;
            }

        if (storeData.mode == "collection" && Managers.UserMng.GetObtainedCollections() != null)
            for (int i = 0; i < Managers.UserMng.GetObtainedCollections().Count - 1; i++)
            {
                if (Managers.UserMng.GetObtainedCollections()[i] == storeData.img) return true;
            }

        if (Managers.UserMng.GetObtainedGraces() != null)
            for (int i = 0; i < Managers.UserMng.GetObtainedGraces().Count - 1; i++)
            {
                if (Managers.UserMng.GetObtainedGraces()[i] == storeData.img) return true;
            }

        return false;
    }

    public void SetModeImage(StoreData storeData)
    {
        _storeData = storeData;
        if (_storeData.mode == "Both") GetImage((int)Images.ModeImage).sprite = Resources.Load<Sprite>("Sprites/UI/StoreUI/Mode_Both");
        if (_storeData.mode == "Story") GetImage((int)Images.ModeImage).sprite = Resources.Load<Sprite>("Sprites/UI/StoreUI/Mode_Story"); ;
        if (_storeData.mode == "OneToOne") GetImage((int)Images.ModeImage).sprite = Resources.Load<Sprite>("Sprites/UI/StoreUI/Mode_OneToOne");
        else GetImage((int)Images.ModeImage).gameObject.SetActive(false);
    }
}