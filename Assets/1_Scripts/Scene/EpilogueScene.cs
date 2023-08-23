using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpilogueScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.EpilogueScene;
        Managers.UI.ShowSceneUI<UI_Epilogue>();
        Debug.Log("Enter EpilogueScene");
        return true;
    }
}
