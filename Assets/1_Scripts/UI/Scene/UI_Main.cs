using MessMathI18n;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.XR;
using static Define;

public class UI_Main : UI_Scene
{
    string[] obtainedBg;

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

        // 배경 소지품 가지고 있으면 랜덤으로 배경 바꿔주는 함수
        //if (CheckHaveBgImage())
        //{
        //    GetImage((int)Images.BG).sprite = Resources.Load<Sprite>("Sprites/background" + GetRandomBgSprite());
        //}

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
        Debug.Log("로그인");
    }

    bool CheckHaveBgImage()
    {
        if (Managers.UserMng.user.UID == null) return false;

        for (int i = 0; i < Managers.UserMng.GetObtainedCollections().Count; i++)
        {
            if (Managers.UserMng.GetObtainedCollections()[i] == "landscape") return true;
            if (Managers.UserMng.GetObtainedCollections()[i] == "night_landscape") return true;
            if (Managers.UserMng.GetObtainedCollections()[i] == "space_landscape") return true;
        }

        return false;
    }

    string GetRandomBgSprite()
    {
        if (!CheckHaveBgImage()) return "";

        for (int i = 0; i < Managers.UserMng.GetObtainedCollections().Count; i++)
        {
            if (Managers.UserMng.GetObtainedCollections()[i] == "landscape") obtainedBg.Append<string>(Managers.UserMng.GetObtainedCollections()[i]);
            if (Managers.UserMng.GetObtainedCollections()[i] == "night_landscape") obtainedBg.Append<string>(Managers.UserMng.GetObtainedCollections()[i]);
            if (Managers.UserMng.GetObtainedCollections()[i] == "space_landscape") obtainedBg.Append<string>(Managers.UserMng.GetObtainedCollections()[i]);
        }

        return obtainedBg[UnityEngine.Random.Range(0, obtainedBg.Count())];
    }

}
