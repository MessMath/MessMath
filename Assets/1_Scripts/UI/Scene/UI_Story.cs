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
        FadeImage,
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
        GetButton((int)Buttons.nxtButton).gameObject.BindEvent(OnClickNxtBtn);
        return true;
    }

    void OnClickNxtBtn()
    {
        if(!Managers.TextEffect.isTypingEnd)
        {
            Managers.TextEffect.SetFastSpeed();
            return;
        }
        if(++count >= maxCount) {
            Managers.Scene.ChangeScene(Define.Scene.StoryGameScene); 
            return;
        }
        Debug.Log("Cnt: " + count);
        if(storyTalkData[count].sceneEffect!="") GetButton((int)Buttons.nxtButton).interactable = false;
        Managers.SceneEffect.SceneEffect(GetImage((int)Images.FadeImage),GetButton((int)Buttons.nxtButton), storyTalkData[count].sceneEffect);
        Managers.SceneEffect.ChangeBackground(GetImage((int)Images.BackGroundImage), storyTalkData[count].backgroundImg);
        Managers.SceneEffect.ChangeCharacter(GetImage((int)Images.PlayerImage), GetImage((int)Images.CharacterImage), storyTalkData[count].characterName, storyTalkData[count].expression);

        if(storyTalkData[count].txtEffect == "MAX") {
            GetText((int)Texts.DialogueTMP).fontSize = 100;
        }
        else {
            GetText((int)Texts.DialogueTMP).fontSize = 80;
        }

        GetText((int)Texts.CharacterNameTMP).text = storyTalkData[count].characterName;

        Managers.TextEffect.SetNormalSpeed();
        Managers.TextEffect.Typing(storyTalkData[count].dialogue, GetText((int)Texts.DialogueTMP));

        //talkDatas[i].soundEffect
        //talkDatas[i].soundEffectDuration
    }
}
