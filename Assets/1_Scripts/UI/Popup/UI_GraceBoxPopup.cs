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
        SelectText,
    }

    enum Buttons
    {
        ExitBtn,
        SelectBtn,
    }

    enum Images
    {
        SelectedGraceBGImage,
        SelectedGraceImage,
    }

    public enum State
    {
        None,
        OneToOne,
        Story,
    }

    List<string> obtainedGraces = new List<string>();
    public State _state = State.None;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        InitObtainedGraces();

        _jsonReader = new JsonReader();

        if (LocalizationManager.Get().GetSelectedLanguage() == Language.KOREAN)
        {
            _graceDatas = _jsonReader.ReadStoreJson(Application.persistentDataPath + "/" + 1 + "_StoreGrace_KOR.json").storeDataList;
        }
        else
        {
            _graceDatas = _jsonReader.ReadStoreJson(Application.persistentDataPath + "/" + 7 + "_StoreGrace_EN.json").storeDataList;
        }

        GetText((int)Texts.SelectText).text = I18n.Get(I18nDefine.GRACE_BOX_SELECT);
        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(OnClosePopup);
        GetButton((int)Buttons.SelectBtn).gameObject.BindEvent(OnClickSelectBtn);
        GetImage((int)Images.SelectedGraceBGImage).gameObject.SetActive(false);
        GetImage((int)Images.SelectedGraceImage).gameObject.SetActive(false);

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

    void InitObtainedGraces()
    {
        var GettingGraces = Managers.DBManager.GetObtainedGraces(Managers.GoogleSignIn.GetUID()).GetAwaiter();
        GettingGraces.OnCompleted(() => {
            obtainedGraces = Managers.DBManager.ParseObtanined(GettingGraces.GetResult());
        });
    }

    async void OneToOneModeRefreshUI()
    {
        obtainedGraces = Managers.DBManager.ParseObtanined(await Managers.DBManager.GetObtainedGraces(Managers.GoogleSignIn.GetUID()));
        _graces.Clear();
        GetText((int)Texts.TitleText).text = I18n.Get(I18nDefine.GRACE_BOX_ONE_TO_ONE_MODE_TILE);
        GetText((int)Texts.TitleText).fontSize = 65;

        // Delete Prefab Item
        Transform parent = GetObject((int)GameObjects.Content).gameObject.transform;
        foreach (Transform t in parent)
            Managers.Resource.Destroy(t.gameObject);

        if (obtainedGraces == null) return;

        for (int i = 0; i < obtainedGraces.Count - 1; i++)
        {
            for (int j = 0; j < _graceDatas.Count; j++)
            {
                if (obtainedGraces[i] != _graceDatas[j].img) continue;
                if (_graceDatas[j].mode == "Story") continue;

                GameObject graceItem = Managers.UI.MakeSubItem<UI_GraceItem>(GetObject((int)GameObjects.Content).gameObject.transform).gameObject;
                Utils.FindChild(graceItem, "GraceIconText", true).GetOrAddComponent<TextMeshProUGUI>().text = _graceDatas[j].name;
                Utils.FindChild(graceItem, "Grace", true).GetOrAddComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Grace/" + _graceDatas[j].img);
                graceItem.GetComponent<UI_GraceItem>()._name = _graceDatas[j].img;
                graceItem.GetComponent<UI_GraceItem>().BgImage = _graceDatas[j].bgImage;
                string[] fullImageName = _graceDatas[j].img2.Split('\r');
                graceItem.GetComponent<UI_GraceItem>().FullImage = fullImageName[0];
                graceItem.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); });
                graceItem.GetOrAddComponent<UI_GraceItem>()._description = _graceDatas[j].explanation;
                graceItem.GetComponentInChildren<Image>().gameObject.BindEvent(() => { selectedObject = graceItem; OnClickGraceBtn(); });

            }
        }

    }

    async void StoryModeRefreshUI()
    {
        obtainedGraces = Managers.DBManager.ParseObtanined(await Managers.DBManager.GetObtainedGraces(Managers.GoogleSignIn.GetUID()));
        _graces.Clear();
        GetText((int)Texts.TitleText).text = I18n.Get(I18nDefine.GRACE_BOX_STORY_MODE_TILE);

        // Delete Prefab Item
        Transform parent = GetObject((int)GameObjects.Content).gameObject.transform;
        foreach (Transform t in parent)
            Managers.Resource.Destroy(t.gameObject);

        if (obtainedGraces == null) return;

        for (int obtainedGracedIdx = 0; obtainedGracedIdx < obtainedGraces.Count - 1; obtainedGracedIdx++)
        {
            for (int j = 0; j < _graceDatas.Count; j++)
            {
                if (obtainedGraces[obtainedGracedIdx] != _graceDatas[j].img) continue;
                if (_graceDatas[j].mode == "OneToOne") continue;

                GameObject graceItem = Managers.UI.MakeSubItem<UI_GraceItem>(GetObject((int)GameObjects.Content).gameObject.transform).gameObject;
                Utils.FindChild(graceItem, "GraceIconText", true).GetOrAddComponent<TextMeshProUGUI>().text = _graceDatas[j].name;
                Utils.FindChild(graceItem, "Grace", true).GetOrAddComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Grace/" + _graceDatas[j].img);
                graceItem.GetComponent<UI_GraceItem>()._name = _graceDatas[j].img;
                graceItem.GetComponent<UI_GraceItem>().BgImage = _graceDatas[j].bgImage;
                string[] fullImageName = _graceDatas[j].img2.Split('\r');
                graceItem.GetComponent<UI_GraceItem>().FullImage = fullImageName[0];
                graceItem.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); });
                graceItem.GetOrAddComponent<UI_GraceItem>()._description = _graceDatas[j].explanation;
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

        GetImage((int)Images.SelectedGraceBGImage).gameObject.SetActive(true);
        GetImage((int)Images.SelectedGraceImage).gameObject.SetActive(true);

        GetImage((int)Images.SelectedGraceBGImage).sprite = Resources.Load<Sprite>("Sprites/MathMtcInFight1vs1/" + selectedObject.GetComponent<UI_GraceItem>().BgImage);
        GetImage((int)Images.SelectedGraceImage).sprite = Resources.Load<Sprite>("Sprites/MathMtcInFight1vs1/" + selectedObject.GetComponent<UI_GraceItem>().FullImage);
        GetText((int)Texts.SelectedGraceText).text = Utils.FindChild(selectedObject, "GraceIconText", true).GetOrAddComponent<TextMeshProUGUI>().text;
        //GetText((int)Texts.SelectedGraceDescription).text = selectedObject.GetOrAddComponent<UI_GraceItem>()._description;
        Managers.TextEffect.ApplyTextEffect(selectedObject.GetOrAddComponent<UI_GraceItem>()._description, GetText((int)Texts.SelectedGraceDescription), 60);
    }

    void OnClickSelectBtn()
    {
        if (selectedObject == null) { OnClosePopup(); return; }

        // 인벤토리 팝업이라면 RefreshUI
        if (Utils.FindChild(gameObject.transform.parent.gameObject, "UI_InventoryPopup") != null)
            Utils.FindChild(gameObject.transform.parent.gameObject, "UI_InventoryPopup").GetComponent<UI_InventoryPopup>().Invoke("RefreshUI", 0);

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

        if (_state == State.OneToOne)
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
