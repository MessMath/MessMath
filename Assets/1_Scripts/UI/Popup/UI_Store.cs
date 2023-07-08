using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StoreDatas;

public class UI_Store : UI_Popup
{
    JsonReader jsonReader;
    List<StoreData> graceData = new List<StoreData>();
    List<StoreData> collectionData = new List<StoreData>();
    GameObject content;
    Color unclickedColor = new Color32(217, 217, 217, 255);
    Color clickedColor = new Color32(241, 148, 148, 255);
    bool isInitialized = false;
    enum Images
    {
        CoinImg,
        ShopKeeperImage,
    }

    enum Texts
    {
        CoinTMP,
    }

    enum Buttons
    {
        ExitButton,
        GraceButton,
        CollectionButton,
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

        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(() => { ClosePopupUI(); });
        GetButton((int)Buttons.GraceButton).gameObject.BindEvent(OnClickedGraceBtn);
        GetButton((int)Buttons.CollectionButton).gameObject.BindEvent(OnClickedCollectionBtn);

        content = GetObject((int)GameObjects.StoreContent);
        jsonReader = new JsonReader();
        graceData = jsonReader.ReadStoreJson(Application.persistentDataPath + "/" + 1 + "_StoreGauss.json").storeDataList;
        collectionData = jsonReader.ReadStoreJson(Application.persistentDataPath + "/" + 2 + "_StoreCollection.json").storeDataList;

        OnClickedGraceBtn();

        isInitialized = true;
        return true;
    }

    void OnClickedGraceBtn()
    {
        GetButton((int)Buttons.GraceButton).gameObject.GetComponent<Image>().color = clickedColor;
        GetButton((int)Buttons.CollectionButton).gameObject.GetComponent<Image>().color = unclickedColor;
        foreach (Transform child in content.transform)
            Managers.Resource.Destroy(child.gameObject);

        for (int i = 0; i < graceData.Count; i++)
        {
            GameObject item = Managers.UI.MakeSubItem<UI_StoreItem>(content.transform, "StoreItemButton").gameObject;
            UI_StoreItem storeItem = item.GetOrAddComponent<UI_StoreItem>();
            if (storeItem.Init())
                storeItem.SetInfo(graceData[i]);
        }
    }

    void OnClickedCollectionBtn()
    {
        GetButton((int)Buttons.GraceButton).gameObject.GetComponent<Image>().color = unclickedColor;
        GetButton((int)Buttons.CollectionButton).gameObject.GetComponent<Image>().color = clickedColor;
        foreach (Transform child in content.transform)
            Managers.Resource.Destroy(child.gameObject);

        for (int i = 0; i < collectionData.Count; i++)
        {
            GameObject item = Managers.UI.MakeSubItem<UI_StoreItem>(content.transform, "StoreItemButton").gameObject;
            UI_StoreItem storeItem = item.GetOrAddComponent<UI_StoreItem>();
            if (storeItem.Init())
                storeItem.SetInfo(collectionData[i]);
        }
    }

    public void SetCoinText()
    {
        if (PlayerPrefs.HasKey("Coin"))
            GetText((int)Texts.CoinTMP).text = PlayerPrefs.GetInt("Coin").ToString();
        else { PlayerPrefs.SetInt("Coin", 0); GetText((int)Texts.CoinTMP).text = PlayerPrefs.GetInt("Coin").ToString(); }
    }
}

