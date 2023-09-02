using Firebase.Database;
using MessMathI18n;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.XR;
using static Define;
using static UserManager;

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

        //��� ����ǰ ������ ������ �������� ��� �ٲ��ִ� �Լ�
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
        if (PlayerPrefs.HasKey("LogInOut") && PlayerPrefs.GetInt("LogInOut") == 100)
        {
            if (Managers.GoogleSignIn.GetUID() != null)
            {
                GetObject((int)GameObjects.Panel).SetActive(false);
                GetImage((int)Images.SignIn).gameObject.SetActive(false);
                GetImage((int)Images.SignInPressed).gameObject.SetActive(false);
                GetText((int)Texts.Start).gameObject.SetActive(true);
            }
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
        CreateUser();
        if (PlayerPrefs.GetInt("SelectLanguage") == 98)
        {
            Managers.Scene.ChangeScene(Define.Scene.MakeTxtFileScene);
            return;
        }
        Managers.UI.ShowPopupUI<UI_SelectLanguage>();
        //if (LocalizationManager.Get().GetSelectedLanguage() == Language.ENGLISH || LocalizationManager.Get().GetSelectedLanguage() == Language.KOREAN)
        //    Managers.Scene.ChangeScene(Define.Scene.MakeTxtFileScene);
        //else
        //    Managers.UI.ShowPopupUI<UI_SelectLanguage>();

    }
    
    async void CreateUser()
    {
        await Managers.DBManager.CheckUserId(Managers.GoogleSignIn.GetUID());
        Debug.Log("///////////////////////////"+ Managers.Game.IsExisted);
        if (Managers.Game.IsExisted == false) { Managers.DBManager.CreateNewUser(""); }

    }

    void onClickedSignIn()
    {
        GetImage((int)Images.SignIn).gameObject.SetActive(false);
        GetImage((int)Images.SignInPressed).gameObject.SetActive(true);

        Managers.Sound.Play("ClickBtnEff");

        Managers.GoogleSignIn.SignInWithGoogle();
        Debug.Log("�α���");
    }

    bool CheckHaveBgImage()
    {
        if (Managers.UserMng.user.UID == null) return false;
        if (Managers.UserMng.GetObtainedCollections() == null) return false;

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
