using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PracticeGame : UI_Scene
{
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
    }

    enum GameObjects
    {
        Sample,
        ChooseDifficulty,
        Problem,
        API,
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
        anim.transform.SetParent(this.transform);
        anim.SetInfo(Define.Scene.PracticeGameScene, () => { });

        yield return new WaitForSeconds(0.3f);
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
        if (PlayerPrefs.HasKey("Coin"))
            GetText((int)Texts.CoinCount).text = PlayerPrefs.GetInt("Coin").ToString();
        else { PlayerPrefs.SetInt("Coin", 0); GetText((int)Texts.CoinCount).text = PlayerPrefs.GetInt("Coin").ToString(); }
        #endregion

        GetButton((int)Buttons.SettingBtn).gameObject.BindEvent(OnClickSettingBtn);
        GetButton((int)Buttons.AnswerBtn_1).gameObject.BindEvent(OnClickAnswerBtn);
        GetButton((int)Buttons.AnswerBtn_2).gameObject.BindEvent(OnClickAnswerBtn);
        GetButton((int)Buttons.AnswerBtn_3).gameObject.BindEvent(OnClickAnswerBtn);
        GetButton((int)Buttons.AnswerBtn_4).gameObject.BindEvent(OnClickAnswerBtn);

        GetObject((int)GameObjects.ChooseDifficulty).gameObject.SetActive(false);
        GetObject((int)GameObjects.Problem).gameObject.SetActive(false);
        GetImage((int)Images.TeacherTalkImage).gameObject.SetActive(false);

        GetButton((int)Buttons.Button_GetLearning).interactable = true;
        GetButton((int)Buttons.Button_GetLearning).gameObject.BindEvent(() => { GetButton((int)Buttons.Button_GetLearning).gameObject.SetActive(false); });

        // Sound
        Managers.Sound.Clear();
        Managers.Sound.Play("PracticeBgm");

        return true;
    }

    void OnClickSettingBtn()
    {
        // TODO UI_Setting
        Managers.UI.ShowPopupUI<UI_Setting>();

    }

    void OnClickAnswerBtn()
    {
        // Sound
        Managers.Sound.Play("ClickBtnEff");

        // 코인 수 연결. TODO 데베랑 연결해야 됨.
        if (PlayerPrefs.HasKey("Coin"))
            GetText((int)Texts.CoinCount).text = PlayerPrefs.GetInt("Coin").ToString();

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

        GetTeacherTalkText();

        GetImage((int)Images.TeacherTalkImage).gameObject.SetActive(true);
        if (Managers.Game.IsCorrect == true) // 정답
        {
            //GetImage((int)Images.TeacherImage).sprite = Managers.Resource.Load<Sprite>("Sprites/Practice/Professor_2"); //AnwerAni
            GetImage((int)Images.AnswerAni).gameObject.GetOrAddComponent<Animator>().SetTrigger("RightAnswerAniTrigger");
        }
        else // 오답
        {
            GetImage((int)Images.AnswerAni).gameObject.GetOrAddComponent<Animator>().SetTrigger("WrongAnswerAniTrigger");
        }

        yield return new WaitForSeconds(delayTime);
        //GetImage((int)Images.TeacherImage).sprite = Managers.Resource.Load<Sprite>("Sprites/Practice/Professor_1");
        GetImage((int)Images.TeacherTalkImage).gameObject.SetActive(false);

        yield return null;
    }

    void GetTeacherTalkText()
    {
        int randValue = Random.Range(0, 100);

        if (Managers.Game.IsCorrect == true)
        {
            if (randValue < 30) { GetText((int)Texts.TeacherTalkText).text = Managers.GetText(Define.RightAnswerTeacherTalkText + 0); }
            else if (randValue < 60) { GetText((int)Texts.TeacherTalkText).text = Managers.GetText(Define.RightAnswerTeacherTalkText + 1); }
            else if (randValue < 100) { GetText((int)Texts.TeacherTalkText).text = Managers.GetText(Define.RightAnswerTeacherTalkText + 2); }
            
        }
        else
        {
            if (randValue < 30) { GetText((int)Texts.TeacherTalkText).text = Managers.GetText(Define.WrongAnswerTeacherTalkText + 0); }
            else if (randValue < 60) { GetText((int)Texts.TeacherTalkText).text = Managers.GetText(Define.WrongAnswerTeacherTalkText + 1); }
            else if (randValue < 100) { GetText((int)Texts.TeacherTalkText).text = Managers.GetText(Define.WrongAnswerTeacherTalkText + 2); }
        }

    }

    // TODO API 코드도 다 넣어서 관리할까..??
    #region 웅진 API

    #endregion

}
