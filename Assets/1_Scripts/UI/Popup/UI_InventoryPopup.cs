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

        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ClosePopupUI(this); });
        GetButton((int)Buttons.SelectOneToOneModeGraceBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); _selectGracePopup = Managers.UI.ShowPopupUI<UI_SelectGracePopup>(); _selectGracePopup._state = UI_SelectGracePopup.State.OneToOne; });
        GetButton((int)Buttons.SelectStoryModeGraceBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); _selectGracePopup = Managers.UI.ShowPopupUI<UI_SelectGracePopup>(); _selectGracePopup._state = UI_SelectGracePopup.State.Story; });
        GetButton((int)Buttons.ReplayStoryBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.Scene.ChangeScene(Define.Scene.StoryScene); });

        RefreshUI();

        return true;
    }

    public void RefreshUI()
    {
        // TODO?? 가호 프리셋이 있으면 이미지도 변경되나??
    }
}
