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

public class UI_ClothesBoxPopup : UI_Popup
{
    JsonReader _jsonReader;
    List<StoreData> _clothesDatas = new List<StoreData>();
    GameObject selectedObject;

    enum GameObjects
    {
        Content,
    }

    enum Texts
    {
        SelectedClothesText,
        SelectedClothesDescription,
        TitleText,
        SelectText,
    }

    enum Buttons
    {
        ExitBtn,
        SelectBtn,
    }

    enum Images
    {
        PresentClothesImage,
        SelectedClothesImage,
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
            _clothesDatas = _jsonReader.ReadStoreJson(Application.persistentDataPath + "/" + 5 + "_StoreClothes_KOR.json").storeDataList;
        }
        else
        {
            _clothesDatas = _jsonReader.ReadStoreJson(Application.persistentDataPath + "/" + 11 + "_StoreClothes_EN.json").storeDataList;
        }

        GetText((int)Texts.SelectText).text = I18n.Get(I18nDefine.CLOTHES_SELECT);
        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(OnClosePopup);
        GetButton((int)Buttons.SelectBtn).gameObject.BindEvent(OnClickSelectBtn);
        GetImage((int)Images.SelectedClothesImage).gameObject.SetActive(false);

        if (PlayerPrefs.HasKey("WearClothes"))
            GetImage((int)Images.PresentClothesImage).sprite = Resources.Load<Sprite>("Sprites/Clothes/" + (PlayerPrefs.GetString("WearClothes")));
        else
            GetImage((int)Images.PresentClothesImage).sprite = Resources.Load<Sprite>("Sprites/Clothes/lobby_Character");

        RefreshUI();

        Debug.Log($"UI_ClothesBoxPopup is opened");

        return true;
    }

    void RefreshUI()
    {
        GetText((int)Texts.TitleText).text = I18n.Get(I18nDefine.CLOTHES_TITLE);
        GetText((int)Texts.TitleText).fontSize = 80;

        // Delete Prefab Item
        Transform parent = GetObject((int)GameObjects.Content).gameObject.transform;
        foreach (Transform t in parent)
            Managers.Resource.Destroy(t.gameObject);

        if (Managers.UserMng.GetObtainedClothes() == null) return; // ø ¿Â ≈÷ ∫Òæ˙¿ª ∂ß

        // TODO Add Item as User Data
        for (int i = 0; i < Managers.UserMng.GetObtainedClothes().Count - 1; i++)
        { 
            //if (PlayerPrefs.HasKey(_clothesDatas[i].img) && PlayerPrefs.GetString(_clothesDatas[i].img) != "")
            for(int j = 0; j < _clothesDatas.Count; j++)
            {
                if (Managers.UserMng.GetObtainedClothes()[i] == _clothesDatas[j].img)
                {
                    Debug.Log(Managers.UserMng.GetObtainedClothes()[i]);
                    GameObject clothesItem = Managers.UI.MakeSubItem<UI_ClothesItem>(GetObject((int)GameObjects.Content).gameObject.transform).gameObject;
                    Utils.FindChild(clothesItem, "ClothesIconText", true).GetOrAddComponent<TextMeshProUGUI>().text = _clothesDatas[j].name;
                    Utils.FindChild(clothesItem, "ClothesIcon", true).GetOrAddComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Clothes/" + _clothesDatas[j].img);
                    clothesItem.GetComponent<UI_ClothesItem>()._name = _clothesDatas[j].img;
                    clothesItem.GetComponent<UI_ClothesItem>()._img = _clothesDatas[j].img;
                    clothesItem.GetComponent<UI_ClothesItem>()._fullImg = _clothesDatas[j].bgImage;
                    string[] fullImageName = _clothesDatas[j].img2.Split('\r');
                    //clothesItem.GetComponent<UI_ClothesItem>()._fullImg = fullImageName[0];
                    clothesItem.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); });
                    clothesItem.GetOrAddComponent<UI_ClothesItem>()._description = _clothesDatas[j].explanation;
                    clothesItem.GetComponentInChildren<Image>().gameObject.BindEvent(() => { selectedObject = clothesItem; OnClickClothesBtn(); });
                }
            }
        }
    }

    void OnClosePopup()
    {
        // Sound
        Managers.Sound.Play("ClickBtnEff");

        Managers.UI.ClosePopupUI(this);
    }

    void OnClickClothesBtn()
    {
        Managers.Sound.Play("ClickBtnEff");

        GetImage((int)Images.SelectedClothesImage).gameObject.SetActive(true);

        GetImage((int)Images.SelectedClothesImage).sprite = Resources.Load<Sprite>("Sprites/Clothes/" + selectedObject.GetComponent<UI_ClothesItem>()._img);
        GetText((int)Texts.SelectedClothesText).text = Utils.FindChild(selectedObject, "ClothesIconText", true).GetOrAddComponent<TextMeshProUGUI>().text;
        //GetText((int)Texts.SelectedGraceDescription).text = selectedObject.GetOrAddComponent<UI_GraceItem>()._description;
        Managers.TextEffect.ApplyTextEffect(selectedObject.GetOrAddComponent<UI_ClothesItem>()._description, GetText((int)Texts.SelectedClothesDescription), 60);

    }

    void OnClickSelectBtn()
    {
        if (selectedObject == null) { OnClosePopup(); return; }

        PlayerPrefs.SetString("WearClothes", selectedObject.GetComponent<UI_ClothesItem>()._img);

        gameObject.transform.parent.gameObject.GetComponent<UI_Lobby>().Invoke("RefreshUI", 0);
        Utils.FindChild(gameObject.transform.parent.gameObject, "UI_InventoryPopup").GetComponent<UI_InventoryPopup>().Invoke("RefreshUI", 0);

        // Sound
        Managers.Sound.Play("ClickBtnEff");

        Managers.UI.ClosePopupUI(this);
    }

}
