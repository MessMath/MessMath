using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SelectGracePopup : UI_Popup
{
    enum Buttons
    {
        SelectedGrace0,
        SelectedGrace1,
        SelectedGrace2,
        Cancel0Btn,
        Cancel1Btn,
        Cancel2Btn,
        ExitBtn,
        StartGameBtn,
    }

    public enum State
    {
        None,
        OneToOne,
        Story,
    }

    UI_GraceBoxPopup _graceBoxPopup;
    public State _state = State.None;

    public override bool Init()

    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        SettingCancelBtn();

        if (Utils.FindChild(gameObject.transform.parent.gameObject, "UI_InventoryPopup") != null) // 인벤토리가 열려있을 때
            GetButton((int)Buttons.StartGameBtn).gameObject.SetActive(false);

        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(OnClosePopup);
        GetButton((int)Buttons.Cancel0Btn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); OnClickCancelBtn(0); });
        GetButton((int)Buttons.Cancel1Btn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); OnClickCancelBtn(1); });
        GetButton((int)Buttons.Cancel2Btn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); OnClickCancelBtn(2); });

        Debug.Log($"UI_SelectedGracePopup's state is {_state}");
        if (_state == State.OneToOne)
        {
            GetButton((int)Buttons.SelectedGrace0).gameObject.BindEvent(() => { _graceBoxPopup = Managers.UI.ShowPopupUI<UI_GraceBoxPopup>(); _graceBoxPopup._state = UI_GraceBoxPopup.State.OneToOne; Managers.Game.SelectGraceInx = 0; });
            GetButton((int)Buttons.SelectedGrace1).gameObject.BindEvent(() => { _graceBoxPopup = Managers.UI.ShowPopupUI<UI_GraceBoxPopup>(); _graceBoxPopup._state = UI_GraceBoxPopup.State.OneToOne; Managers.Game.SelectGraceInx = 1; });
            GetButton((int)Buttons.SelectedGrace2).gameObject.BindEvent(() => { _graceBoxPopup = Managers.UI.ShowPopupUI<UI_GraceBoxPopup>(); _graceBoxPopup._state = UI_GraceBoxPopup.State.OneToOne; Managers.Game.SelectGraceInx = 2; });
            OneToOneModeRefreshUI();
            GetButton((int)Buttons.StartGameBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.Scene.ChangeScene(Define.Scene.Fight1vs1GameScene); });
        }
        else if (_state == State.Story)
        {
            GetButton((int)Buttons.SelectedGrace0).gameObject.BindEvent(() => { _graceBoxPopup = Managers.UI.ShowPopupUI<UI_GraceBoxPopup>(); _graceBoxPopup._state = UI_GraceBoxPopup.State.Story; Managers.Game.SelectGraceInx = 0; });
            GetButton((int)Buttons.SelectedGrace1).gameObject.BindEvent(() => { _graceBoxPopup = Managers.UI.ShowPopupUI<UI_GraceBoxPopup>(); _graceBoxPopup._state = UI_GraceBoxPopup.State.Story; Managers.Game.SelectGraceInx = 1; });
            GetButton((int)Buttons.SelectedGrace2).gameObject.BindEvent(() => { _graceBoxPopup = Managers.UI.ShowPopupUI<UI_GraceBoxPopup>(); _graceBoxPopup._state = UI_GraceBoxPopup.State.Story; Managers.Game.SelectGraceInx = 2; });
            StoryModeRefreshUI();
            GetButton((int)Buttons.StartGameBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.Scene.ChangeScene(Define.Scene.StoryGameScene); });
        }

        return true;
    }

    void SettingCancelBtn()
    {
        if (_state == State.OneToOne)
        {
            for (int i = 0; i < 3; i++)
            {
                if (PlayerPrefs.GetString($"SelectedGrace{i}InOneToOne") != "")
                    GetButton(i + 3).gameObject.SetActive(true);
                else
                    GetButton(i + 3).gameObject.SetActive(false);
            }
        }
        else if (_state == State.Story)
        {
            for (int i = 0; i < 3; i++)
            {
                if (PlayerPrefs.GetString($"SelectedGrace{i}InStory") != "")
                    GetButton(i + 3).gameObject.SetActive(true);
                else
                    GetButton(i + 3).gameObject.SetActive(false);
            }
        }
    }

    void OnClickCancelBtn(int idx)
    {
        if (_state == State.OneToOne)
        {
            for (int i = 0; i < 3; i++)
            {
                if (idx == i)
                {
                    GetButton(i).gameObject.GetComponent<Image>().sprite = null;
                    PlayerPrefs.SetString($"SelectedGrace{i}InOneToOne", "");
                    SettingCancelBtn();
                    OneToOneModeRefreshUI();
                }
            }
        }
        else if (_state == State.Story)
        {
            for (int i = 0; i < 3; i++)
            {
                if (idx == i)
                {
                    GetButton(i).gameObject.GetComponent<Image>().sprite = null;
                    PlayerPrefs.SetString($"SelectedGrace{i}InStory", "");
                    SettingCancelBtn();
                    StoryModeRefreshUI();
                }
            }
        }
    }

    public void OneToOneModeRefreshUI()
    {
        SettingCancelBtn();
        for (int i = 0; i < 3; i++)
        {
            if (PlayerPrefs.HasKey($"SelectedGrace{i}InOneToOne"))
                GetButton(i).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Grace/" + PlayerPrefs.GetString($"SelectedGrace{i}InOneToOne"));
        }
    }

    public void StoryModeRefreshUI()
    {
        SettingCancelBtn();
        for (int i = 0; i < 3; i++)
        {
            if (PlayerPrefs.HasKey($"SelectedGrace{i}InStory"))
                GetButton(i).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Grace/" + PlayerPrefs.GetString($"SelectedGrace{i}InStory"));
        }
    }

    void OnClosePopup()
    {
        // Sound
        // TODO ClosePopupSound
        Managers.Sound.Play("ClickBtnEff");

        Managers.UI.ClosePopupUI(this);

    }
}
