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
        // �ž��ǵ� API ���� ������ �ҵ�
        GetText((int)Texts.QuestionText).text = "3 + 3 =";
        GetText((int)Texts.LeftAnswerText).text = "6";
        GetText((int)Texts.RightAnswerText).text = "9";

        // TODO GameManager���� �÷��̾� ������ Coin���� ��������
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
        // ���� Ȯ��
        // ����� -> ���ΰ��� ����, ���� ǥ�� �� �� �� ����
        // ����� -> ������ ǥ�� �� �� �� ����

        Debug.Log("����!");
        int presentCoinCount = int.Parse(GetText((int)Texts.CoinCount).text);
        GetText((int)Texts.CoinCount).text = (presentCoinCount + 1).ToString();

        // �ϴ��� Ʋ���� �� �����Ÿ���
        StartCoroutine("BlinkTeacherImg", 0.1f);
    }

    void OnClickRightAnswerBtn()
    {
        Debug.Log("����...");

        // �ϴ��� Ʋ���� �� �����Ÿ���
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
