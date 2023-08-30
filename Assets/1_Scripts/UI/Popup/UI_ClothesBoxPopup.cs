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
    List<StoreData> _graceDatas = new List<StoreData>();
    List<UI_GraceItem> _graces = new List<UI_GraceItem>();
    GameObject selectedObject;

    // 의상 수집품을 하나의 프리펩으로 사용할것인가?
    // 프리펩이 다르다면 코드역시 달라질 것인가?
    // 레이아웃은 어떻게 되며 구현해야하는 부분은 무엇인가?

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

    void OneToOneModeRefreshUI()
    {
        _graces.Clear();
        GetText((int)Texts.TitleText).text = I18n.Get(I18nDefine.GRACE_BOX_ONE_TO_ONE_MODE_TILE);
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
                graceItem.GetComponent<UI_GraceItem>().BgImage = _graceDatas[i].bgImage;
                string[] fullImageName = _graceDatas[i].img2.Split('\r');
                graceItem.GetComponent<UI_GraceItem>().FullImage = fullImageName[0];
                graceItem.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); });
                graceItem.GetOrAddComponent<UI_GraceItem>()._description = _graceDatas[i].explanation;
                graceItem.GetComponentInChildren<Image>().gameObject.BindEvent(() => { selectedObject = graceItem; OnClickGraceBtn(); });
            }
        }
    }

    void StoryModeRefreshUI()
    {
        _graces.Clear();
        GetText((int)Texts.TitleText).text = I18n.Get(I18nDefine.GRACE_BOX_STORY_MODE_TILE);

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
                graceItem.GetComponent<UI_GraceItem>().BgImage = _graceDatas[i].bgImage;
                string[] fullImageName = _graceDatas[i].img2.Split('\r');
                graceItem.GetComponent<UI_GraceItem>().FullImage = fullImageName[0];
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

        GetImage((int)Images.SelectedGraceBGImage).gameObject.SetActive(true);
        GetImage((int)Images.SelectedGraceImage).gameObject.SetActive(true);

        GetImage((int)Images.SelectedGraceBGImage).sprite = Resources.Load<Sprite>("Sprites/MathMtcInFight1vs1/" + selectedObject.GetComponent<UI_GraceItem>().BgImage);
        #region 폰노이만 크기 예외
        // 아니 폰노이만 크기가 이상해서 이거만 예외처리 해야 돼 => 말 안됨.
        // 이렇게 하드코딩하는거 잘못된거 아는데 어떻게 할 지 모르겠으니까 하드코딩해야징
        if (selectedObject.GetComponent<UI_GraceItem>().FullImage == "NeumannImage")
        {
            GetImage((int)Images.SelectedGraceImage).transform.localPosition = new Vector3(-1708.3f, 426.69f, 0);
            Debug.Log(GetImage((int)Images.SelectedGraceImage).transform.localPosition);
            GetImage((int)Images.SelectedGraceImage).rectTransform.sizeDelta = new Vector2(509.8f, 944.08f);
        }
        else if (selectedObject.GetComponent<UI_GraceItem>().FullImage != "NeumannImage")
        {
            GetImage((int)Images.SelectedGraceImage).transform.localPosition = new Vector3(-1964.95f, 426.69f, 0);
            Debug.Log(GetImage((int)Images.SelectedGraceImage).transform.localPosition);
            GetImage((int)Images.SelectedGraceImage).rectTransform.sizeDelta = new Vector2(956.02f, 818.46f);
        }
        #endregion
        GetImage((int)Images.SelectedGraceImage).sprite = Resources.Load<Sprite>("Sprites/MathMtcInFight1vs1/" + selectedObject.GetComponent<UI_GraceItem>().FullImage);
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
