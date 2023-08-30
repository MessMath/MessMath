using MessMathI18n;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SelectMathMtcfor1vs1 : UI_Popup
{
    enum Texts
    {
        ChooseBossText,
        MathMtcName1,
        QuestionNum1,
        MathMtcName2,
        QuestionNum2,
        MathMtcName3,
        QuestionNum3,
    }

    enum Buttons
    {
        Gauss,
        Pythagoras,
        Newton
    }

    enum Images
    {
        Panel,
    }

    enum GameObjects
    {

    }

    private void Start()
    {
        Init();
    }

    UI_SelectGracePopup _selectGracepopup;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));
        BindImage(typeof(Images));

        GetImage((int)Images.Panel).gameObject.BindEvent(OnClickBG);

        GetText((int)Texts.ChooseBossText).text = I18n.Get(I18nDefine.CHOOSE_BOSS_TEXT);

        GetText((int)Texts.MathMtcName1).text = I18n.Get(I18nDefine.MathMtcName1);
        GetText((int)Texts.QuestionNum1).text = I18n.Get(I18nDefine.QuestionNum1);
        GetText((int)Texts.MathMtcName2).text = I18n.Get(I18nDefine.MathMtcName2);
        GetText((int)Texts.QuestionNum2).text = I18n.Get(I18nDefine.QuestionNum2);
        GetText((int)Texts.MathMtcName3).text = I18n.Get(I18nDefine.MathMtcName3);
        GetText((int)Texts.QuestionNum3).text = I18n.Get(I18nDefine.QuestionNum3);

        // TODO
        // 여기서 정한 수학자가 인게임에서 적으로 등장해야 한다.
        // 버튼을 누르면 다음 팝업(가호 선택 팝업)이 뜨도록.
        GetButton((int)Buttons.Gauss).gameObject.BindEvent(ChooseGause);
        GetButton((int)Buttons.Pythagoras).gameObject.BindEvent(ChoosePythagoras);
        GetButton((int)Buttons.Newton).gameObject.BindEvent(ChooseNewton);

        return true;
    }

    void OnClickBG()
    {
        // Sound
        Managers.Sound.Play("ClickBtnEff");

        Managers.UI.ClosePopupUI();
    }

    void ChooseGause()
    {
        PlayerPrefs.SetString("Boss", "Gauss");
        PlayerPrefs.SetInt("QstLimit", 8);
        Managers.Sound.Play("ClickBtnEff");
        _selectGracepopup = Managers.UI.ShowPopupUI<UI_SelectGracePopup>();
        _selectGracepopup._state = UI_SelectGracePopup.State.OneToOne;
        //Managers.Scene.ChangeScene(Define.Scene.Fight1vs1GameScene);
    }

    void ChoosePythagoras()
    {
        PlayerPrefs.SetString("Boss", "Pythagoras");
        PlayerPrefs.SetInt("QstLimit", 16);
        Managers.Sound.Play("ClickBtnEff");
        _selectGracepopup = Managers.UI.ShowPopupUI<UI_SelectGracePopup>();
        _selectGracepopup._state = UI_SelectGracePopup.State.OneToOne;
        //Managers.Scene.ChangeScene(Define.Scene.Fight1vs1GameScene);
    }
    void ChooseNewton()
    {
        PlayerPrefs.SetString("Boss", "Newton");
        PlayerPrefs.SetInt("QstLimit", 24);
        Managers.Sound.Play("ClickBtnEff");
        _selectGracepopup = Managers.UI.ShowPopupUI<UI_SelectGracePopup>();
        _selectGracepopup._state = UI_SelectGracePopup.State.OneToOne;
        //Managers.Scene.ChangeScene(Define.Scene.Fight1vs1GameScene);
    }
}