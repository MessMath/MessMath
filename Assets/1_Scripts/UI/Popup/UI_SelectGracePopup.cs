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

        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(OnClosePopup);

        if (_state == State.OneToOne) 
        {
            for (int i = 0; i < 3; i++)
            {
                GetButton(i).gameObject.BindEvent(() => { _graceBoxPopup = Managers.UI.ShowPopupUI<UI_GraceBoxPopup>(); _graceBoxPopup._state = UI_GraceBoxPopup.State.OneToOne; Managers.Game.SelectGraceInx = i; });
            }
            OneToOneModeRefreshUI();
            GetButton((int)Buttons.StartGameBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.Scene.ChangeScene(Define.Scene.Fight1vs1GameScene); });
        }
        else if (_state == State.Story) 
        {
            for (int i = 0; i < 3; i++)
            {
                GetButton(i).gameObject.BindEvent(() => { _graceBoxPopup = Managers.UI.ShowPopupUI<UI_GraceBoxPopup>(); _graceBoxPopup._state = UI_GraceBoxPopup.State.Story; Managers.Game.SelectGraceInx = i; });
            }
            StoryModeRefreshUI();
            GetButton((int)Buttons.StartGameBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.Scene.ChangeScene(Define.Scene.StoryGameScene); });
        }


        return true;
    }

    public void OneToOneModeRefreshUI()
    {
        for (int i = 0; i < 3; i++)
        {
            if (PlayerPrefs.HasKey($"SelectedGrace{i}InOneToOne")) GetButton(i).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Grace/" + PlayerPrefs.GetString($"SelectedGrace{i}InOneToOne"));
        }
    }

    public void StoryModeRefreshUI()
    {
        for (int i = 0; i < 3; i++)
        {
            if (PlayerPrefs.HasKey($"SelectedGrace{i}InStory")) GetButton(i).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Grace/" + PlayerPrefs.GetString($"SelectedGrace{i}InStory"));
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
