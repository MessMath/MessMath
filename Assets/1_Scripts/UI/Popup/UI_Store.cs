using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StoreDatas;
using MessMathI18n;

public class UI_Store : UI_Popup
{
    JsonReader jsonReader;
    List<StoreData> graceData = new List<StoreData>();
    List<StoreData> collectionData = new List<StoreData>();
    List<StoreData> ClothesData = new List<StoreData>();
    GameObject content;
    Color unclickedColor = new Color32(217, 217, 217, 255);
    Color clickedColor = new Color32(241, 148, 148, 255);
    bool isInitialized = false;

    enum Images
    {
        CoinImg,
        ShopKeeperImage,
        GraceLightBar,
        CollectionLightBar,
        ClothesLightBar,
    }

    enum Texts
    {
        CoinTMP,
        TitleTMP,
        GraceBtnTMP,
        CollectionBtnTMP,
        ClothesBtnTMP,
    }

    enum Buttons
    {
        ExitButton,
        GraceButton,
        CollectionButton,
        ClothesButton,
    }

    enum GameObjects
    {
        CategoryPanel,
        StoreContent,
    }
    void Update()
    {
        if (isInitialized)
            SetCoinText();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));

        SetCoinText();

        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); ClosePopupUI(); });
        GetButton((int)Buttons.GraceButton).gameObject.BindEvent(OnClickedGraceBtn);
        GetButton((int)Buttons.CollectionButton).gameObject.BindEvent(OnClickedCollectionBtn);
        GetButton((int)Buttons.ClothesButton).gameObject.BindEvent(OnClickedClothesBtn);

        GetText((int)Texts.TitleTMP).text = I18n.Get(I18nDefine.STORE_MATHEMATICS_BOOK);
        GetText((int)Texts.GraceBtnTMP).text = I18n.Get(I18nDefine.STORE_GRACE);
        GetText((int)Texts.CollectionBtnTMP).text = I18n.Get(I18nDefine.STORE_COLLECTIONS);
        GetText((int)Texts.ClothesBtnTMP).text = I18n.Get(I18nDefine.STORE_CLOTHES);

        content = GetObject((int)GameObjects.StoreContent);
        jsonReader = new JsonReader();

        if(LocalizationManager.Get().GetSelectedLanguage() == Language.KOREAN)
        {
            graceData = jsonReader.ReadStoreJson(Application.persistentDataPath + "/" + 1 + "_StoreGrace_KOR.json").storeDataList;
            collectionData = jsonReader.ReadStoreJson(Application.persistentDataPath + "/" + 2 + "_StoreCollection_KOR.json").storeDataList;
            ClothesData = jsonReader.ReadStoreJson(Application.persistentDataPath + "/" + 5 + "_StoreClothes_KOR.json").storeDataList;
        }
        else
        {
            graceData = jsonReader.ReadStoreJson(Application.persistentDataPath + "/" + 7 + "_StoreGrace_EN.json").storeDataList;
            collectionData = jsonReader.ReadStoreJson(Application.persistentDataPath + "/" + 8 + "_StoreCollection_EN.json").storeDataList;
            ClothesData = jsonReader.ReadStoreJson(Application.persistentDataPath + "/" + 11 + "_StoreClothes_EN.json").storeDataList; //StoreClothes_EN
        }

        OnClickedGraceBtn();

        isInitialized = true;
        return true;
    }

    void OnClickedGraceBtn()
    {
        Managers.Sound.Play("ClickBtnEff");

        foreach (Transform child in content.transform)
            Managers.Resource.Destroy(child.gameObject);

        GetImage((int)Images.GraceLightBar).gameObject.SetActive(true);
        GetImage((int)Images.CollectionLightBar).gameObject.SetActive(false);
        GetImage((int)Images.ClothesLightBar).gameObject.SetActive(false);

        for (int i = 0; i < graceData.Count; i++)
        {
            GameObject item = Managers.UI.MakeSubItem<UI_StoreItem>(content.transform, "StoreItemButton").gameObject;
            item.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); });
            UI_StoreItem storeItem = item.GetOrAddComponent<UI_StoreItem>();
            if (storeItem.Init())
                storeItem.SetInfo(graceData[i]);
        }
    }

    void OnClickedCollectionBtn()
    {
        Managers.Sound.Play("ClickBtnEff");

        foreach (Transform child in content.transform)
            Managers.Resource.Destroy(child.gameObject);

        GetImage((int)Images.GraceLightBar).gameObject.SetActive(false);
        GetImage((int)Images.CollectionLightBar).gameObject.SetActive(true);
        GetImage((int)Images.ClothesLightBar).gameObject.SetActive(false);

        for (int i = 0; i < collectionData.Count; i++)
        {
            GameObject item = Managers.UI.MakeSubItem<UI_StoreItem>(content.transform, "StoreItemButton").gameObject;
            UI_StoreItem storeItem = item.GetOrAddComponent<UI_StoreItem>();
            if (storeItem.Init())
                storeItem.SetInfo(collectionData[i]);
        }
    }

    void OnClickedClothesBtn()
    {
        Managers.Sound.Play("ClickBtnEff");

        foreach (Transform child in content.transform)
            Managers.Resource.Destroy(child.gameObject);

        GetImage((int)Images.GraceLightBar).gameObject.SetActive(false);
        GetImage((int)Images.CollectionLightBar).gameObject.SetActive(false);
        GetImage((int)Images.ClothesLightBar).gameObject.SetActive(true);

        for (int i = 0; i < ClothesData.Count; i++)
        {
            GameObject item = Managers.UI.MakeSubItem<UI_StoreItem>(content.transform, "StoreItemButton").gameObject;
            UI_StoreItem storeItem = item.GetOrAddComponent<UI_StoreItem>();
            if (storeItem.Init())
                storeItem.SetInfo(ClothesData[i]);
        }
    }

    public void SetCoinText()
    {
        GetText((int)Texts.CoinTMP).text = Managers.UserMng.GetCoin().ToString();
    }
}

