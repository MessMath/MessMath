using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MakeTxtFile : UI_Scene
{
    enum Texts
    {
        LogTMP,
        TipText,
    }
    enum GameObjects
    {
        MagicCircle,
    }

    private void Start()
    {
        Init();
    }

    void Update()
    {
        GetObject((int)GameObjects.MagicCircle).transform.Rotate(new Vector3(0f, 0f, -30f) * Time.deltaTime);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        
        BindText(typeof(Texts));
        BindObject(typeof(GameObjects));

        SelectTip();
        GetText((int)Texts.LogTMP).text = "업데이트 정보를 불러오는 중입니다...";

        return true;
    }

    void SelectTip()
    {
        int tipTextCount = Random.Range(0, 21);
        GetText((int)Texts.TipText).text = Managers.GetText(Define.TipText + tipTextCount);
    }
}
