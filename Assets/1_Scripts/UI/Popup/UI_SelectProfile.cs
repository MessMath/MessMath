using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessMathI18n;

public class UI_SelectProfile : UI_Popup
{
    enum Buttons
    {
        ExitBtn,
    }

    enum Images
    {
        GraceOfGauss,
        GraceOfNeumann,
        GraceOfEinstein,
        GraceOfNewton,
        GraceOfPythagoras,
        EmptyGrace1,
        EmptyGrace2,
        EmptyGrace3,
        EmptyGrace4,
    }

    enum Texts
    {
        TitleText,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(() => OnClosePopup());
        GetImage((int)Images.GraceOfGauss).gameObject.BindEvent(() => OnClickedProfile("GraceOfGauss"));
        GetImage((int)Images.GraceOfNeumann).gameObject.BindEvent(() => OnClickedProfile("GraceOfNeumann"));
        GetImage((int)Images.GraceOfEinstein).gameObject.BindEvent(() => OnClickedProfile("GraceOfEinstein"));
        GetImage((int)Images.GraceOfNewton).gameObject.BindEvent(() => OnClickedProfile("GraceOfNewton"));
        GetImage((int)Images.GraceOfPythagoras).gameObject.BindEvent(() => OnClickedProfile("GraceOfPythagoras"));
        GetImage((int)Images.EmptyGrace1).gameObject.BindEvent(() => Managers.Sound.Play("ClickBtnEff"));
        GetImage((int)Images.EmptyGrace2).gameObject.BindEvent(() => Managers.Sound.Play("ClickBtnEff"));
        GetImage((int)Images.EmptyGrace3).gameObject.BindEvent(() => Managers.Sound.Play("ClickBtnEff"));
        GetImage((int)Images.EmptyGrace4).gameObject.BindEvent(() => Managers.Sound.Play("ClickBtnEff"));
        GetText((int)Texts.TitleText).text = I18n.Get(I18nDefine.INFO_PROFILE);

        return true;
    }

    void OnClickedProfile(string profile)
    {
        Managers.Sound.Play("ClickBtnEff");

        string path = $"Sprites/Grace/{profile}";
        PlayerPrefs.SetString("Profile", path);
        Managers.UI.ClosePopupUI(this);
    }

    void OnClosePopup()
    {
        // Sound
        // TODO ClosePopupSound
        Managers.Sound.Play("ClickBtnEff");

        Managers.UI.ClosePopupUI(this);

    }

}