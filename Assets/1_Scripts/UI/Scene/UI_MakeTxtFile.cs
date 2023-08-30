using TipDatas;
using MessMathI18n;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MakeTxtFile : UI_Scene
{
    JsonReader jsonReader;
    List<TipData> tipData = new List<TipData>();

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
        GetText((int)Texts.LogTMP).text = "Loading...";

        jsonReader = new JsonReader();

        if (LocalizationManager.Get().GetSelectedLanguage() == Language.KOREAN)
        {
            tipData = jsonReader.ReadTipJson(Application.persistentDataPath + "/" + 5 + "_Tip_KOR.json").tipDataList;
        }
        else
        {
            tipData = jsonReader.ReadTipJson(Application.persistentDataPath + "/" + 11 + "_Tip_EN.json").tipDataList;
        }

        return true;
    }
    void SelectTip()
    {
        int tipTextCount = Random.Range(0, 21);

        if (LocalizationManager.Get().GetSelectedLanguage() == Language.KOREAN)
            GetText((int)Texts.TipText).text = Managers.GetText(Define.TipText + tipTextCount);
        else 
            GetText((int)Texts.TipText).text = Managers.GetText(Define.TipText + tipTextCount);
    }
}
