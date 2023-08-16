using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ChangeScenePopup : UI_Popup
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
        GetText((int)Texts.LogTMP).text = "화면 전환 중...";

        return true;
    }

    void SelectTip()
    {
        int tipTextCount = Random.Range(0, 21);
        GetText((int)Texts.TipText).text = Managers.GetText(Define.TipText + tipTextCount);
    }
}
