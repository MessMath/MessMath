using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Windows;
using System.Data;
using System;
using TMPro;
using System.IO;
using Photon.Pun;
using Unity.VisualScripting;

public class UI_PvpMatchingScene : UI_Scene
{
    enum Texts
    {
        
    }

    enum Buttons
    {
        
    }

    enum Images
    {
        
    }

    enum GameObjects
    {

    }

    private void Awake()
    {
        Init();
    }

    private void Start()
    {

        // 서버에 연결!
        Managers.Network.Connect();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));
        BindImage(typeof(Images));

        

        return true;
    }

}
