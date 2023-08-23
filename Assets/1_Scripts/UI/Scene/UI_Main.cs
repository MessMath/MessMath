using MessMathI18n;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.XR;
using static Define;

public class UI_Main : UI_Scene
{
    enum Texts
    {

    }

    enum Buttons
    {
        KoreanBtn,
        EnglishBtn
    }

    enum Images
    {
        BG,
    }

    enum GameObjects
    {

    }

    private void Start()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));
        BindImage(typeof(Images));

        GetImage((int)Images.BG).gameObject.BindEvent(OnClickBG);
        GetButton((int)Buttons.KoreanBtn).gameObject.BindEvent(OnClickedKorBtn);
        GetButton((int)Buttons.EnglishBtn).gameObject.BindEvent(OnClickedEnBtn);

        // Sound
        Managers.Sound.Clear();
        Managers.Sound.Play("MainBgm", Define.Sound.Bgm);

        return true;
    }

    void OnClickBG( )
    {
        // Sound
        Managers.Sound.Play("ClickBtnEff");

        //Managers.UI.ShowPopupUI<UI_Diagnosis>();
        Managers.Scene.ChangeScene(Define.Scene.MakeTxtFileScene);
    }

    void OnClickedKorBtn()
    {
        //GetButton((int)Buttons.KoreanBtn).gameObject.GetComponent<Image>().color = Color.white;
        //GetButton((int)Buttons.EnglishBtn).gameObject.GetComponent<Image>().color = Color.grey;
        LocalizationManager.Get().SetLanguage(Language.KOREAN);
        Debug.Log("한글 버전 선택");
    }

    void OnClickedEnBtn()
    {
        //GetButton((int)Buttons.KoreanBtn).gameObject.GetComponent<Image>().color = Color.grey;
        //GetButton((int)Buttons.EnglishBtn).gameObject.GetComponent<Image>().color = Color.white;
        LocalizationManager.Get().SetLanguage(Language.ENGLISH);
        Debug.Log("영어 버전 선택");
    }
}
