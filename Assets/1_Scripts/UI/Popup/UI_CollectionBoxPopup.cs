using DG.Tweening;
using MessMathI18n;
using Newtonsoft.Json;
using StoreDatas;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class UI_CollectionBoxPopup : UI_Popup
{
    JsonReader _jsonReader;
    List<StoreData> _collectionDatas = new List<StoreData>();
    GameObject selectedObject;

    enum GameObjects
    {
        Content,
    }

    enum Texts
    {
        SelectedCollectionText,
        SelectedCollectionDescription,
        TitleText,
    }

    enum Buttons
    {
        ExitBtn,
    }

    enum Images
    {
        SelectedCollectionImage,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        _jsonReader = new JsonReader();

        if (LocalizationManager.Get().GetSelectedLanguage() == Language.KOREAN)
        {
            _collectionDatas = _jsonReader.ReadStoreJson(Application.persistentDataPath + "/" + 2 + "_StoreCollection_KOR.json").storeDataList;
        }
        else
        {
            _collectionDatas = _jsonReader.ReadStoreJson(Application.persistentDataPath + "/" + 8 + "_StoreCollection_EN.json").storeDataList;
        }

        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(OnClosePopup);

        RefreshUI();

        Debug.Log($"UI_CollectionBoxPopup is opened");

        return true;
    }

    void OnClosePopup()
    {
        // Sound
        Managers.Sound.Play("ClickBtnEff");

        Managers.UI.ClosePopupUI(this);
    }

    void RefreshUI()
    {
        GetText((int)Texts.TitleText).text = I18n.Get(I18nDefine.COLLECTION_TITLE);
        GetText((int)Texts.TitleText).fontSize = 80;

        // Delete Prefab Item
        Transform parent = GetObject((int)GameObjects.Content).gameObject.transform;
        foreach (Transform t in parent)
            Managers.Resource.Destroy(t.gameObject);

        for (int obtainedCollectiondIdx = 0; obtainedCollectiondIdx < Managers.UserMng.GetObtainedGraces().Count - 1; obtainedCollectiondIdx++)
        {
            for (int i = 0; i < _collectionDatas.Count; i++)
            {
                if (Managers.UserMng.GetObtainedCollections()[obtainedCollectiondIdx] != _collectionDatas[i].img) continue;

                GameObject collectionItem = Managers.UI.MakeSubItem<UI_CollectionItem>(GetObject((int)GameObjects.Content).gameObject.transform).gameObject;
                Utils.FindChild(collectionItem, "CollectionIconText", true).GetOrAddComponent<TextMeshProUGUI>().text = _collectionDatas[i].name;
                Utils.FindChild(collectionItem, "CollectionIcon", true).GetOrAddComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Collections/" + _collectionDatas[i].img);
                collectionItem.GetComponent<UI_CollectionItem>()._name = _collectionDatas[i].img;
                collectionItem.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); });
                collectionItem.GetOrAddComponent<UI_CollectionItem>()._description = _collectionDatas[i].explanation;
                collectionItem.GetComponentInChildren<Image>().gameObject.BindEvent(() => { selectedObject = collectionItem; OnClickCollectionBtn(); });
            }
        }
    }

    void OnClickCollectionBtn()
    {
        Managers.Sound.Play("ClickBtnEff");

        GetImage((int)Images.SelectedCollectionImage).gameObject.SetActive(true);

        GetImage((int)Images.SelectedCollectionImage).sprite = Resources.Load<Sprite>("Sprites/Collections/" + selectedObject.GetComponent<UI_CollectionItem>()._name);
        GetText((int)Texts.SelectedCollectionText).text = Utils.FindChild(selectedObject, "CollectionIconText", true).GetOrAddComponent<TextMeshProUGUI>().text;
        //GetText((int)Texts.SelectedGraceDescription).text = selectedObject.GetOrAddComponent<UI_GraceItem>()._description;
        Managers.TextEffect.ApplyTextEffect(selectedObject.GetOrAddComponent<UI_CollectionItem>()._description, GetText((int)Texts.SelectedCollectionDescription), 60);

    }

}
