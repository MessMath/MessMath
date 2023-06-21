using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Diagnosis : UI_Popup
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
        TeacherImage,
        TeacherTalkImage,
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

        BindEvent(gameObject, MakeToLobbyBtn);
        GetButton((int)Buttons.ToLobbyBtn).gameObject.BindEvent(() => { ClosePopupUI(); });

        GetObject((int)GameObjects.Sample).gameObject.SetActive(false);
        GetObject((int)GameObjects.API).gameObject.SetActive(false);
        GetButton((int)Buttons.ToLobbyBtn).gameObject.SetActive(false);

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
        GetImage((int)Images.TeacherImage).gameObject.SetActive(false);
    }

    void MakeToLobbyBtn()
    {
        if (Managers.Game.CurrentStatus == Define.CurrentStatus.LEARNING)
        {
            GetButton((int)Buttons.ToLobbyBtn).gameObject.SetActive(true);
        }

    }
}
