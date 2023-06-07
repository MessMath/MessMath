using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class TxtFileMaker : MonoBehaviour
{
    string DBAddress = "https://docs.google.com/spreadsheets/d/13RmRePVU38TYU10FdhqtJ5DJWCDioeqfkFKqfte7Wv0";
    string sheetNum = "0";
    List<string> range = new List<string>(); 
    List<string> fileName = new List<string>();

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
        range.Add("A2:B44");
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

            SaveData(data, i);
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
    string ParsingData(string data)
    {
        string characterName;
        string dialogue;
        string result = "";

        string[] lines = data.Split('\n');
        
        for(int i = 0; i < lines.Length; i++)
        {
            characterName = lines[i].Split('\t')[0];
            Debug.Log(characterName);
            dialogue = lines[i].Split('\t')[1];
            Debug.Log(dialogue);

            result += characterName + "\t" + dialogue +"\n";
        }

        return result;
    }
    
    // ANCHOR 파일로 저장
    /// <summary>
    /// 데이터를 파일에 저장하는 함수
    /// </summary>
    /// <param string="data">
    /// 파일에 저장할 데이터
    /// </param>
    void SaveData(string data, int i)
    {
        string filePath = "Assets/Resources/DialogueData/";
        StreamWriter sw;

        if (!File.Exists(filePath))
        {
            sw = new StreamWriter(filePath + i + "_" + fileName[i] +".txt");
            sw.WriteLine(data);
            sw.Flush();
            sw.Close();
        }
        else if (File.Exists(filePath))
        {
            File.Delete(filePath);
            SaveData(data, i);
        }
    }
}
