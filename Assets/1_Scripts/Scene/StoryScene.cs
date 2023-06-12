using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.StoryScene;
        Managers.UI.ShowSceneUI<UI_Story>();
        Debug.Log("Enter Story");
        return true;
    }
}