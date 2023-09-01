using MessMathI18n;
using StoryData;
using System.Collections;
using System.Collections.Generic;
using TutorialDatas;
using UnityEngine;

public class UI_LobbyTutorial : UI_Popup
{
    JsonReader jsonReader;
    List<TalkData> storyTalkData = new List<TalkData>();
    int maxCount;
    int count = 0;

    enum GameObjects
    {
        Panel,
    }

    enum Images
    {
        BackGroundImage,
        PlayerImage,
        CharacterImage,
        CharacterBG,
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

        BindObject(typeof(GameObjects));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        jsonReader = new JsonReader();

        if (LocalizationManager.Get().GetSelectedLanguage() == Language.KOREAN)
        {
            storyTalkData = jsonReader.ReadStoryJson(Application.persistentDataPath + "/" + 12 + "_LobbyTutorial_KOR.json").talkDataList;
        }
        else
        {
            storyTalkData = jsonReader.ReadStoryJson(Application.persistentDataPath + "/" + 13 + "_LobbyTutorial_EN.json").talkDataList;
        }

        maxCount = storyTalkData.Count;

        GetText((int)Texts.CharacterNameTMP).text = "";
        GetText((int)Texts.DialogueTMP).text = "";
        GetButton((int)Buttons.nxtButton).gameObject.BindEvent(OnClickNxtBtn);

        return true;
    }

    public void OnClickNxtBtn()
    {
        //PlayerPrefs.SetInt("DoTutorial", 2);
        if (!Managers.TextEffect.isTypingEnd)
        {
            Managers.TextEffect.SetFastSpeed();
            return;
        }
        if (++count >= maxCount)
        {
            Managers.DBManager.SetIsCompletedTutorial(true);
            Managers.UI.ClosePopupUI(this);
            return;
        }

        //PlayerPrefs.SetInt("WatchedStory", count);
        Managers.SceneEffect.ChangeCharacterBG(GetImage((int)Images.CharacterBG), storyTalkData[count].characterName);
        Managers.SceneEffect.ChangeCharacter(GetImage((int)Images.PlayerImage), GetImage((int)Images.CharacterImage), storyTalkData[count].characterName, storyTalkData[count].expression);


        if (storyTalkData[count].txtEffect == "MAX")
        {
            GetText((int)Texts.DialogueTMP).fontSize = 100;
        }
        else
        {
            GetText((int)Texts.DialogueTMP).fontSize = 80;
        }

        GetText((int)Texts.CharacterNameTMP).text = storyTalkData[count].characterName;

        Managers.TextEffect.SetNormalSpeed();
        Managers.TextEffect.Typing(storyTalkData[count].dialogue, GetText((int)Texts.DialogueTMP));

        // Sound
        Managers.Sound.Play("ClickBtnEff");
        //talkDatas[i].soundEffect
        //talkDatas[i].soundEffectDuration
    }
}
