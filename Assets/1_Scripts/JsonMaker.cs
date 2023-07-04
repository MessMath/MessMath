using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using StoryData;
using StoreDatas;
using TutorialDatas;

public class JsonMaker : MonoBehaviour
{
    string[] DBAddress = {
        "https://docs.google.com/spreadsheets/d/13RmRePVU38TYU10FdhqtJ5DJWCDioeqfkFKqfte7Wv0",
        "https://docs.google.com/spreadsheets/d/1iYRgERiJ5bsqjfg_ikzWQbgQYA7OcRiUnZsZ64qDb2M",
        "https://docs.google.com/spreadsheets/d/18Ba5zNPk4IxahKMCI3498xQSPbOmDU-gYdiXF2n8wWM",
        "https://docs.google.com/spreadsheets/d/1SLsoFg1UYiPSzXfYs8j7lo-gDRo71pSopK1saJHtATU",
    };
    string sheetNum = "0";
    List<string> range = new List<string>(); 
    List<string> fileName = new List<string>();
    TalkInfo storyTalkInfo = new TalkInfo();
    StoreInfo storeInfo = new StoreInfo();
    TutorialInfo tutorialInfo = new TutorialInfo();
    bool[] isDone = {false, false, false, false};
    bool doneCompletely = false;

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
        //if(isDone)Managers.Scene.ChangeScene(Define.Scene.LobbyScene);
        /*for(int i = 0; i < isDone.Length; i++)
        {
            if(!isDone[i]) continue;
            doneCompletely = true;
        }*/
        if(isDone[0] && isDone[1] && isDone[2] && isDone[3]) doneCompletely = true;
        if (doneCompletely)
        {
            //isDone = false;
            // 진단평가가 완료된 상태라면 로비로 이동
            if(PlayerPrefs.GetInt("DoDiagnosis") == 1)
            {
                Managers.Game.CurrentStatus = Define.CurrentStatus.LEARNING;
                Managers.Scene.ChangeScene(Define.Scene.LobbyScene);
            }
            // 진단평가가 되어 있지 않다면 진단평가부터
            else
            {
                Managers.Scene.ChangeScene(Define.Scene.DiagnosisScene);
            }
        }
        else
        {
            //GetDatas();
        }
    }

    void GetDatas()
    {
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
        range.Add("A2:G46");
        range.Add("A2:D4");
        range.Add("A2:D4");
        range.Add("A2:A7");
    }

    void AddFileName()
    {
        fileName.Add("EnterGameStory");
        fileName.Add("StoreGauss");
        fileName.Add("StoreCollection");
        fileName.Add("Tutorial");
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
            talkData.expression = tap[6];
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
}
