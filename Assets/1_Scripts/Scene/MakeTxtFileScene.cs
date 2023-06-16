using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTxtFileScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.MakeTxtFileScene;
        Managers.UI.ShowSceneUI<UI_MakeTxtFile>();
        Debug.Log("Enter MakeTxtFileScene");
        return true;
    }
}
