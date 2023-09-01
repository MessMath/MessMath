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

    }

    enum Buttons
    {

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

        // 배경 소지품 가지고 있으면 랜덤으로 배경 바꿔주는 함수
        //if (CheckHaveBgImage())
        //{
        //    GetImage((int)Images.BG).sprite = Resources.Load<Sprite>("Sprites/background" + GetRandomBgSprite());
        //}

        GetImage((int)Images.BG).gameObject.BindEvent(OnClickBG);

        // Sound
        Managers.Sound.Clear();
        Managers.Sound.Play("MainBgm", Define.Sound.Bgm);

        return true;
    }

    void OnClickBG()
    {
        // Sound
        Managers.Sound.Play("ClickBtnEff");

        Managers.UI.ShowPopupUI<UI_SelectLanguage>();

        //if (LocalizationManager.Get().GetSelectedLanguage() == Language.ENGLISH || LocalizationManager.Get().GetSelectedLanguage() == Language.KOREAN)
        //    Managers.Scene.ChangeScene(Define.Scene.MakeTxtFileScene);
        //else
        //    Managers.UI.ShowPopupUI<UI_SelectLanguage>();

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
