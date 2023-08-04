using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignInScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.SignInScene;
        Managers.UI.ShowSceneUI<UI_SignIn>();
        Debug.Log("Enter Lobby");
        return true;
    }
}
