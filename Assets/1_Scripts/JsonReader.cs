using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using StoryData;

public class JsonReader : MonoBehaviour
{
    List<string> fileName = new List<string>();
    void AddFileName()
    {
        fileName.Add("EnterGameStory");
    }
  
    // json코드 읽는 함수
    TalkInfo ReadJson(string path)
    {
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<TalkInfo>(json);
    }

    void printReadedTalkData(List<TalkData>  talkDatas)
    {      
        Debug.Log($"------talkData------talkData.Count: {talkDatas.Count}");  
        for(int i = 0; i < talkDatas.Count; i++)
        {
            Debug.Log($"characterName: {talkDatas[i].characterName}\t dialogue: {talkDatas[i].dialogue}\t sceneEffect: {talkDatas[i].sceneEffect} \t soundEffect:{talkDatas[i].soundEffect} \t soundEffectDuration: {talkDatas[i].soundEffectDuration}\t backgroundImg: {talkDatas[i].backgroundImg}\t txtEffect: {talkDatas[i].txtEffect}\t expression:{talkDatas[i].expression}");
        }
    }

    public void OnClickedPrintBtn()
    {
        AddFileName();
        List<TalkData> storyTalkData = ReadJson(Application.persistentDataPath + "/" + 0 + "_" + fileName[0] +".json").talkDataList;
        printReadedTalkData(storyTalkData);
    }
   
}
