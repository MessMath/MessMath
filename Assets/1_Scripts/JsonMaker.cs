using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using StoryData;

public class JsonMaker : MonoBehaviour
{
    string DBAddress = "https://docs.google.com/spreadsheets/d/13RmRePVU38TYU10FdhqtJ5DJWCDioeqfkFKqfte7Wv0";
    string sheetNum = "0";
    List<string> range = new List<string>(); 
    List<string> fileName = new List<string>();
    TalkInfo storyTalkInfo = new TalkInfo();

    void Awake() 
    {
        AddRange();
        AddFileName();
    }

    void Start()
    {
        StartCoroutine(DialogNetConnect());
    }

    void AddRange()
    {
        range.Add("A2:G44");
    }

    void AddFileName()
    {
        fileName.Add("EnterGameStory");
    }

    // ANCHOR 구글 docs에서 데이터 읽기
    IEnumerator DialogNetConnect() 
    {      
        for(int i = 0; i < range.Count; i++)
        {
            string URL = DBAddress + "/export?format=tsv&gid=" + sheetNum + "&range=" + range[i];
            Debug.Log(URL);

            UnityWebRequest www = UnityWebRequest.Get(URL);
            yield return www.SendWebRequest();

            string data = www.downloadHandler.text;
            Debug.Log(data);

            ParsingData(data);
            MakeJsonFile(i);
        }
        
    }

    // ANCHOR 데이터 파싱(임시)
    /// <summary>
    /// 데이터를 스트링으로 파싱하는 함수
    /// </summary>
    /// <param string="data">
    /// 파싱할 데이터
    /// </param>
    /// <returns>
    ///  파싱된 데이터
    /// </returns>
    void ParsingData(string data)
    {
        string[] lines = data.Split('\n');
        storyTalkInfo.talkDataList = new List<TalkData>();
        
        for(int i = 0; i < lines.Length; i++)
        {
            TalkData talkData = new TalkData();
            string[] tap = lines[i].Split('\t');
            talkData.characterName = tap[0];
            Debug.Log(talkData.characterName);
            talkData.dialogue = tap[1];
            Debug.Log(talkData.dialogue);
            talkData.sceneEffect = tap[2];
            talkData.soundEffect = tap[3];
            talkData.soundEffectDuration = float.Parse(tap[4]);
            talkData.backgroundImg = tap[5];
            talkData.expression = tap[6];
            storyTalkInfo.talkDataList.Add(talkData);
        }
    }
    
    // ANCHOR 파일로 저장
    /// <summary>
    /// 데이터를 json 파일에 저장하는 함수
    /// </summary>
    /// <param string="data">
    /// 파일에 저장할 데이터
    /// </param>
    void MakeJsonFile(int i)
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
            MakeJsonFile(i);
        }
    }
}
