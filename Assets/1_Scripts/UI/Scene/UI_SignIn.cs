using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SignIn : UI_Scene
{

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

        return true;
    }
}
