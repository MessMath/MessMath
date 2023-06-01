using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.LobbyScene;
        Managers.UI.ShowSceneUI<UI_Lobby>();
        Debug.Log("Enter Lobby");
        return true;
    }
}
