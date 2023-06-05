using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_PracticeGame : UI_Scene
{
    enum Texts
    {
        QuestionText,
        LeftAnswerText,
        RightAnswerText,
        CoinCount,
    }

    enum Buttons
    {
        SettingBtn,
        LeftAnswerBtn,
        RightAnswerBtn,
    }

    enum Images
    {
        BG,
        CoinImage,
        TeacherImage,
    }

    enum GameObjects
    {

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

        // TODO
        // 매쓰피드 API 보고 만들어야 할듯
        GetText((int)Texts.QuestionText).text = "3 + 3 =";
        GetText((int)Texts.LeftAnswerText).text = "6";
        GetText((int)Texts.RightAnswerText).text = "9";

        // TODO GameManager에서 플레이어 정보로 Coin개수 가져오기
        GetText((int)Texts.CoinCount).text = "0";
        
        GetButton((int)Buttons.SettingBtn).gameObject.BindEvent(OnClickSettingBtn);
        GetButton((int)Buttons.LeftAnswerBtn).gameObject.BindEvent(OnClickLeftAnswerBtn);
        GetButton((int)Buttons.RightAnswerBtn).gameObject.BindEvent(OnClickRightAnswerBtn);

        

        return true;
    }

    void OnClickSettingBtn()
    {
        // TODO UI_Setting
        // Managers.UI.ShowPopupUI<UI_Setting>();

        Managers.Scene.ChangeScene(Define.Scene.LobbyScene);
    }

    void OnClickLeftAnswerBtn()
    {
        // TODO
        // 정답 확인
        // 정답시 -> 코인개수 증가, 선생 표정 몇 초 간 변경
        // 오답시 -> 선생의 표정 몇 초 간 변경

        Debug.Log("정답!");
        int presentCoinCount = int.Parse(GetText((int)Texts.CoinCount).text);
        GetText((int)Texts.CoinCount).text = (presentCoinCount + 1).ToString();

        // 일단은 틀렸을 때 깜빡거리기
        StartCoroutine("BlinkTeacherImg", 0.1f);
    }

    void OnClickRightAnswerBtn()
    {
        Debug.Log("오답...");

        // 일단은 틀렸을 때 깜빡거리기
        StartCoroutine("BlinkTeacherImg", 0.1f);
    }

    IEnumerator BlinkTeacherImg(float delayTime)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.15f);
        int countTime = 0;
        while (countTime < 10)
        {
            if (countTime % 2 == 0)
                GetImage((int)Images.TeacherImage).color = new Color32(255, 255, 255, 90);
            else
                GetImage((int)Images.TeacherImage).color = new Color32(255, 255, 255, 180);

            yield return waitForSeconds;

            countTime++;
        }

        GetImage((int)Images.TeacherImage).color = Color.white;
        yield return null;
    }
}
