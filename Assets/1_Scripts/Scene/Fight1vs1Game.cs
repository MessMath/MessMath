using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;

public class Fight1vs1Game : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.Fight1vs1GameScene;
        Managers.UI.ShowSceneUI<UI_Fight1vs1Game>();
        Debug.Log("Enter Fight1vs1Game");
        return true;
    }
}
