using TipDatas;
using MessMathI18n;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class UI_MakeTxtFile : UI_Scene
{
    JsonReader jsonReader;
    List<TipData> tipData = new List<TipData>();
    string[] obtainedMagicCircle;
    List<string> obtainedCollections = new List<string>();
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

        InitObtainedCollections();

        //GetObject((int)GameObjects.MagicCircle).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Collections/" + GetRandomMagicCircleSprite());
        GetText((int)Texts.LogTMP).text = "Loading...";

        /*jsonReader = new JsonReader();

        if (LocalizationManager.Get().GetSelectedLanguage() == Language.KOREAN)
        {
            tipData = jsonReader.ReadTipJson(Application.persistentDataPath + "/" + 5 + "_Tip_KOR.json").tipDataList;
        }
        else
        {
            tipData = jsonReader.ReadTipJson(Application.persistentDataPath + "/" + 11 + "_Tip_EN.json").tipDataList;
        }*/

        return true;
    }

    async void InitObtainedCollections()
    {
        if (Managers.GoogleSignIn.GetUID() == null) return;
        obtainedCollections = Managers.DBManager.ParseObtanined(await Managers.DBManager.GetObtainedCollections(Managers.GoogleSignIn.GetUID()));
    }


    void SelectTip()
    {
        int tipTextCount = Random.Range(0, 21);

        if (LocalizationManager.Get().GetSelectedLanguage() == Language.KOREAN)
            GetText((int)Texts.TipText).text = Managers.GetText(Define.TipText + tipTextCount);
        else 
            GetText((int)Texts.TipText).text = Managers.GetText(Define.TipText + tipTextCount);
    }

    bool CheckHaveMagicCircleImage()
    {
        if (obtainedCollections == null) return false;

        for (int i = 0; i < obtainedCollections.Count; i++)
        {
            if (obtainedCollections[i] == "light_magic_circle") return true;
            if (obtainedCollections[i] == "moon_magic_circle") return true;
            if (obtainedCollections[i] == "old_magic_circle") return true;
        }

        return false;
    }

    string GetRandomMagicCircleSprite()
    {
        if (!CheckHaveMagicCircleImage()) return "";

        for (int i = 0; i < obtainedCollections.Count; i++)
        {
            if (obtainedCollections[i] == "light_magic_circle") obtainedMagicCircle.Append<string>(obtainedCollections[i]);
            if (obtainedCollections[i] == "moon_magic_circle") obtainedMagicCircle.Append<string>(obtainedCollections[i]);
            if (obtainedCollections[i] == "old_magic_circle") obtainedMagicCircle.Append<string>(obtainedCollections[i]);
        }

        return obtainedMagicCircle[UnityEngine.Random.Range(0, obtainedMagicCircle.Count())];
    }
}
