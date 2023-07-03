using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagnosisScene :  BaseScene
{
     protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.DiagnosisScene;
        Managers.UI.ShowSceneUI<UI_Diagnosis>();
        Debug.Log("Enter DiagnosisScene");
        return true;
    }
}
