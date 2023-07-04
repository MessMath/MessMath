using Newtonsoft.Json;
using StoreDatas;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_GraceBoxPopup : UI_Popup
{
    JsonReader _jsonReader;
    List<StoreData> _graceDatas = new List<StoreData>();
    List<UI_GraceItem> _graces = new List<UI_GraceItem>();
    GameObject selectedObject;

    enum GameObjects
    {
        Content,
    }

    enum Texts
    {
        SelectedGraceText,
        SelectedGraceDescription,
        TitleText,
    }

    enum Buttons
    {
        ExitBtn,
        SelectBtn,
    }

    enum Images
    {
        SelectedGraceImage
    }

    public enum State
    {
        None,
        OneToOne,
        Story,
    }

    public State _state = State.None;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        _jsonReader = new JsonReader();
        _graceDatas = _jsonReader.ReadStoreJson(Application.persistentDataPath + "/" + 1 + "_StoreGauss.json").storeDataList;

        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(OnClosePopup);
        GetButton((int)Buttons.SelectBtn).gameObject.BindEvent(OnClickSelectBtn);
        
        if (_state == State.OneToOne)
        {
            OneToOneModeRefreshUI();
        }
        else if ( _state == State.Story)
        {
            StoryModeRefreshUI();
        }

        return true;
    }

    void OneToOneModeRefreshUI()
    {
        _graces.Clear();
        GetText((int)Texts.TitleText).text = "스토리모드 가호 선택 창"; // 임시로 그냥 텍스트 넣음

        // 프리펩에 보이는 가호 아이콘 정리
        Transform parent = GetObject((int)GameObjects.Content).gameObject.transform;
        foreach (Transform t in parent)
            Managers.Resource.Destroy(t.gameObject);

        // TODO 플레이어 정보로 채우기
        for (int i = 0; i < _graceDatas.Count; i++)
        {
            GameObject graceItem = Managers.UI.MakeSubItem<UI_GraceItem>(GetObject((int)GameObjects.Content).gameObject.transform).gameObject;
            Utils.FindChild(graceItem, "GraceIconText", true).GetOrAddComponent<TextMeshProUGUI>().text = _graceDatas[i].name;
            Utils.FindChild(graceItem, "Grace", true).GetOrAddComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Grace/" + _graceDatas[i].img);
            graceItem.GetComponent<UI_GraceItem>()._name = _graceDatas[i].img;
            graceItem.GetOrAddComponent<UI_GraceItem>()._description = _graceDatas[i].explanation;
            graceItem.GetComponentInChildren<Image>().gameObject.BindEvent(() => { selectedObject = graceItem; OnClickGraceBtn(); });
        }
    }

    void StoryModeRefreshUI()
    {
        _graces.Clear();
        GetText((int)Texts.TitleText).text = "수학자모드 가호 선택 창"; // 임시로 그냥 텍스트 넣음

        // 프리펩에 보이는 가호 아이콘 정리
        Transform parent = GetObject((int)GameObjects.Content).gameObject.transform;
        foreach (Transform t in parent)
            Managers.Resource.Destroy(t.gameObject);

        // TODO 플레이어 정보로 채우기
        for (int i = 0; i < _graceDatas.Count; i++)
        {
            GameObject graceItem = Managers.UI.MakeSubItem<UI_GraceItem>(GetObject((int)GameObjects.Content).gameObject.transform).gameObject;
            Utils.FindChild(graceItem, "GraceIconText", true).GetOrAddComponent<TextMeshProUGUI>().text = _graceDatas[i].name;
            Utils.FindChild(graceItem, "Grace", true).GetOrAddComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Grace/" + _graceDatas[i].img);
            graceItem.GetComponent<UI_GraceItem>()._name = _graceDatas[i].img;
            graceItem.GetOrAddComponent<UI_GraceItem>()._description = _graceDatas[i].explanation;
            graceItem.GetComponentInChildren<Image>().gameObject.BindEvent(() => { selectedObject = graceItem; OnClickGraceBtn(); });
        }
    }

    void OnClosePopup()
    {
        // Sound
        // TODO ClosePopupSound
        Managers.Sound.Play("ClickBtnEff");

        Managers.UI.ClosePopupUI(this);

    }

    void OnClickGraceBtn()
    {
        GetImage((int)Images.SelectedGraceImage).sprite = Utils.FindChild(selectedObject, "Grace").GetOrAddComponent<Image>().sprite;
        GetText((int)Texts.SelectedGraceText).text = Utils.FindChild(selectedObject, "GraceIconText", true).GetOrAddComponent<TextMeshProUGUI>().text;
        GetText((int)Texts.SelectedGraceDescription).text = selectedObject.GetOrAddComponent<UI_GraceItem>()._description;
    }

    void OnClickSelectBtn()
    {
        if (_state == State.OneToOne)
        {
            SettingOneToOneModeGrace();
        }
        else if (_state == State.Story)
        {
            SettingStoryModeGrace();
        }

        //if (Utils.FindChild(gameObject.transform.parent.gameObject, "UI_InventoryPopup") != null) // 인벤토리가 열려있을 때
        //    Utils.FindChild(gameObject.transform.parent.gameObject, "UI_InventoryPopup").GetComponent<UI_InventoryPopup>().Invoke("RefreshUI", 0);

        if (Utils.FindChild(gameObject.transform.parent.gameObject, "UI_SelectGracePopup") != null) // 가호 선택 창이 열려있을 때
            Utils.FindChild(gameObject.transform.parent.gameObject, "UI_SelectGracePopup").GetComponent<UI_SelectGracePopup>().Invoke("RefreshUI", 0);
        
        // Sound
        // TODO ClosePopupSound
        Managers.Sound.Play("ClickBtnEff");

        Managers.UI.ClosePopupUI(this);
    }

    void SettingOneToOneModeGrace()
    {
        for (int i = 0; i < 3; i++)
        {
            if (Managers.Game.SelectGraceInx == i)
            {
                PlayerPrefs.SetString($"SelectedGrace{i}InOneToOne", selectedObject.GetComponent<UI_GraceItem>().name);
            }
        }
    }

    void SettingStoryModeGrace()
    {
        for (int i = 0; i < 3; i++)
        {
            if (Managers.Game.SelectGraceInx == i)
            {
                PlayerPrefs.SetString($"SelectedGrace{i}InStory", selectedObject.GetComponent<UI_GraceItem>().name);
            }
        }
    }
}
