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

        //GetObject((int)GameObjects.MagicCircle).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Collections/" + GetRandomMagicCircleSprite());
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

    bool CheckHaveMagicCircleImage()
    {
        if (Managers.UserMng.user.UID == null) return false;
        if (Managers.UserMng.GetObtainedCollections() == null) return false;

        for (int i = 0; i < Managers.UserMng.GetObtainedCollections().Count; i++)
        {
            if (Managers.UserMng.GetObtainedCollections()[i] == "light_magic_circle") return true;
            if (Managers.UserMng.GetObtainedCollections()[i] == "moon_magic_circle") return true;
            if (Managers.UserMng.GetObtainedCollections()[i] == "old_magic_circle") return true;
        }

        return false;
    }

    string GetRandomMagicCircleSprite()
    {
        if (!CheckHaveMagicCircleImage()) return "";

        for (int i = 0; i < Managers.UserMng.GetObtainedCollections().Count; i++)
        {
            if (Managers.UserMng.GetObtainedCollections()[i] == "light_magic_circle") obtainedMagicCircle.Append<string>(Managers.UserMng.GetObtainedCollections()[i]);
            if (Managers.UserMng.GetObtainedCollections()[i] == "moon_magic_circle") obtainedMagicCircle.Append<string>(Managers.UserMng.GetObtainedCollections()[i]);
            if (Managers.UserMng.GetObtainedCollections()[i] == "old_magic_circle") obtainedMagicCircle.Append<string>(Managers.UserMng.GetObtainedCollections()[i]);
        }

        return obtainedMagicCircle[UnityEngine.Random.Range(0, obtainedMagicCircle.Count())];
    }
}
