using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StoreDatas;

public class UI_Store : UI_Popup
{
    JsonReader jsonReader;
    List<StoreData> storeData = new List<StoreData>();
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
        CategoryButton,
        //ItemButton,
    }

    enum GameObjects
    {
        CategoryPanel,
        StoreContent,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        jsonReader = new JsonReader();
        storeData = jsonReader.ReadStoreJson(Application.persistentDataPath + "/" + 1 + "_StoreGaus.json").storeDataList;
        Debug.Log("storeDataListCnt: " + storeData.Count);

        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(() => { ClosePopupUI(); });

        GameObject content = GetObject((int)GameObjects.StoreContent);
    
        foreach (Transform child in content.transform)
            Managers.Resource.Destroy(child.gameObject);

        for (int i = 0; i < storeData.Count; i++)
        {
            GameObject item = Managers.UI.MakeSubItem<UI_StoreItem>(content.transform, "StoreItemButton").gameObject; 
            UI_StoreItem storeItem = item.GetOrAddComponent<UI_StoreItem>();
            if(storeItem.Init()) 
                storeItem.SetInfo(storeData[i]);
        }

        return true;
    }
}
