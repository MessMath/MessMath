using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StoryData;
using MessMathI18n;

public class UI_Story : UI_Scene
{
    JsonReader jsonReader;
    int maxCount;
    int count = -1;
    int unlockCnt = 0;
    List<TalkData> storyTalkData = new List<TalkData>();
    GameObject replayPopup;
    bool isFadeDone = false;
    bool openSide = true;
    bool enteredOffice = false;

    enum GameObjects
    {
        Panel,
        SidePanel,
        SchoolHallway,
        EntranceOffice,
        Library,
    }
    enum Images
    {
        BackGroundImage,
        PlayerImage,
        CharacterImage,
        GaussImage,
        FadeImage,
        OpenedSide,
        ClosedSide,
        CharacterBG,
        LeftImage,
        RightImage,
        FirstBrokeImg,
        SecondBrokeImg,
        ThirdBrokeImg,
        FogImage,
        SealCircleImage,
        SpeechBubbleImg,
        SmallSpeechBubbleImg,
    }
    enum Buttons
    {
        nxtButton,
        SettingButton,
        ReplayButton,
        SkipButton,
        TmpNxtButton,
        NeumannBtn,
        StainedGlassBtn,
        EinsteinBtn,
        NewtonBtn,
        PythagorasBtn,
        GaussBtn,
        EntranceBtn,
        DoorBtn,
        LockedBookBtn,
        LockedBtn,
    }
    enum Texts
    {
        CharacterNameTMP,
        DialogueTMP,
        TouchScreenTMP,
        SmallTMP,
        SpeechTMP,
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

        GetText((int)Texts.TouchScreenTMP).text = I18n.Get(I18nDefine.STORY_TOUCH_SCREEN);

        jsonReader = new JsonReader();

        if (LocalizationManager.Get().GetSelectedLanguage() == Language.KOREAN)
        {
            storyTalkData = jsonReader.ReadStoryJson(Application.persistentDataPath + "/" + 0 + "_EnterGameStory_KOR.json").talkDataList;
        }
        else
        {
            storyTalkData = jsonReader.ReadStoryJson(Application.persistentDataPath + "/" + 6 + "_EnterGameStory_EN.json").talkDataList;
        }

        maxCount = storyTalkData.Count;

        replayPopup = Managers.UI.ShowPopupUI<UI_ReplayStory>().gameObject;
        replayPopup.SetActive(false);

        GetText((int)Texts.CharacterNameTMP).text = "";
        GetText((int)Texts.DialogueTMP).text = "";
        GetButton((int)Buttons.TmpNxtButton).gameObject.BindEvent(StartBtn);
        GetButton((int)Buttons.nxtButton).gameObject.BindEvent(OnClickNxtBtn);
        GetButton((int)Buttons.ReplayButton).gameObject.BindEvent(OnClickReplayBtn);
        GetButton((int)Buttons.SkipButton).gameObject.BindEvent(Skip);
        GetButton((int)Buttons.LockedBookBtn).gameObject.BindEvent(OnClickedLockedBookBtn);

        GetButton((int)Buttons.NeumannBtn).gameObject.BindEvent(OnClickedNeumannBtn);
        GetButton((int)Buttons.StainedGlassBtn).gameObject.BindEvent(OnClickedStainedGlassBtn);
        GetButton((int)Buttons.EinsteinBtn).gameObject.BindEvent(OnClickedEinsteinBtn);
        GetButton((int)Buttons.NewtonBtn).gameObject.BindEvent(OnClickedNewtonBtn);
        GetButton((int)Buttons.PythagorasBtn).gameObject.BindEvent(OnClickedPythagorasBtn);
        GetButton((int)Buttons.GaussBtn).gameObject.BindEvent(OnClickedGaussBtn);
        GetButton((int)Buttons.EntranceBtn).gameObject.BindEvent(OnClickedEntranceBtn);
        GetButton((int)Buttons.DoorBtn).gameObject.BindEvent(OnClickedDoorBtn);

        for (int i = 0; i < System.Enum.GetValues(typeof(Buttons)).Length; i++)
        {
            GetButton(i).gameObject.BindEvent(CloseSide);
        }

