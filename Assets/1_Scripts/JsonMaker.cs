using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using StoryData;
using StoreDatas;
using TutorialDatas;
using DiagnosisDatas;
using TipDatas;

public class JsonMaker : MonoBehaviour
{
    string[] DBAddress = {
        "https://docs.google.com/spreadsheets/d/13RmRePVU38TYU10FdhqtJ5DJWCDioeqfkFKqfte7Wv0", // 스토리 0
        "https://docs.google.com/spreadsheets/d/1iYRgERiJ5bsqjfg_ikzWQbgQYA7OcRiUnZsZ64qDb2M", // 가호정보 1
        "https://docs.google.com/spreadsheets/d/18Ba5zNPk4IxahKMCI3498xQSPbOmDU-gYdiXF2n8wWM", // 수집품 정보 2
        "https://docs.google.com/spreadsheets/d/1SLsoFg1UYiPSzXfYs8j7lo-gDRo71pSopK1saJHtATU", // 튜토리얼 3
        "https://docs.google.com/spreadsheets/d/1A32zfYnZVIVMRCm4aKVXpt1vl1v_DVCe9Eut_2zIyy8", // 진단평가 4
        "https://docs.google.com/spreadsheets/d/1PQqQ221jaD5sAEUOksi8i-8FH9Qn4430iRqOqRXgnI0", // 의상 정보 5
        "https://docs.google.com/spreadsheets/d/1JVqCDQPs_rfZPhg3x05gzdvr5cDKgFYfUd5MAnB-rmM", // 스토리_영어 6
        "https://docs.google.com/spreadsheets/d/1WkDvfIOCIUD3NP21QboDDwPcCfuifDmoiFSYpCHkld4", // 가호정보_영어 7
        "https://docs.google.com/spreadsheets/d/1s6NE0G3nOnoRmRJcLa2osfqmwvTfZXjlI9-IPmmG7LM", // 수집폼 정보_영어 8
        "https://docs.google.com/spreadsheets/d/1OdKxEzgRwspt6SoLH0_Wnd8-33TqOsmLeKO7f5Hj5cg",  // 튜토리얼 정보_영어 9
        "https://docs.google.com/spreadsheets/d/1GAwrbav-8b991er9UQd9DwAal0O9nYNnfiW4aN9qofU", // 진단평가_영어 10
        "https://docs.google.com/spreadsheets/d/17E7BYZtHXLIgfHeGh0sqGE93TF3EZyLFkJ2ddcPgxsA", // 의상 정보_영어 11
        "https://docs.google.com/spreadsheets/d/12y7mWorBoIKE_zHAAINlxRFv8044bv57xv6QdW1CkKs", // 로비 튜토리얼 12
        "https://docs.google.com/spreadsheets/d/1WkWlJXH4WbmTLB0zg89g3DDRXuaup2GqDDD5L-hs5d0", // 로비 튜토리얼_영어 13
    };
    string sheetNum = "0";
    List<string> range = new List<string>(); 
    List<string> fileName = new List<string>();
    TalkInfo storyTalkInfo = new TalkInfo();
    StoreInfo storeInfo = new StoreInfo();
    TutorialInfo tutorialInfo = new TutorialInfo();
    DiagnosisInfo diagnosisInfo = new DiagnosisInfo();
    TipInfo tipInfo = new TipInfo();
    bool madeFile = false;
    bool[] isDone = {false, false, false, false, false, false, false, false, false, false, false, false, false, false };
    
    void Awake() 
    {
        AddRange();
        AddFileName();
    }

    void Start()
    {
        GetDatas();
    }

    void Update()
    {
        if(PlayerPrefs.GetInt("DoDiagnosis") == 1 && isDone[0] && isDone[1] && isDone[2] && isDone[3] && isDone[4] && isDone[5] && isDone[6] && isDone[7] && isDone[8] && isDone[9] && isDone[10] && isDone[11] && isDone[12] && isDone[13])
        {
            Managers.Game.CurrentStatus = Define.CurrentStatus.LEARNING;
            Managers.Scene.ChangeScene(Define.Scene.LobbyScene);
        }
        // 진단평가가 되어 있지 않다면 진단평가부터
        else if(isDone[0] && isDone[1] && isDone[2] && isDone[3] && isDone[4] && isDone[5] && isDone[6] && isDone[7] && isDone[8] && isDone[9] && isDone[10] && isDone[11] && isDone[12] && isDone[13])
        {
            Managers.Scene.ChangeScene(Define.Scene.DiagnosisScene);
        }
        if(madeFile == false) {
                GetDatas();
                madeFile = true;
        }
    }

