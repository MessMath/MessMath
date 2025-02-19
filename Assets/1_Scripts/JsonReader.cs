using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using StoryData;
using StoreDatas;
using TutorialDatas;
using DiagnosisDatas;
using TipDatas;

public class JsonReader
{  
    // json코드 읽는 함수
    public TalkInfo ReadStoryJson(string path)
    {
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<TalkInfo>(json);
    }

    public TutorialInfo ReadTutorialJson(string path)
    {
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<TutorialInfo>(json);
    }

    public DiagnosisInfo ReadDiagnosisJson(string path)
    {
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<DiagnosisInfo>(json);
    }

    public TipInfo ReadTipJson(string path)
    {
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<TipInfo>(json);
    }

    public StoreInfo ReadStoreJson(string path)
    {
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<StoreInfo>(json);
    }
}
