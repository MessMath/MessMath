using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Diagnosis : UI_Popup
{
    // TODO 
    // 진단평가 팝업 
    // 첫 스타트

    // 진단 평가의 순서
    // 난이도 선택 ex) 숫자를 안다. 곱셉을 할 줄 안다. 등등
    // 8문제로 진단 평가 진행.
    // 진단 평가 완료 후, 닫기 버튼 생성? -> 로비로 가기.
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
        // teacherImage가 있으면 실행
        // 다음 대사로 변경
        float waitTime = 2.0f;
        int textCount = 5;
        for (int i = 0; i < textCount; i++)
        {
            GetText((int)Texts.TeacherTalkText).text = Managers.GetText(Define.DiagnosisTeacherText + i);
            yield return new WaitForSeconds(waitTime);
            // TODO Sound 추가
        }

        // 화면 세팅
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