    void GetDatas()
    {
        if(madeFile) return;
        for(int i = 0; i < isDone.Length; i++)
        {
            if(isDone[i]) {
                StopCoroutine(NetConnect(i));
                continue;
            }
            StartCoroutine(NetConnect(i));
        }
    }

    void AddRange()
    {
        range.Add("A2:G82");
        range.Add("A2:G7");
        range.Add("A2:G7");
        range.Add("A2:A7");
        range.Add("A2:A6");
        range.Add("A2:G11");
        range.Add("A2:G82");
        range.Add("A2:G7");
        range.Add("A2:G7");
        range.Add("A2:A7");
        range.Add("A2:A6");
        range.Add("A2:G11");
        range.Add("A2:G23");
        range.Add("A2:G23");
    }

    void AddFileName()
    {
        fileName.Add("EnterGameStory_KOR");
        fileName.Add("StoreGrace_KOR");
        fileName.Add("StoreCollection_KOR");
        fileName.Add("Tutorial_KOR");
        fileName.Add("Diagnosis_KOR");
        fileName.Add("StoreClothes_KOR");
        fileName.Add("EnterGameStory_EN");
        fileName.Add("StoreGrace_EN");
        fileName.Add("StoreCollection_EN");
        fileName.Add("Tutorial_EN");
        fileName.Add("Diagnosis_EN");
        fileName.Add("StoreClothes_EN");
        fileName.Add("LobbyTutorial_KOR");
        fileName.Add("LobbyTutorial_EN");
    }

    // ANCHOR 구글 docs에서 데이터 읽기
    IEnumerator NetConnect(int idx) 
    {      
        string URL = DBAddress[idx] + "/export?format=tsv&gid=" + sheetNum + "&range=" + range[idx];
        Debug.Log(URL);

        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;
        Debug.Log(data);

        switch(idx)
        {
            case 0: 
                ParsingDialogueData(data);
                MakeDialgoueJsonFile(idx);
                break;
            case 1:
                ParsingStoreData(data);
                MakeStoreJsonFile(idx);
                break;
            case 2:
                ParsingStoreData(data);
                MakeStoreJsonFile(idx);
                break;
            case 3:
                ParsingTutorialData(data);
                MakeTutorailJsonFile(idx);
                break;
            case 4:
                ParsingDiagnosisData(data);
                MakeDiagnosisJsonFile(idx);
                break;
            case 5:
                ParsingStoreData(data);
                MakeStoreJsonFile(idx);
                break;
            case 6:
                ParsingDialogueData(data);
                MakeDialgoueJsonFile(idx);
                break;
            case 7:
                ParsingStoreData(data);
                MakeStoreJsonFile(idx);
                break;
            case 8:
                ParsingStoreData(data);
                MakeStoreJsonFile(idx);
                break;
            case 9:
                ParsingTutorialData(data);
                MakeTutorailJsonFile(idx);
                break;
            case 10:
                ParsingDiagnosisData(data);
                MakeDiagnosisJsonFile(idx);
                break;
            case 11:
                ParsingStoreData(data);
                MakeStoreJsonFile(idx);
                break;
            case 12:
                ParsingDialogueData(data);
                MakeDialgoueJsonFile(idx);
                break;
            case 13:
                ParsingDialogueData(data);
                MakeDialgoueJsonFile(idx);
                break;
        }
    }

    void ParsingDialogueData(string data)
    {
        string[] lines = data.Split('\n');
        storyTalkInfo.talkDataList = new List<TalkData>();
        
        for(int i = 0; i < lines.Length; i++)
        {
            TalkData talkData = new TalkData();
            string[] tap = lines[i].Split('\t');
            talkData.characterName = tap[0];
            talkData.dialogue = tap[1];
            talkData.sceneEffect = tap[2];
            talkData.soundEffect = tap[3];
            talkData.soundEffectDuration = float.Parse(tap[4]);
            talkData.backgroundImg = tap[5];
            talkData.expression = tap[6].Replace("\r","");
            storyTalkInfo.talkDataList.Add(talkData);
        }
    }

    void ParsingStoreData(string data)
    {
        string[] lines = data.Split('\n');
        storeInfo.storeDataList = new List<StoreData>();
        
        for(int i = 0; i < lines.Length; i++)
        {
            StoreData storeData = new StoreData();
            string[] tap = lines[i].Split('\t');
            storeData.name = tap[0];
            storeData.explanation = tap[1];
            storeData.img = tap[2];
            storeData.price = int.Parse(tap[3]);
            storeData.mode = tap[4];
            storeData.bgImage = tap[5];
            storeData.img2 = tap[6];
            storeInfo.storeDataList.Add(storeData);
        }
    }

