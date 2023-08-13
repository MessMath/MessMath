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
    bool openSide = true;
    bool enteredOffice = false;

    enum GameObjects
    {
        Panel,
        SidePanel,
        SchoolHallway,
        EntranceOffice,
        Library
    }
    enum Images
    {
        BackGroundImage,
        PlayerImage,
        CharacterImage,
        FadeImage,
        OpenedSide,
        ClosedSide,
        CharacterBG,
        LeftImage,
        RightImage,
    }
    enum Buttons
    {
        nxtButton,
        SettingButton,
        ReplayButton,
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
        storyTalkData = jsonReader.ReadStoryJson(Application.persistentDataPath + "/" + 0 + "_EnterGameStory.json").talkDataList;
        maxCount = storyTalkData.Count;
        replayPopup = Managers.UI.ShowPopupUI<UI_ReplayStory>().gameObject;
        replayPopup.SetActive(false);

        GetText((int)Texts.CharacterNameTMP).text = "";
        GetText((int)Texts.DialogueTMP).text = "";
        GetButton((int)Buttons.TmpNxtButton).gameObject.BindEvent(StartBtn);
        GetButton((int)Buttons.nxtButton).gameObject.BindEvent(OnClickNxtBtn);
        GetButton((int)Buttons.ReplayButton).gameObject.BindEvent(OnClickReplayBtn);

        GetButton((int)Buttons.NeumannBtn).gameObject.BindEvent(OnClickedNeumannBtn);
        GetButton((int)Buttons.StainedGlassBtn).gameObject.BindEvent(OnClickedStainedGlassBtn);
        GetButton((int)Buttons.EinsteinBtn).gameObject.BindEvent(OnClickedEinsteinBtn);
        GetButton((int)Buttons.NewtonBtn).gameObject.BindEvent(OnClickedNewtonBtn);
        GetButton((int)Buttons.PythagorasBtn).gameObject.BindEvent(OnClickedPythagorasBtn);
        GetButton((int)Buttons.GaussBtn).gameObject.BindEvent(OnClickedGaussBtn);
        GetButton((int)Buttons.EntranceBtn).gameObject.BindEvent(OnClickedEntranceBtn);
        GetButton((int)Buttons.DoorBtn).gameObject.BindEvent(OnClickedDoorBtn);
        GetButton((int)Buttons.LockedBookBtn).gameObject.BindEvent(OnClickedLockedBookBtn);

        GetImage((int)Images.OpenedSide).gameObject.BindEvent(OnClickedSide);
        GetImage((int)Images.OpenedSide).gameObject.SetActive(!openSide);
        GetImage((int)Images.ClosedSide).gameObject.BindEvent(OnClickedSide);

        /*GetButton((int)Buttons.nxtButton).gameObject.SetActive(false);
        GetImage((int)Images.FadeImage).gameObject.SetActive(false);
        GetObject((int)GameObjects.Panel).SetActive(false);
        GetObject((int)GameObjects.SidePanel).SetActive(false);*/
        GetObject((int)GameObjects.EntranceOffice).SetActive(false);
        GetObject((int)GameObjects.SchoolHallway).SetActive(false);
        GetObject((int)GameObjects.Library).SetActive(false);


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
        /*GetButton((int)Buttons.TmpNxtButton).gameObject.SetActive(false);
        GetText((int)Texts.CharacterNameTMP).text = "주인공";
        Managers.SceneEffect.ChangeCharacterBG(GetImage((int)Images.CharacterBG), "주인공");
        CoroutineHandler.StartCoroutine(ShowInfo("수학 성적이 떨어졌다고 교장실로 오라니... 그나저나 교장실이 이 근처였는데 어디였지...?"));
        
        GetText((int)Texts.CharacterNameTMP).text = "";
        Managers.SceneEffect.ChangeCharacterBG(GetImage((int)Images.CharacterBG), "");
        CoroutineHandler.StartCoroutine(ShowInfo("화면을 드래그 해 교장실 입구를 찾아보자."));*/
    }

    public void OnClickNxtBtn()
    {
        if(!Managers.TextEffect.isTypingEnd)
        {
            Managers.TextEffect.SetFastSpeed();
            return;
        }
        if(++count >= maxCount) 
        {
            PlayerPrefs.SetInt("WatchedStory", -2);
            if (PlayerPrefs.GetInt("DoTutorial") == 2)
                Managers.Scene.ChangeScene(Define.Scene.StoryGameScene);
            else
                Managers.Scene.ChangeScene(Define.Scene.TutorialGameScene);
            return;
        }
        if(count == 7 || count == 11 || count == 32)
        {
            HideDialogue();
            GetObject((int)GameObjects.SchoolHallway).gameObject.SetActive(true);
            return;
        }
        if(count == 46 || count == 59)
        {
            HideDialogue();
            GetObject((int)GameObjects.Library).gameObject.SetActive(true);
            return;
        }
        PlayerPrefs.SetInt("WatchedStory", count);
        Managers.SceneEffect.SceneEffect(GetImage((int)Images.FadeImage),GetButton((int)Buttons.nxtButton), storyTalkData[count].sceneEffect);
        Managers.SceneEffect.ChangeBackground(GetImage((int)Images.BackGroundImage), storyTalkData[count].backgroundImg);
        Managers.SceneEffect.ChangeCharacterBG(GetImage((int)Images.CharacterBG), storyTalkData[count].characterName);
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

    void OnClickedSide()
    {
        GetImage((int)Images.OpenedSide).gameObject.SetActive(openSide);
        GetImage((int)Images.ClosedSide).gameObject.SetActive(!openSide);
        openSide = !openSide;
    }

    void OnClickedNeumannBtn()
    {
        CoroutineHandler.StartCoroutine(ShowInfo("폰 노인만의 방. 지금은 들어갈 수 없습니다."));
    }

    void OnClickedStainedGlassBtn()
    {
        if(enteredOffice) 
        {
            OnClickNxtBtn();
            ShowDialogue();
        }
        else 
        {
            CoroutineHandler.StartCoroutine(ShowInfo("아름다운 장식의 스테인드 글라스입니다."));
        }
    }

    void OnClickedEinsteinBtn()
    {
        CoroutineHandler.StartCoroutine(ShowInfo("아인슈타인의 방. 지금은 들어갈 수 없습니다."));
    }

    void OnClickedNewtonBtn()
    {
        CoroutineHandler.StartCoroutine(ShowInfo("뉴턴의 방. 지금은 들어갈 수 없습니다."));
    }

    void OnClickedPythagorasBtn()
    {
        CoroutineHandler.StartCoroutine(ShowInfo("피타고라스의 방. 지금은 들어갈 수 없습니다."));
    }

    void OnClickedGaussBtn()
    {
        CoroutineHandler.StartCoroutine(ShowInfo("가우스의 방. 지금은 들어갈 수 없습니다."));
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
            /*GetText((int)Texts.CharacterNameTMP).text = "주인공";
            Managers.SceneEffect.ChangeCharacterBG(GetImage((int)Images.CharacterBG), "주인공");
            CoroutineHandler.StartCoroutine(ShowInfo("맞아. 여기였어!"));
            */
        }
        else
        {
            CoroutineHandler.StartCoroutine(ShowInfo("아까까진 교장실 입구로 이어졌지만 지금은 어디에도 연결되어 있지 않습니다."));
        }
    }

    void OnClickedDoorBtn()
    {       
        ShowDialogue();
        //GetImage((int)Images.FadeImage).gameObject.SetActive(true);
        //GetObject((int)GameObjects.SidePanel).SetActive(true);
        OnClickNxtBtn();
    }

    void OnClickedLockedBookBtn()
    {
        ShowDialogue();
        OnClickNxtBtn();
        GetButton((int)Buttons.LockedBookBtn).gameObject.SetActive(false);
    }

    IEnumerator ShowInfo(string dialgoue)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1.0f);
        //GetButton((int)Buttons.DoorBtn).gameObject.SetActive(false);
        ShowDialogue();
        GetText((int)Texts.CharacterNameTMP).text = "";
        Managers.SceneEffect.ChangeCharacterBG(GetImage((int)Images.CharacterBG), "");
        GetText((int)Texts.DialogueTMP).text = dialgoue;
        yield return waitForSeconds;
        //GetObject((int)GameObjects.Panel).SetActive(false);
        //GetButton((int)Buttons.DoorBtn).gameObject.SetActive(true);
        HideDialogue();
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
}
