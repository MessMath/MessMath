using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeGameScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.PracticeGameScene;
        Managers.UI.ShowSceneUI<UI_PracticeGame>();
        Debug.Log("Init");
        return true;
    }
}

