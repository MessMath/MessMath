using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryGameScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.StoryGameScene;
        Managers.UI.ShowSceneUI<UI_StoryGame>();
        Debug.Log("Enter StoryGame");
        return true;
    }
}