        GetImage((int)Images.OpenedSide).gameObject.BindEvent(OnClickedSide);
        GetImage((int)Images.OpenedSide).gameObject.SetActive(!openSide);
        GetImage((int)Images.ClosedSide).gameObject.BindEvent(OnClickedSide);

        GetObject((int)GameObjects.EntranceOffice).SetActive(false);
        GetObject((int)GameObjects.SchoolHallway).SetActive(false);
        GetObject((int)GameObjects.Library).SetActive(false);
        GetButton((int)Buttons.LockedBtn).gameObject.SetActive(false);
        GetImage((int)Images.FogImage).gameObject.SetActive(false);
        GetImage((int)Images.SealCircleImage).gameObject.SetActive(false);
        GetImage((int)Images.FirstBrokeImg).gameObject.SetActive(false);
        GetImage((int)Images.SecondBrokeImg).gameObject.SetActive(false);
        GetImage((int)Images.ThirdBrokeImg).gameObject.SetActive(false);
        GetImage((int)Images.SpeechBubbleImg).gameObject.SetActive(false);
        GetImage((int)Images.SmallSpeechBubbleImg).gameObject.SetActive(false);
        GetImage((int)Images.GaussImage).gameObject.SetActive(false);

        // skip
        if (!Managers.UserMng.GetIsCompletedStory())
            GetButton((int)Buttons.SkipButton).gameObject.SetActive(false);

        // Sound
        Managers.Sound.Clear();
        Managers.Sound.Play("StoryBgm", Define.Sound.Bgm);

        return true;
    }
    void Skip()
    {
        Managers.Scene.ChangeScene(Define.Scene.LobbyScene);
    }

    void StartBtn()
    {
        OnClickNxtBtn();
        GetButton((int)Buttons.TmpNxtButton).gameObject.SetActive(false);
    }

