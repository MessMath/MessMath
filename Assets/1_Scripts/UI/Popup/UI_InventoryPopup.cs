using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryPopup : UI_Popup
{
    UI_SelectGracePopup _selectGracePopup;

    enum Buttons
    {
        SelectOneToOneModeGraceBtn,
        SelectStoryModeGraceBtn,
        ReplayStoryBtn,
        ExitBtn,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(ExitBtn);
        GetButton((int)Buttons.SelectOneToOneModeGraceBtn).gameObject.BindEvent(SelectOneToOneModeGraceBtn);
        GetButton((int)Buttons.SelectStoryModeGraceBtn).gameObject.BindEvent(SelectStoryModeGraceBtn);
        GetButton((int)Buttons.ReplayStoryBtn).gameObject.BindEvent(ReplayStoryBtn);

        RefreshUI();

        return true;
    }

    public void RefreshUI()
    {
        // TODO?? 가호 프리셋이 있으면 이미지도 변경되나??
    }

    public void ExitBtn()
    {
        Managers.Sound.Play("ClickBtnEff"); 
        Managers.UI.ClosePopupUI(this);
    }

    public void SelectOneToOneModeGraceBtn()
    {
        Managers.Sound.Play("ClickBtnEff"); 
        _selectGracePopup = Managers.UI.ShowPopupUI<UI_SelectGracePopup>(); 
        _selectGracePopup._state = UI_SelectGracePopup.State.OneToOne;
    }

    public void SelectStoryModeGraceBtn()
    {
        Managers.Sound.Play("ClickBtnEff"); 
        _selectGracePopup = Managers.UI.ShowPopupUI<UI_SelectGracePopup>(); 
        _selectGracePopup._state = UI_SelectGracePopup.State.Story;
    }

    public void ReplayStoryBtn()
    {
        Managers.Sound.Play("ClickBtnEff"); 
        Managers.Scene.ChangeScene(Define.Scene.StoryScene);
    }
}
