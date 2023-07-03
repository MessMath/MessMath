using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SelectGracePopup : UI_Popup
{
    enum Buttons
    {
        ExitBtn,
        SelectedGrace,
        SelectedGrace1,
        SelectedGrace2,
        StartGameBtn,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(OnClosePopup);
        GetButton((int)Buttons.SelectedGrace).gameObject.BindEvent(() => { Managers.UI.ShowPopupUI<UI_GraceBoxPopup>(); Managers.Game.SelectGarceInx = 0; });
        GetButton((int)Buttons.SelectedGrace1).gameObject.BindEvent(() => { Managers.UI.ShowPopupUI<UI_GraceBoxPopup>(); Managers.Game.SelectGarceInx = 1; });
        GetButton((int)Buttons.SelectedGrace2).gameObject.BindEvent(() => { Managers.UI.ShowPopupUI<UI_GraceBoxPopup>(); Managers.Game.SelectGarceInx = 2; });
        GetButton((int)Buttons.StartGameBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.Scene.ChangeScene(Define.Scene.Fight1vs1GameScene); });

        RefreshUI();

        return true;
    }

    public void RefreshUI()
    {
        if (PlayerPrefs.HasKey("SelectedGrace")) GetButton((int)Buttons.SelectedGrace).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Grace/" + PlayerPrefs.GetString("SelectedGrace"));
        if (PlayerPrefs.HasKey("SelectedGrace1")) GetButton((int)Buttons.SelectedGrace1).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Grace/" + PlayerPrefs.GetString("SelectedGrace1"));
        if (PlayerPrefs.HasKey("SelectedGrace2")) GetButton((int)Buttons.SelectedGrace2).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Grace/" + PlayerPrefs.GetString("SelectedGrace2"));
    }

    void OnClosePopup()
    {
        // Sound
        // TODO ClosePopupSound
        Managers.Sound.Play("ClickBtnEff");

        Managers.UI.ClosePopupUI(this);

    }
}
