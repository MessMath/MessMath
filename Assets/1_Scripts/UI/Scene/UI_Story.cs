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
    GameObject replayPopup;
    bool isFadeDone = false;

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
        ReplayButton,
        TmpNxtButton,
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
        storyTalkData = jsonReader.ReadStoryJson(Application.persistentDataPath + "/" + 0 + "_EnterGameStory.json").talkDataList;
        maxCount = storyTalkData.Count;
        replayPopup = Managers.UI.ShowPopupUI<UI_ReplayStory>().gameObject;
        replayPopup.SetActive(false);

        GetText((int)Texts.CharacterNameTMP).text = "";
        GetText((int)Texts.DialogueTMP).text = "";
        GetButton((int)Buttons.TmpNxtButton).gameObject.BindEvent(StartBtn);
        GetButton((int)Buttons.nxtButton).gameObject.BindEvent(OnClickNxtBtn);
        //GetButton((int)Buttons.ReplayButton).gameObject.BindEvent(OnClickReplayBtn);
        GetButton((int)Buttons.ReplayButton).gameObject.BindEvent(Skip);

        // Sound
        Managers.Sound.Clear();
        Managers.Sound.Play("StoryBgm", Define.Sound.Bgm);

        return true;
    }
    void Skip()
    {
        PlayerPrefs.SetInt("WatchedStory", -2);
        Managers.Scene.ChangeScene(Define.Scene.LobbyScene);
    }

    void StartBtn()
    {
        OnClickNxtBtn();
        GetButton((int)Buttons.TmpNxtButton).gameObject.SetActive(false);
    }

    void OnClickNxtBtn()
    {
        if(!Managers.TextEffect.isTypingEnd)
        {
            Managers.TextEffect.SetFastSpeed();
            return;
        }
        if(++count >= maxCount) {
            PlayerPrefs.SetInt("WatchedStory", -2);
            if (PlayerPrefs.GetInt("DoTutorial") == 2)
                Managers.Scene.ChangeScene(Define.Scene.StoryGameScene);
            else
                Managers.Scene.ChangeScene(Define.Scene.TutorialGameScene);
            return;
        }
        PlayerPrefs.SetInt("WatchedStory", count);
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
        replayPopup.SetActive(true);
        replayPopup.GetComponent<UI_ReplayStory>().AddReplayStory(storyTalkData[count].characterName, storyTalkData[count].dialogue, storyTalkData[count].expression);
        replayPopup.SetActive(false);

        // Sound
        Managers.Sound.Play("ClickBtnEff");
        //talkDatas[i].soundEffect
        //talkDatas[i].soundEffectDuration
    }

    void OnClickReplayBtn()
    {
        replayPopup.SetActive(true);
    }
}