    void ParsingTutorialData(string data)
    {
        string[] lines = data.Split('\n');
        tutorialInfo.tutorialDataList = new List<TutorialData>();
        
        for(int i = 0; i < lines.Length; i++)
        {
            TutorialData tutorialData = new TutorialData();
            tutorialData.dialogue = lines[i];
            tutorialInfo.tutorialDataList.Add(tutorialData);
        }
    }

    void ParsingDiagnosisData(string data)
    {
        string[] lines = data.Split('\n');
        diagnosisInfo.diagnosisDataList = new List<DiagnosisData>();

        for (int i = 0; i < lines.Length; i++)
        {
            DiagnosisData diagnosisData = new DiagnosisData();
            diagnosisData.dialogue = lines[i];
            diagnosisInfo.diagnosisDataList.Add(diagnosisData);
        }
    }

    void ParsingTipData(string data)
    {
        string[] lines = data.Split('\n');
        tipInfo.tipDataList = new List<TipData>();

        for (int i = 0; i < lines.Length; i++)
        {
            TipData tipData = new TipData();
            tipData.tipText = lines[i];
            tipInfo.tipDataList.Add(tipData);
        }
    }

    void MakeDialgoueJsonFile(int i)
    {
        string filePath = Application.persistentDataPath + "/" + i + "_" + fileName[i] +".json";
        StreamWriter sw;
        FileStream fs;

        string json = JsonUtility.ToJson(storyTalkInfo);

        if (!File.Exists(filePath))
        {
            fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            sw = new StreamWriter(fs);
            sw.WriteLine(json);
            sw.Flush();
            sw.Close();
            fs.Close();
        }
        else if (File.Exists(filePath))
        {
            File.Delete(filePath);
            MakeDialgoueJsonFile(i);
            isDone[i] = true;
        }
    }

    void MakeStoreJsonFile(int i)
    {
        string filePath = Application.persistentDataPath + "/" + i + "_" + fileName[i] +".json";
        StreamWriter sw;
        FileStream fs;

        string json = JsonUtility.ToJson(storeInfo);

        if (!File.Exists(filePath))
        {
            fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            sw = new StreamWriter(fs);
            sw.WriteLine(json);
            sw.Flush();
            sw.Close();
            fs.Close();
        }
        else if (File.Exists(filePath))
        {
            File.Delete(filePath);
            MakeStoreJsonFile(i);
            isDone[i] = true;
        }
    }

    void MakeTutorailJsonFile(int i)
    {
        string filePath = Application.persistentDataPath + "/" + i + "_" + fileName[i] +".json";
        StreamWriter sw;
        FileStream fs;

        string json = JsonUtility.ToJson(tutorialInfo);

        if (!File.Exists(filePath))
        {
            fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            sw = new StreamWriter(fs);
            sw.WriteLine(json);
            sw.Flush();
            sw.Close();
            fs.Close();
        }
        else if (File.Exists(filePath))
        {
            File.Delete(filePath);
            MakeTutorailJsonFile(i);
            Debug.Log("Done Making 3_Tutorial.json File");
            isDone[i] = true;
        }
    }

    void MakeDiagnosisJsonFile(int i)
    {
        string filePath = Application.persistentDataPath + "/" + i + "_" + fileName[i] + ".json";
        StreamWriter sw;
        FileStream fs;

        string json = JsonUtility.ToJson(diagnosisInfo);

        if (!File.Exists(filePath))
        {
            fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            sw = new StreamWriter(fs);
            sw.WriteLine(json);
            sw.Flush();
            sw.Close();
            fs.Close();
        }
        else if (File.Exists(filePath))
        {
            File.Delete(filePath);
            MakeDiagnosisJsonFile(i);
            Debug.Log("Done Making Diagnosis.json File");
            isDone[i] = true;
        }
    }

    void MakeTipJsonFile(int i)
    {
        string filePath = Application.persistentDataPath + "/" + i + "_" + fileName[i] + ".json";
        StreamWriter sw;
        FileStream fs;

        string json = JsonUtility.ToJson(tipInfo);

        if (!File.Exists(filePath))
        {
            fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            sw = new StreamWriter(fs);
            sw.WriteLine(json);
            sw.Flush();
            sw.Close();
            fs.Close();
        }
        else if (File.Exists(filePath))
        {
            File.Delete(filePath);
            MakeTipJsonFile(i);
            Debug.Log("Done Making tip.json File");
            isDone[i] = true;
        }
    }

}
