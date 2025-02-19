using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MessMathI18n;

public class UI_PracticeGame : UI_Scene
{
    List<string> obtainedCollections = new List<string>();
    enum Texts
    {
        CoinCount,
        TeacherTalkText,
    }

    enum Buttons
    {
        SettingBtn,
        AnswerBtn_1,
        AnswerBtn_2,
        AnswerBtn_3,
        AnswerBtn_4,
        Button_GetLearning,
    }

    enum Images
    {
        BG,
        CoinImage,
        TeacherImage,
        TeacherTalkImage,
        AnswerAni,
        HolyRing,
        DarkRing,
        Pencil,
    }

    enum GameObjects
    {
        Sample,
        ChooseDifficulty,
        Problem,
        API,
        StartText,
    }

    private void Start()
    {
        Init();

        CoroutineHandler.StartCoroutine(SceneChangeAnimation_Out_PracticeGame());
    }

    #region 씬변환 애니

    IEnumerator SceneChangeAnimation_Out_PracticeGame()
    {
        // Ani
        UI_LockTouch uI_LockTouch = Managers.UI.ShowPopupUI<UI_LockTouch>();
        SceneChangeAnimation_Out anim = Managers.Resource.Instantiate("Animation/SceneChangeAnimation_Out").GetOrAddComponent<SceneChangeAnimation_Out>();
        anim.transform.SetParent(uI_LockTouch.transform);
        anim.SetInfo(Define.Scene.PracticeGameScene, () => { });

        yield return new WaitForSeconds(0.5f);
        Managers.UI.ClosePopupUI(uI_LockTouch);
    }
    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));
        BindImage(typeof(Images));
        
        #region Set Coin
        SetCoinTxt();
        #endregion

        GetButton((int)Buttons.SettingBtn).gameObject.BindEvent(OnClickSettingBtn);
        GetButton((int)Buttons.AnswerBtn_1).gameObject.BindEvent(OnClickAnswerBtn);
        GetButton((int)Buttons.AnswerBtn_2).gameObject.BindEvent(OnClickAnswerBtn);
        GetButton((int)Buttons.AnswerBtn_3).gameObject.BindEvent(OnClickAnswerBtn);
        GetButton((int)Buttons.AnswerBtn_4).gameObject.BindEvent(OnClickAnswerBtn);
        GetObject((int)GameObjects.StartText).gameObject.GetComponent<Text>().text = I18n.Get(I18nDefine.PRACTICE_START);

        GetObject((int)GameObjects.ChooseDifficulty).gameObject.SetActive(false);
        GetObject((int)GameObjects.Problem).gameObject.SetActive(false);
        GetImage((int)Images.TeacherTalkImage).gameObject.SetActive(false);
        GetImage((int)Images.HolyRing).gameObject.SetActive(false);
        GetImage((int)Images.DarkRing).gameObject.SetActive(false);
        GetImage((int)Images.Pencil).gameObject.SetActive(false);

        GetButton((int)Buttons.Button_GetLearning).interactable = true;
        GetButton((int)Buttons.Button_GetLearning).gameObject.BindEvent(() => { GetButton((int)Buttons.Button_GetLearning).gameObject.SetActive(false); });

        InitObtainedCollections();

        // Sound
        Managers.Sound.Clear();
        Managers.Sound.Play("PracticeBgm", Define.Sound.Bgm);

        return true;
    }
    async void InitObtainedCollections()
    {
        obtainedCollections = Managers.DBManager.ParseObtanined(await Managers.DBManager.GetObtainedCollections(Managers.GoogleSignIn.GetUID()));

        for (int i = 0; i < obtainedCollections.Count; i++)
        {
            if (obtainedCollections[i] == "holy_ring") GetImage((int)Images.HolyRing).gameObject.SetActive(true);
            if (obtainedCollections[i] == "dark_ring") GetImage((int)Images.DarkRing).gameObject.SetActive(true);
            if (obtainedCollections[i] == "smarty_pencil") GetImage((int)Images.Pencil).gameObject.SetActive(true);
        }
    }

    async void SetCoinTxt()
    {
        int coin = await Managers.DBManager.GetCoin(Managers.GoogleSignIn.GetUID());
        GetText((int)Texts.CoinCount).text = coin.ToString();
    }

    void OnClickSettingBtn()
    {
        // TODO UI_Setting
        Managers.UI.ShowPopupUI<UI_CheckToLobby>();
    }

    void OnClickAnswerBtn()
    {
        // Sound
        Managers.Sound.Play("ClickBtnEff");

        SetCoinTxt();

        if (Managers.Game.IsCorrect == true && Managers.Game.CurrentStatus == Define.CurrentStatus.LEARNING) // 오답일 경우?
        {
            StartCoroutine("SetTeacher");
        }
        else if (Managers.Game.IsCorrect == false && Managers.Game.CurrentStatus == Define.CurrentStatus.LEARNING)
        {
            StartCoroutine("SetTeacher");

        }
    }

    IEnumerator SetTeacher()
    {
        float delayTime = 1.0f;
        int randValue = Random.Range(0, 2);
        GetTeacherTalkText();

        GetImage((int)Images.TeacherTalkImage).gameObject.SetActive(true);
        if (Managers.Game.IsCorrect == true) // 정답
        {
            if (randValue == 0) GetImage((int)Images.TeacherImage).sprite = Managers.Resource.Load<Sprite>("Sprites/Character/teacher/Teacher_Right1");
            else if (randValue == 1) GetImage((int)Images.TeacherImage).sprite = Managers.Resource.Load<Sprite>("Sprites/Character/teacher/Teacher_Right2");
            GetImage((int)Images.AnswerAni).gameObject.GetOrAddComponent<Animator>().SetTrigger("RightAnswerAniTrigger");
        }
        else // 오답
        {
            GetImage((int)Images.TeacherImage).sprite = Managers.Resource.Load<Sprite>("Sprites/Character/teacher/Teacher_Wrong");
            GetImage((int)Images.AnswerAni).gameObject.GetOrAddComponent<Animator>().SetTrigger("WrongAnswerAniTrigger");
        }

        yield return new WaitForSeconds(delayTime);
        GetImage((int)Images.TeacherImage).sprite = Managers.Resource.Load<Sprite>("Sprites/Character/teacher/Teacher");
        GetImage((int)Images.TeacherTalkImage).gameObject.SetActive(false);

        yield return null;
    }

    void GetTeacherTalkText()
    {
        int randValue = Random.Range(0, 100);

        if (Managers.Game.IsCorrect == true)
        {
            if (randValue < 30) { GetText((int)Texts.TeacherTalkText).text = I18n.Get(I18nDefine.PRACTICE_TEACHER_RIGHT_ANSWER_REACTION_1); }
            else if (randValue < 60) { GetText((int)Texts.TeacherTalkText).text = I18n.Get(I18nDefine.PRACTICE_TEACHER_RIGHT_ANSWER_REACTION_2); }
            else if (randValue < 100) { GetText((int)Texts.TeacherTalkText).text = I18n.Get(I18nDefine.PRACTICE_TEACHER_RIGHT_ANSWER_REACTION_3); }

        }
        else
        {
            if (randValue < 30) { GetText((int)Texts.TeacherTalkText).text = I18n.Get(I18nDefine.PRACTICE_TEACHER_WRONG_ANSWER_REACTION_1); }
            else if (randValue < 60) { GetText((int)Texts.TeacherTalkText).text = I18n.Get(I18nDefine.PRACTICE_TEACHER_WRONG_ANSWER_REACTION_2); }
            else if (randValue < 100) { GetText((int)Texts.TeacherTalkText).text = I18n.Get(I18nDefine.PRACTICE_TEACHER_WRONG_ANSWER_REACTION_3); }
        }

    }

    // TODO API 코드도 다 넣어서 관리할까..??
    #region 웅진 API

    #endregion

}