    public void OnClickNxtBtn()
    {
        CloseSide();
        if (!Managers.TextEffect.isTypingEnd)
        {
            Managers.TextEffect.SetFastSpeed();
            return;
        }
        if (++count >= maxCount)
        {
            Managers.DBManager.SetIsCompletedStory(true);
            if (Managers.UserMng.isCompletedTutorial == true)
                Managers.Scene.ChangeScene(Define.Scene.StoryGameScene);
            else
                Managers.Scene.ChangeScene(Define.Scene.TutorialGameScene);
            return;
        }
        if (count == 7 || count == 11 || count == 33)
        {
            HideDialogue();
            GetObject((int)GameObjects.SchoolHallway).gameObject.SetActive(true);
            return;
        }
        if (count == 46)
        {
            HideDialogue();
            GetObject((int)GameObjects.Library).gameObject.SetActive(true);
            GetImage((int)Images.SmallSpeechBubbleImg).gameObject.SetActive(true);
            GetText((int)Texts.SmallTMP).text = I18n.Get(I18nDefine.STORY_HERE);
            return;
        }
        if (count == 52)
        {
            CoroutineHandler.StartCoroutine(ShowSpeechBubble(1.5f));
        }
        if (count == 60)
        {
            GetButton((int)Buttons.LockedBtn).gameObject.BindEvent(OnClickedLockedBtn);
            HideDialogue();
            GetObject((int)GameObjects.Library).gameObject.SetActive(true);
            return;
        }
        //PlayerPrefs.SetInt("WatchedStory", count);
        Managers.SceneEffect.SceneEffect(GetImage((int)Images.FadeImage), GetButton((int)Buttons.nxtButton), storyTalkData[count].sceneEffect);
        Managers.SceneEffect.ChangeBackground(GetImage((int)Images.BackGroundImage), storyTalkData[count].backgroundImg);
        Managers.SceneEffect.ChangeCharacterBG(GetImage((int)Images.CharacterBG), storyTalkData[count].characterName);
        if (storyTalkData[count].characterName == "가우스" || storyTalkData[count].characterName == "Gauss")
        {
            GetImage((int)Images.GaussImage).gameObject.SetActive(true);
            GetImage((int)Images.CharacterImage).gameObject.SetActive(false);
        }
        else
        {
            GetImage((int)Images.GaussImage).gameObject.SetActive(false);
            GetImage((int)Images.CharacterImage).gameObject.SetActive(true);
            Managers.SceneEffect.ChangeCharacter(GetImage((int)Images.PlayerImage), GetImage((int)Images.CharacterImage), storyTalkData[count].characterName, storyTalkData[count].expression);
        }

        if (storyTalkData[count].txtEffect == "MAX")
        {
            GetText((int)Texts.DialogueTMP).fontSize = 100;
        }
        else
        {
            GetText((int)Texts.DialogueTMP).fontSize = 80;
        }

        if (storyTalkData[count].characterName == "주인공" || storyTalkData[count].characterName == "Main character")
        {
            GetText((int)Texts.CharacterNameTMP).text = Managers.UserMng.GetNickname();
        }
        else
        {
            GetText((int)Texts.CharacterNameTMP).text = storyTalkData[count].characterName;
        }

        Managers.TextEffect.SetNormalSpeed();
        Managers.TextEffect.Typing(storyTalkData[count].dialogue, GetText((int)Texts.DialogueTMP));
        replayPopup.SetActive(true);
        replayPopup.GetComponent<UI_ReplayStory>().AddReplayStory(storyTalkData[count].characterName, storyTalkData[count].dialogue);
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

    void CloseSide()
    {
        GetImage((int)Images.OpenedSide).gameObject.SetActive(false);
        GetImage((int)Images.ClosedSide).gameObject.SetActive(true);
        openSide = false;
    }

    void OnClickedSide()
    {
        GetImage((int)Images.OpenedSide).gameObject.SetActive(openSide);
        GetImage((int)Images.ClosedSide).gameObject.SetActive(!openSide);
        openSide = !openSide;
    }

    void OnClickedNeumannBtn()
    {
        CoroutineHandler.StartCoroutine(ShowInfo(I18n.Get(I18nDefine.STORY_VON_NORMAN)));
    }

    void OnClickedStainedGlassBtn()
    {
        if (enteredOffice)
        {
            OnClickNxtBtn();
            ShowDialogue();
        }
        else
        {
            CoroutineHandler.StartCoroutine(ShowInfo(I18n.Get(I18nDefine.STORY_STAINED_GLASS)));
        }
    }

    void OnClickedEinsteinBtn()
    {
        CoroutineHandler.StartCoroutine(ShowInfo(I18n.Get(I18nDefine.STORY_EINSTEIN)));
    }

    void OnClickedNewtonBtn()
    {
        CoroutineHandler.StartCoroutine(ShowInfo(I18n.Get(I18nDefine.STORY_NEWTON)));
    }

    void OnClickedPythagorasBtn()
    {
        CoroutineHandler.StartCoroutine(ShowInfo(I18n.Get(I18nDefine.STORY_PYTHAGORAS)));
    }

    void OnClickedGaussBtn()
    {
        CoroutineHandler.StartCoroutine(ShowInfo(I18n.Get(I18nDefine.STORY_PYTHAGORAS)));
    }

    void OnClickedEntranceBtn()
    {
        if (!enteredOffice)
        {
            enteredOffice = true;
            GetImage((int)Images.LeftImage).sprite = Resources.Load("Sprites/Story/Background/school_hallway_black_left", typeof(Sprite)) as Sprite;
            GetImage((int)Images.RightImage).sprite = Resources.Load("Sprites/Story/Background/school_hallway_black_right", typeof(Sprite)) as Sprite;
            OnClickNxtBtn();
            ShowDialogue();
            GetObject((int)GameObjects.EntranceOffice).SetActive(enteredOffice);
        }
        else
        {
            CoroutineHandler.StartCoroutine(ShowInfo(I18n.Get(I18nDefine.STORY_DISCONNECT)));
        }
    }

    void OnClickedDoorBtn()
    {
        ShowDialogue();
        OnClickNxtBtn();
    }

    void OnClickedLockedBookBtn()
    {
        ShowDialogue();
        OnClickNxtBtn();
        GetImage((int)Images.SmallSpeechBubbleImg).gameObject.SetActive(false);
        GetObject((int)GameObjects.Library).gameObject.SetActive(true);
        GetButton((int)Buttons.LockedBookBtn).gameObject.SetActive(false);
        GetButton((int)Buttons.LockedBtn).gameObject.SetActive(true);
        GetImage((int)Images.FogImage).gameObject.SetActive(true);
    }

    void OnClickedLockedBtn()
    {
        unlockCnt++;

        switch (unlockCnt)
        {
            case 5:
                GetImage((int)Images.FirstBrokeImg).gameObject.SetActive(true);
                GetImage((int)Images.SealCircleImage).gameObject.SetActive(true);
                GetObject((int)GameObjects.Library).GetComponent<Image>().color = HexColor("#A2A2A2FF");
                break;
            case 10:
                GetImage((int)Images.SecondBrokeImg).gameObject.SetActive(true);
                GetImage((int)Images.SealCircleImage).sprite = Resources.Load("Sprites/Story/seal_circle2", typeof(Sprite)) as Sprite;
                GetObject((int)GameObjects.Library).GetComponent<Image>().color = HexColor("#686868FF");
                break;
            case 15:
                GetImage((int)Images.ThirdBrokeImg).gameObject.SetActive(true);
                GetImage((int)Images.SealCircleImage).sprite = Resources.Load("Sprites/Story/seal_circle3", typeof(Sprite)) as Sprite;
                GetObject((int)GameObjects.Library).GetComponent<Image>().color = HexColor("#000000FF");
                break;
            case 20:
                GetObject((int)GameObjects.Library).SetActive(false);
                GetImage((int)Images.CharacterImage).gameObject.SetActive(false);
                GetImage((int)Images.PlayerImage).gameObject.SetActive(false);
                CoroutineHandler.StartCoroutine(UnlockedAnimation(0.5f, "unlocked1"));
                CoroutineHandler.StartCoroutine(UnlockedAnimation(1.0f, "unlocked2"));
                CoroutineHandler.StartCoroutine(UnlockedAnimation(2.0f, "unlocked3"));
                break;
        }

    }

    IEnumerator ShowInfo(string dialgoue)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1.0f);
        GetObject((int)GameObjects.Panel).gameObject.SetActive(true);
        GetText((int)Texts.CharacterNameTMP).text = "";
        Managers.SceneEffect.ChangeCharacterBG(GetImage((int)Images.CharacterBG), "");
        GetText((int)Texts.DialogueTMP).text = dialgoue;
        yield return waitForSeconds;
        HideDialogue();
    }

    IEnumerator UnlockedAnimation(float time, string imgName)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(time);
        yield return waitForSeconds;
        GetImage((int)Images.BackGroundImage).sprite = Resources.Load("Sprites/Story/Background/" + imgName, typeof(Sprite)) as Sprite;
        if (imgName == "unlocked3")
        {
            ShowDialogue();
            GetImage((int)Images.CharacterImage).gameObject.SetActive(true);
            GetImage((int)Images.PlayerImage).gameObject.SetActive(true);
            OnClickNxtBtn();
        }
    }

    IEnumerator ShowSpeechBubble(float time)
    {
        HideDialogue();
        GetImage((int)Images.SpeechBubbleImg).gameObject.SetActive(true);
        GetText((int)Texts.SpeechTMP).text = I18n.Get(I18nDefine.STORY_WHO);
        WaitForSeconds waitForSeconds = new WaitForSeconds(time);
        yield return waitForSeconds;
        GetImage((int)Images.SpeechBubbleImg).gameObject.SetActive(false);
        ShowDialogue();
        GetObject((int)GameObjects.Library).gameObject.SetActive(true);
        OnClickNxtBtn();
    }

    void HideDialogue()
    {
        GetButton((int)Buttons.nxtButton).gameObject.SetActive(false);
        GetObject((int)GameObjects.Panel).gameObject.SetActive(false);
    }

    void ShowDialogue()
    {
        GetButton((int)Buttons.nxtButton).gameObject.SetActive(true);
        GetObject((int)GameObjects.Panel).gameObject.SetActive(true);
        GetObject((int)GameObjects.SchoolHallway).SetActive(false);
        GetObject((int)GameObjects.EntranceOffice).SetActive(false);
        GetObject((int)GameObjects.Library).gameObject.SetActive(false);
    }

    Color HexColor(string hexCode)
    {
        Color color;

        if (ColorUtility.TryParseHtmlString("#A2A2A2", out color))
        {
            return color;
        }
        return Color.white;
    }
}
