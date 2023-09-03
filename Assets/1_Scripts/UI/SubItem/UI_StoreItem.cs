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
    List<string> obtainedGraces= new List<string>();
    List<string> obtainedClothes= new List<string>();
    List<string> obtainedCollections= new List<string>();

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
        CoinImage,
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

        InitObtainedClothes();
        InitObtainedCollections();
        InitObtainedGraces();

        return true;
    }

    async void InitObtainedClothes()
    {
        obtainedClothes = Managers.DBManager.ParseObtanined(await Managers.DBManager.GetObtainedClothes(Managers.GoogleSignIn.GetUID()));
    }

    async void InitObtainedCollections()
    {
        obtainedCollections = Managers.DBManager.ParseObtanined(await Managers.DBManager.GetObtainedCollections(Managers.GoogleSignIn.GetUID()));
    }

    async void InitObtainedGraces()
    {
        obtainedGraces = Managers.DBManager.ParseObtanined(await Managers.DBManager.GetObtainedGraces(Managers.GoogleSignIn.GetUID()));
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

        RefreshUI();
    }

    public void RefreshUI()
    {
        if (obtainedCollections == null) return;

        for (int i = 0; i < obtainedCollections.Count; i++)
        {
            if (obtainedCollections[i] != "gauss_token") continue;

            GetImage((int)Images.CoinImage).sprite = Resources.Load<Sprite>("Sprites/Collections/gauss_token");
            GetImage((int)Images.CoinImage).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(70, 70);
        }
    }

    public bool IsHave(StoreData storeData)
    {
        if (storeData.mode == "clothes" && obtainedClothes != null)
            for (int i = 0; i < obtainedClothes.Count - 1; i++)
            {
                if (obtainedClothes[i] == storeData.img) return true;
            }

        if (storeData.mode == "collection" && obtainedCollections != null)
            for (int i = 0; i < obtainedCollections.Count - 1; i++)
            {
                if (obtainedCollections[i] == storeData.img) return true;
            }

        if (obtainedGraces != null)
            for (int i = 0; i < obtainedGraces.Count - 1; i++)
            {
                if (obtainedGraces[i] == storeData.img) return true;
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