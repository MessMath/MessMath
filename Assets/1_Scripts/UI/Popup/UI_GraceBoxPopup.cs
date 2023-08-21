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

        Debug.Log($"UI_GraceBoxPopup's state is {_state}");
        if (_state == State.OneToOne)
        {
            OneToOneModeRefreshUI();
        }
        else if (_state == State.Story)
        {
            StoryModeRefreshUI();
        }

        return true;
    }

    void OneToOneModeRefreshUI()
    {
        _graces.Clear();
        GetText((int)Texts.TitleText).text = Managers.GetText(Define.OneToOneModeSelectGracePopupText);
        GetText((int)Texts.TitleText).fontSize = 65;

        // Delete Prefab Item
        Transform parent = GetObject((int)GameObjects.Content).gameObject.transform;
        foreach (Transform t in parent)
            Managers.Resource.Destroy(t.gameObject);

        // TODO Add Item as User Data
        for (int i = 0; i < _graceDatas.Count; i++)
        {
            if (PlayerPrefs.HasKey(_graceDatas[i].img) && PlayerPrefs.GetString(_graceDatas[i].img) != "")
            {
                GameObject graceItem = Managers.UI.MakeSubItem<UI_GraceItem>(GetObject((int)GameObjects.Content).gameObject.transform).gameObject;
                Utils.FindChild(graceItem, "GraceIconText", true).GetOrAddComponent<TextMeshProUGUI>().text = _graceDatas[i].name;
                Utils.FindChild(graceItem, "Grace", true).GetOrAddComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Grace/" + _graceDatas[i].img);
                graceItem.GetComponent<UI_GraceItem>()._name = _graceDatas[i].img;
                graceItem.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); });
                graceItem.GetOrAddComponent<UI_GraceItem>()._description = _graceDatas[i].explanation;
                graceItem.GetComponentInChildren<Image>().gameObject.BindEvent(() => { selectedObject = graceItem; OnClickGraceBtn(); });
            }
        }
    }

    void StoryModeRefreshUI()
    {
        _graces.Clear();
        GetText((int)Texts.TitleText).text = Managers.GetText(Define.StoryModeSelectGracePopupText);

        // Delete Prefab Item
        Transform parent = GetObject((int)GameObjects.Content).gameObject.transform;
        foreach (Transform t in parent)
            Managers.Resource.Destroy(t.gameObject);

        // TODO Add Item as User Data
        for (int i = 0; i < _graceDatas.Count; i++)
        {
            if (PlayerPrefs.HasKey(_graceDatas[i].img) && PlayerPrefs.GetString(_graceDatas[i].img) != "")
            {
                GameObject graceItem = Managers.UI.MakeSubItem<UI_GraceItem>(GetObject((int)GameObjects.Content).gameObject.transform).gameObject;
                Utils.FindChild(graceItem, "GraceIconText", true).GetOrAddComponent<TextMeshProUGUI>().text = _graceDatas[i].name;
                Utils.FindChild(graceItem, "Grace", true).GetOrAddComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Grace/" + _graceDatas[i].img);
                graceItem.GetComponent<UI_GraceItem>()._name = _graceDatas[i].img;
                graceItem.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); });
                graceItem.GetOrAddComponent<UI_GraceItem>()._description = _graceDatas[i].explanation;
                graceItem.GetComponentInChildren<Image>().gameObject.BindEvent(() => { selectedObject = graceItem; OnClickGraceBtn(); });
            }
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
        Managers.Sound.Play("ClickBtnEff");

        GetImage((int)Images.SelectedGraceImage).sprite = Utils.FindChild(selectedObject, "Grace").GetOrAddComponent<Image>().sprite;
        GetText((int)Texts.SelectedGraceText).text = Utils.FindChild(selectedObject, "GraceIconText", true).GetOrAddComponent<TextMeshProUGUI>().text;
        //GetText((int)Texts.SelectedGraceDescription).text = selectedObject.GetOrAddComponent<UI_GraceItem>()._description;
        Managers.TextEffect.ApplyTextEffect(selectedObject.GetOrAddComponent<UI_GraceItem>()._description, GetText((int)Texts.SelectedGraceDescription), 60);

    }

    void OnClickSelectBtn()
    {
        if (selectedObject == null) { OnClosePopup(); return; }

        if (_state == State.OneToOne)
        {
            CheckSameGrace();
            SettingOneToOneModeGrace();
            if (Utils.FindChild(gameObject.transform.parent.gameObject, "UI_SelectGracePopup") != null)
                Utils.FindChild(gameObject.transform.parent.gameObject, "UI_SelectGracePopup").GetComponent<UI_SelectGracePopup>().Invoke("OneToOneModeRefreshUI", 0);
        }
        else if (_state == State.Story)
        {
            CheckSameGrace();
            SettingStoryModeGrace();
            if (Utils.FindChild(gameObject.transform.parent.gameObject, "UI_SelectGracePopup") != null)
                Utils.FindChild(gameObject.transform.parent.gameObject, "UI_SelectGracePopup").GetComponent<UI_SelectGracePopup>().Invoke("StoryModeRefreshUI", 0);
        }

        // Sound
        // TODO ClosePopupSound
        Managers.Sound.Play("ClickBtnEff");

        Managers.UI.ClosePopupUI(this);
    }

    void CheckSameGrace()
    {
        string selectedObjectName = selectedObject.GetComponent<UI_GraceItem>()._name;

        if ( _state == State.OneToOne)
        {
            for (int i = 0; i < 3; i++)
            {
                if (selectedObjectName == PlayerPrefs.GetString($"SelectedGrace{i}InOneToOne"))
                {
                    PlayerPrefs.SetString($"SelectedGrace{i}InOneToOne", "");
                }
            }
        }
        else if (_state == State.Story)
        {
            for (int i = 0; i < 3; i++)
            {
                if (selectedObjectName == PlayerPrefs.GetString($"SelectedGrace{i}InStory"))
                {
                    PlayerPrefs.SetString($"SelectedGrace{i}InStory", "");
                }
            }
        }
    }

    void SettingOneToOneModeGrace()
    {
        Debug.Log("Init SettingOneToOneModeGrace");
        Debug.Log(Managers.Game.SelectGraceInx);
        for (int i = 0; i < 3; i++)
        {
            if (Managers.Game.SelectGraceInx == i)
            {
                PlayerPrefs.SetString($"SelectedGrace{i}InOneToOne", selectedObject.GetComponent<UI_GraceItem>()._name);
                Debug.Log(PlayerPrefs.GetString($"SelectedGrace{i}InOneToOne"));
            }
        }
    }

    void SettingStoryModeGrace()
    {
        Debug.Log("Init SettingStoryModeGrace");
        Debug.Log(Managers.Game.SelectGraceInx);
        for (int i = 0; i < 3; i++)
        {
            if (Managers.Game.SelectGraceInx == i)
            {
                PlayerPrefs.SetString($"SelectedGrace{i}InStory", selectedObject.GetComponent<UI_GraceItem>()._name);
                Debug.Log(PlayerPrefs.GetString($"SelectedGrace{i}InStory"));
            }
        }
    }
}
