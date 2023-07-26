using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PvpMatchingScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.PvpMatchingScene;
        Managers.UI.ShowSceneUI<UI_PvpMatchingScene>();
        Debug.Log("Enter PvpMatchingScene");
        return true;
    }
}
