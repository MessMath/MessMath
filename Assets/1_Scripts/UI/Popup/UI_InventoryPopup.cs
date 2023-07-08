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
        CostumeBtn,
        CollectionBtn,
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
        GetButton((int)Buttons.CostumeBtn).gameObject.BindEvent(CostumeBtn);
        GetButton((int)Buttons.CollectionBtn).gameObject.BindEvent(CollectionBtn);

        RefreshUI();

        return true;
    }

    public void RefreshUI()
    {
        
    }

    void CostumeBtn()
    {
        Managers.Sound.Play("ClickBtnEff");
        Debug.Log("On Click CostumeBtn");
    }

    void CollectionBtn()
    {
        Managers.Sound.Play("ClickBtnEff");
        Debug.Log("On Click On Click CollectionBtn");
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
