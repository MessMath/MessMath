using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SelectGracePopup : UI_Popup
{
    UI_GraceBoxPopup _graceBoxPopup;

    enum GameObjects
    {

    }

    enum Texts
    {

    }

    enum Buttons
    {
        ExitBtn,
        SelectedGrace,
        SelectedGrace1,
        SelectedGrace2,
        StartGameBtn,
    }

    enum Images
    {

    }

    List<UI_GraceItem> _blesses = new List<UI_GraceItem>();
    GameObject selectedObject;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(OnClosePopup);
        GetButton((int)Buttons.SelectedGrace).gameObject.BindEvent(() => { _graceBoxPopup = Managers.UI.ShowPopupUI<UI_GraceBoxPopup>(); Managers.Game.SelectGarceInx = 0; });
        GetButton((int)Buttons.SelectedGrace1).gameObject.BindEvent(() => { _graceBoxPopup = Managers.UI.ShowPopupUI<UI_GraceBoxPopup>(); Managers.Game.SelectGarceInx = 1; });
        GetButton((int)Buttons.SelectedGrace2).gameObject.BindEvent(() => { _graceBoxPopup = Managers.UI.ShowPopupUI<UI_GraceBoxPopup>(); Managers.Game.SelectGarceInx = 2; });
        GetButton((int)Buttons.StartGameBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.Scene.ChangeScene(Define.Scene.Fight1vs1GameScene); });

        RefreshUI();

        return true;
    }

    UI_GraceItem _graceItem = null;
    void RefreshUI()
    {
        if (Managers.Game.SelectedGrace != null) GetButton((int)Buttons.SelectedGrace).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Grace/" + Managers.Game.SelectedGrace);
        if (Managers.Game.SelectedGrace1 != null) GetButton((int)Buttons.SelectedGrace1).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Grace/" + Managers.Game.SelectedGrace1);
        if (Managers.Game.SelectedGrace2 != null) GetButton((int)Buttons.SelectedGrace2).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Grace/" + Managers.Game.SelectedGrace2);
    }

    void OnClosePopup()
    {
        // Sound
        // TODO ClosePopupSound

        Managers.UI.ClosePopupUI(this);

    }
}
