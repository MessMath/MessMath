using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryPopup : UI_Popup
{
    UI_GraceBoxPopup _graceBoxPopup;
    public int _graceIdx = 0;

    enum GameObjects
    {

    }

    enum Texts
    {

    }

    enum Images
    {

    }

    enum Buttons
    {
        ExitBtn,
        SelectedGrace,
        SelectedGrace1,
        SelectedGrace2,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ClosePopupUI(this); });
        GetButton((int)Buttons.SelectedGrace).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ShowPopupUI<UI_GraceBoxPopup>(); Managers.Game.SelectGraceInx = 0; });
        GetButton((int)Buttons.SelectedGrace1).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ShowPopupUI<UI_GraceBoxPopup>(); Managers.Game.SelectGraceInx = 1; });
        GetButton((int)Buttons.SelectedGrace2).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ShowPopupUI<UI_GraceBoxPopup>(); Managers.Game.SelectGraceInx = 2; });

        RefreshUI();

        return true;
    }

    public void RefreshUI()
    {
        if (PlayerPrefs.HasKey("SelectedGrace")) GetButton((int)Buttons.SelectedGrace).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Grace/" + PlayerPrefs.GetString("SelectedGrace"));
        if (PlayerPrefs.HasKey("SelectedGrace1")) GetButton((int)Buttons.SelectedGrace1).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Grace/" + PlayerPrefs.GetString("SelectedGrace1"));
        if (PlayerPrefs.HasKey("SelectedGrace2")) GetButton((int)Buttons.SelectedGrace2).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Grace/" + PlayerPrefs.GetString("SelectedGrace2"));
    }
}
