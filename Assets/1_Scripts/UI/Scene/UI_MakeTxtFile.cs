using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MakeTxtFile : UI_Scene
{
    enum Texts
    {
        LogTMP,
    }

    private void Start()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        
        BindText(typeof(Texts));

        GetText((int)Texts.LogTMP).text = "업데이트 정보를 불러오는 중입니다...";

        return true;
    }
}
