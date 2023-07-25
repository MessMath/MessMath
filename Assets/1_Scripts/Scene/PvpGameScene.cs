using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PvpGameScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.PvpGameScene;
        Managers.UI.ShowSceneUI<UI_PvpGameScene>();
        Debug.Log("Enter PvpGameScene");
        return true;
    }
}
