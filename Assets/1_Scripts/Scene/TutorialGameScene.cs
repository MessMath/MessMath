using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGameScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.TutorialGameScene;
        Managers.UI.ShowSceneUI<UI_TutorialGame>();
        Debug.Log("Enter Tutorial");
        return true;
    }
}
