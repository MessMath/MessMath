using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StoreDatas;
using MessMathI18n;
using System.Threading.Tasks;

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
    float storeContentWidthSizie = 250f;
    float storeContentHeightSizie = 926.87f;

    //List<string> obtainedCollections = new List<string>();

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

        RefreshUI();

        isInitialized = true;
        return true;
    }

    async void RefreshUI()
    {
        List<string> obtainedCollections = Managers.DBManager.ParseObtanined(await Managers.DBManager.GetObtainedCollections(Managers.GoogleSignIn.GetUID()));

        for (int i = 0; i < obtainedCollections.Count; i++)
        {
            if (obtainedCollections[i] == "gauss_token")
                GetImage((int)Images.CoinImg).sprite = Resources.Load<Sprite>("Sprites/Collections/gauss_token");
        }
    }

    void OnClickedGraceBtn()
    {
        Managers.Sound.Play("ClickBtnEff");

        foreach (Transform child in content.transform)
            Managers.Resource.Destroy(child.gameObject);

        GetImage((int)Images.GraceLightBar).gameObject.SetActive(true);
        GetImage((int)Images.CollectionLightBar).gameObject.SetActive(false);
        GetImage((int)Images.ClothesLightBar).gameObject.SetActive(false);
        GetObject((int)GameObjects.StoreContent).gameObject.GetComponent<RectTransform>().sizeDelta= new Vector2(storeContentWidthSizie * graceData.Count, storeContentHeightSizie);

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
        GetObject((int)GameObjects.StoreContent).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(storeContentWidthSizie * collectionData.Count, storeContentHeightSizie);

        for (int i = 0; i < collectionData.Count; i++)
        {
            GameObject item = Managers.UI.MakeSubItem<UI_StoreItem>(content.transform, "StoreItemButton").gameObject;
            UI_StoreItem storeItem = item.GetOrAddComponent<UI_StoreItem>();
            if (storeItem.Init())
                storeItem.SetInfo(collectionData[i]);
        }
    }

    async void OnClickedClothesBtn()
    {
        Managers.Sound.Play("ClickBtnEff");

        foreach (Transform child in content.transform)
            Managers.Resource.Destroy(child.gameObject);

        GetImage((int)Images.GraceLightBar).gameObject.SetActive(false);
        GetImage((int)Images.CollectionLightBar).gameObject.SetActive(false);
        GetImage((int)Images.ClothesLightBar).gameObject.SetActive(true);
        GetObject((int)GameObjects.StoreContent).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(storeContentWidthSizie * ClothesData.Count, storeContentHeightSizie);

        for (int i = 0; i < ClothesData.Count; i++)
        {
            if (ClothesData[i].img == "the_wise" && ! await (Managers.DBManager.GetIsKilledWitch(Managers.GoogleSignIn.GetUID()))) continue;

            GameObject item = Managers.UI.MakeSubItem<UI_StoreItem>(content.transform, "StoreItemButton").gameObject;
            UI_StoreItem storeItem = item.GetOrAddComponent<UI_StoreItem>();
            if (storeItem.Init())
                storeItem.SetInfo(ClothesData[i]);
        }
    }

    public async void SetCoinText()
    {
        GetText((int)Texts.CoinTMP).text = (await Managers.DBManager.GetCoin(Managers.GoogleSignIn.GetUID())).ToString();
    }

    public void SetStorContentSize()
    {

    }
}

