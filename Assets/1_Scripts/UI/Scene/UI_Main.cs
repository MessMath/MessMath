using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.XR;
using static Define;

public class UI_Main : UI_Scene
{
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

        GetImage((int)Images.BG).gameObject.BindEvent(OnClickBG);

        return true;
    }

    void OnClickBG( )
    {
        Managers.Scene.ChangeScene(Define.Scene.MakeTxtFileScene);
    }
}
