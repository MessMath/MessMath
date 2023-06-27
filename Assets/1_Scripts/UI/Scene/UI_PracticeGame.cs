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
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));
        BindImage(typeof(Images));

        // TODO GameManager에서 플레이어 정보로 Coin개수 가져오기
        GetText((int)Texts.CoinCount).text = Managers.Game.Coin.ToString();
        
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
        GetText((int)Texts.CoinCount).text = Managers.Game.Coin.ToString();

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
        if (Managers.Game.IsCorrect == true) GetImage((int)Images.TeacherImage).sprite = Managers.Resource.Load<Sprite>("Sprites/testTeacher");
        else GetImage((int)Images.TeacherImage).sprite = Managers.Resource.Load<Sprite>("test");

        yield return new WaitForSeconds(delayTime);
        GetImage((int)Images.TeacherImage).sprite = Managers.Resource.Load<Sprite>("Sprites/TeacherImage");
        GetImage((int)Images.TeacherTalkImage).gameObject.SetActive(false);

        yield return null;
    }

    void GetTeacherTalkText()
    {
        int randValue = Random.Range(0, 100);

        if (Managers.Game.IsCorrect == true)
        {
            if (randValue < 30) { GetText((int)Texts.TeacherTalkText).text = "Good Job!!"; }
            else if (randValue < 60) { GetText((int)Texts.TeacherTalkText).text = "Oh!!"; }
            else if (randValue < 100) { GetText((int)Texts.TeacherTalkText).text = "Yes!!"; }
            
        }
        else
        {
            if (randValue < 30) { GetText((int)Texts.TeacherTalkText).text = "Use your head"; }
            else if (randValue < 60) { GetText((int)Texts.TeacherTalkText).text = "Not Kidding"; }
            else if (randValue < 100) { GetText((int)Texts.TeacherTalkText).text = "Hmm..."; }
        }

    }

    // TODO? API 코드도 다 넣어서 관리할까..??
    #region 웅진 API

    #endregion

}
