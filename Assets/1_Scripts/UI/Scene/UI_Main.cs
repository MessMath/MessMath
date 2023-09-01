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
        Start,
    }

    enum Buttons
    {

    }

    enum Images
    {
        BG,
        SignIn,
        SignInPressed,
    }

    enum GameObjects
    {
        Panel,
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
        GetImage((int)Images.SignIn).gameObject.BindEvent(onClickedSignIn);
        GetImage((int)Images.SignInPressed).gameObject.SetActive(false);
        GetText((int)Texts.Start).text = I18n.Get(I18nDefine.MAIN_START);
        GetText((int)Texts.Start).gameObject.SetActive(false);

        // Sound
        Managers.Sound.Clear();
        Managers.Sound.Play("MainBgm", Define.Sound.Bgm);
        return true;
    }

    private void Update()
    {
        if (Managers.GoogleSignIn.IsLogined == true)
        {
            GetObject((int)GameObjects.Panel).SetActive(false);
            GetImage((int)Images.SignIn).gameObject.SetActive(false);
            GetImage((int)Images.SignInPressed).gameObject.SetActive(false);
            GetText((int)Texts.Start).gameObject.SetActive(true);
        }
        else
        {
            GetObject((int)GameObjects.Panel).SetActive(true);
            GetImage((int)Images.SignIn).gameObject.SetActive(true);
            //GetImage((int)Images.SignInPressed).gameObject.SetActive(false);
            GetText((int)Texts.Start).gameObject.SetActive(false);
        }
    }

    void OnClickBG( )
    {
        // Sound
        Managers.Sound.Play("ClickBtnEff");
        Managers.GoogleSignIn.SignInWithGoogle();
        Managers.UI.ShowPopupUI<UI_SelectLanguage>();

        //if (LocalizationManager.Get().GetSelectedLanguage() == Language.ENGLISH || LocalizationManager.Get().GetSelectedLanguage() == Language.KOREAN)
        //    Managers.Scene.ChangeScene(Define.Scene.MakeTxtFileScene);
        //else
        //    Managers.UI.ShowPopupUI<UI_SelectLanguage>();

    }
    
    void onClickedSignIn()
    {
        GetImage((int)Images.SignIn).gameObject.SetActive(false);
        GetImage((int)Images.SignInPressed).gameObject.SetActive(true);

        Managers.Sound.Play("ClickBtnEff");

        Managers.GoogleSignIn.SignInWithGoogle();
        Debug.Log("·Î±×ÀÎ");
    }

}
