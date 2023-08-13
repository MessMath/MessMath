using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Diagnosis : UI_Scene
{
    // TODO 
    // ������ �˾� 
    // ù ��ŸƮ

    // ���� ���� ����
    // ���̵� ���� ex) ���ڸ� �ȴ�. ������ �� �� �ȴ�. ���
    // 8������ ���� �� ����.
    // ���� �� �Ϸ� ��, �ݱ� ��ư ����? -> �κ�� ����.
    enum Buttons
    {
        ToLobbyBtn,
    }

    enum Texts
    {
        TeacherTalkText,
    }

    enum Images
    {
        Panel,
        Crystal,
        CrystalImage,
    }

    enum GameObjects
    {
        Sample,
        API,
        ChooseDifficulty,
        Problem,
        Debug,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindObject(typeof(GameObjects));

        StartCoroutine("NextTalk");

        //BindEvent(gameObject, MakeToLobbyBtn);
        GetButton((int)Buttons.ToLobbyBtn).gameObject.BindEvent(() => 
        {
            // Sound
            Managers.Sound.Play("ClickBtnEff");
            // TODO 예외처리
            if(PlayerPrefs.HasKey("WatchedStory") && PlayerPrefs.GetInt("WatchedStory") == -2)
                Managers.Scene.ChangeScene(Define.Scene.LobbyScene); 
            else
                Managers.Scene.ChangeScene(Define.Scene.StoryScene);
        });

        GetObject((int)GameObjects.Sample).gameObject.SetActive(false);
        GetObject((int)GameObjects.API).gameObject.SetActive(false);
        GetButton((int)Buttons.ToLobbyBtn).gameObject.SetActive(false);

        // Sound
        Managers.Sound.Clear();
        Managers.Sound.Play("DiagnosisBgm", Define.Sound.Bgm);

        return true;
    }

    IEnumerator NextTalk()
    {
        // teacherImage�� ������ ����
        // ���� ���� ����
        float waitTime = 2.0f;
        int textCount = 5;
        for (int i = 0; i < textCount; i++)
        {
            GetText((int)Texts.TeacherTalkText).text = Managers.GetText(Define.DiagnosisTeacherText + i);
            yield return new WaitForSeconds(waitTime);
            // TODO Sound �߰�
        }

        // ȭ�� ����
        GetObject((int)GameObjects.Sample).gameObject.SetActive(true);
        GetObject((int)GameObjects.API).gameObject.SetActive(true);
        GetObject((int)GameObjects.Problem).gameObject.SetActive(false);
        GetObject((int)GameObjects.Debug).gameObject.SetActive(false);
        GetText((int)Texts.TeacherTalkText).gameObject.SetActive(false);
        GetImage((int)Images.Crystal).gameObject.SetActive(false);
    }

    public void MakeToLobbyBtn()
    {
        if (Managers.Game.CurrentStatus == Define.CurrentStatus.LEARNING)
        {
            GetImage((int)Images.Crystal).gameObject.SetActive(false);
            GetImage((int)Images.CrystalImage).gameObject.SetActive(false);
            GetButton((int)Buttons.ToLobbyBtn).gameObject.SetActive(true);

            Managers.Sound.Play("합격통보음");
        }
    }
}
