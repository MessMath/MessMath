using DiagnosisDatas;
using MessMathI18n;
using StoreDatas;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Diagnosis : UI_Scene
{
    JsonReader jsonReader;
    List<DiagnosisData> diagnosisData = new List<DiagnosisData>();

    enum Buttons
    {
        ToLobbyBtn,
    }

    enum Texts
    {
        TeacherTalkText,
        Text_ChooseDifficulty,
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
        Difficulty1,
        Difficulty2,
        Difficulty3,
        Difficulty4,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindObject(typeof(GameObjects));

        jsonReader = new JsonReader();

        if (LocalizationManager.Get().GetSelectedLanguage() == Language.KOREAN)
        {
            diagnosisData = jsonReader.ReadDiagnosisJson(Application.persistentDataPath + "/" + 4 + "_Diagnosis_KOR.json").diagnosisDataList;
            GetButton((int)Buttons.ToLobbyBtn).gameObject.GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>("Sprites/Diagnosis/AdmissionCertificate");
        }
        else
        {
            diagnosisData = jsonReader.ReadDiagnosisJson(Application.persistentDataPath + "/" + 10 + "_Diagnosis_EN.json").diagnosisDataList;
            GetButton((int)Buttons.ToLobbyBtn).gameObject.GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>("Sprites/Diagnosis/AdmissionCertificate_ENG");
        }


        SetToLobbyBtn();

        GetObject((int)GameObjects.Sample).gameObject.SetActive(false);
        GetObject((int)GameObjects.API).gameObject.SetActive(false);
        GetButton((int)Buttons.ToLobbyBtn).gameObject.SetActive(false);
        GetObject((int)GameObjects.Difficulty1).GetComponent<Text>().text = I18n.Get(I18nDefine.DIAGNOSIS_DIFFICULTY_1);
        GetObject((int)GameObjects.Difficulty2).GetComponent<Text>().text = I18n.Get(I18nDefine.DIAGNOSIS_DIFFICULTY_2);
        GetObject((int)GameObjects.Difficulty3).GetComponent<Text>().text = I18n.Get(I18nDefine.DIAGNOSIS_DIFFICULTY_3);
        GetObject((int)GameObjects.Difficulty4).GetComponent<Text>().text = I18n.Get(I18nDefine.DIAGNOSIS_DIFFICULTY_4);
        GetText((int)Texts.Text_ChooseDifficulty).text = I18n.Get(I18nDefine.DIAGNOSIS_CHOOSE_DIFFICULTY);

        StartCoroutine("NextTalk");

        // Sound
        Managers.Sound.Clear();
        Managers.Sound.Play("DiagnosisBgm", Define.Sound.Bgm);
        Managers.UI.ShowPopupUI<UI_GetNicknamePopup>();
        return true;
    }

    async void SetToLobbyBtn()
    {
        bool isCompletedStory = await Managers.DBManager.GetIsCompletedStory(Managers.GoogleSignIn.GetUID());
        GetButton((int) Buttons.ToLobbyBtn).gameObject.BindEvent(() => 
        {
            // Sound
            Managers.Sound.Play("ClickBtnEff");
            Managers.DBManager.SetIsCompletedDiagnosis(true);
            Managers.DBManager.SetMyClothes("uniform");
            Managers.DBManager.SetObtainedClothes("uniform");
            // TODO 예외처리
            if (isCompletedStory)
                Managers.Scene.ChangeScene(Define.Scene.LobbyScene); 
            else
                Managers.Scene.ChangeScene(Define.Scene.StoryScene);
        });
    }

    IEnumerator NextTalk()
    {
        // teacherImage�� ������ ����
        // ���� ���� ����
        float waitTime = 2.0f;
        int textCount = 5;
        for (int i = 0; i < textCount; i++)
        {
            //GetText((int)Texts.TeacherTalkText).text = Managers.GetText(Define.DiagnosisTeacherText + i);
            GetText((int)Texts.TeacherTalkText).text = diagnosisData[i].dialogue;
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
