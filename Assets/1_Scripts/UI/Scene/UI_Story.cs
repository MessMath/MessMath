using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StoryData;

public class UI_Story : UI_Scene
{
    JsonReader jsonReader;
    int maxCount;
    int count = -1;
    List<TalkData> storyTalkData = new List<TalkData>();
     enum Images
    {
        BackGroundImage,
        PlayerImage,
        CharacterImage,
    }

    enum Buttons
    {
        nxtButton,
    }

    enum Texts
    {
        CharacterNameTMP,
        DialogueTMP,
    }

    private void Start()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        jsonReader = new JsonReader();
        storyTalkData = jsonReader.ReadJson(Application.persistentDataPath + "/" + 0 + "_EnterGameStory.json").talkDataList;
        maxCount = storyTalkData.Count;

        GetText((int)Texts.CharacterNameTMP).text = "";
        GetText((int)Texts.DialogueTMP).text = "";
        GetButton((int)Buttons.nxtButton).gameObject.BindEvent(OnClickPanel);
        return true;
    }

    void OnClickPanel( )
    {
        if(++count >= maxCount) {
            Managers.Scene.ChangeScene(Define.Scene.StoryGameScene); 
            return;
        }
        //storyTalkData[count].backgroundImg
        if(storyTalkData[count].txtEffect == "MAX") {
            GetText((int)Texts.DialogueTMP).fontSize = 100;
        }
        else {
            GetText((int)Texts.DialogueTMP).fontSize = 80;
        }
        GetText((int)Texts.CharacterNameTMP).text = storyTalkData[count].characterName;
        GetText((int)Texts.DialogueTMP).text = storyTalkData[count].dialogue;
        //talkDatas[i].sceneEffect
        //talkDatas[i].soundEffect
        //talkDatas[i].soundEffectDuration
        //talkDatas[i].expression
    }
}
