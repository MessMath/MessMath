using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WjChallenge;
using TexDrawLib;

// public enum CurrentStatus { WAITING, DIAGNOSIS, LEARNING }
public class WJ_Sample : MonoBehaviour
{
    //[SerializeField] WJ_Connector wj_conn;
    // [SerializeField] Managers.Game.CurrentStatus      currentStatus;
    // public Managers.Game.CurrentStatus CurrentStatus => currentStatus;

    [Header("Panels")]
    [SerializeField] GameObject panel_diag_chooseDiff;  //���̵� ���� �г�
    [SerializeField] GameObject panel_question;         //���� �г�(����,�н�)

    [SerializeField] Text textDescription;              //���� ���� �ؽ�Ʈ
    [SerializeField] TEXDraw textEquation;              //���� �ؽ�Ʈ(��TextDraw�� ���� �ʿ�)
    [SerializeField] Button[] btAnsr = new Button[4];   //���� ��ư��
    TEXDraw[] textAnsr;                                 //���� ��ư�� �ؽ�Ʈ(��TextDraw�� ���� �ʿ�)

    [Header("Status")]
    int currentQuestionIndex;
    bool isSolvingQuestion;
    float questionSolveTime;

    [Header("For Debug")]
    [SerializeField] WJ_DisplayText wj_displayText;     //�ؽ�Ʈ ǥ�ÿ�(�ʼ�X)
    [SerializeField] Button getLearningButton;          //���� �޾ƿ��� ��ư

    private void Awake()
    {
        textAnsr = new TEXDraw[btAnsr.Length];

        for (int i = 0; i < btAnsr.Length; ++i)

            textAnsr[i] = btAnsr[i].GetComponentInChildren<TEXDraw>();

        wj_displayText.SetState("�����", "", "", "");
    }

    private void OnEnable()
    {
        Setup();
    }

    private void Setup()
    {
        switch (Managers.Game.CurrentStatus)
        {
            case Define.CurrentStatus.WAITING:
                panel_diag_chooseDiff.SetActive(true);
                break;
        }

        if (Managers.Connector != null)
        {
            Debug.Log("wj_connector!");
            if (Managers.Game.CurrentStatus == Define.CurrentStatus.WAITING)
                Managers.Connector.onGetDiagnosis.AddListener(() => GetDiagnosis());
            if (Managers.Game.CurrentStatus == Define.CurrentStatus.LEARNING)
                Managers.Connector.onGetLearning.AddListener(() => GetLearning(0));
        }
        else Debug.LogError("Cannot find Connector");
    }

    private void Update()
    {
        if (isSolvingQuestion) questionSolveTime += Time.deltaTime;
    }

    /// <summary>
    /// ������ ���� �޾ƿ���
    /// </summary>
    private void GetDiagnosis()
    {
        switch (Managers.Connector.cDiagnotics.data.prgsCd)
        {
            case "W":
                MakeQuestion(Managers.Connector.cDiagnotics.data.textCn,
                            Managers.Connector.cDiagnotics.data.qstCn,
                            Managers.Connector.cDiagnotics.data.qstCransr,
                            Managers.Connector.cDiagnotics.data.qstWransr);
                wj_displayText.SetState("������ ��", "", "", "");
                break;
            case "E":
                Debug.Log("������ ��! �н� �ܰ�� �Ѿ�ϴ�.");
                wj_displayText.SetState("������ �Ϸ�", "", "", "");
                Managers.Game.CurrentStatus = Define.CurrentStatus.LEARNING;
                getLearningButton.interactable = true;
                break;
        }
    }

    /// <summary>
    /// 
    ///  n ��° �н� ���� �޾ƿ���
    /// </summary>
    private void GetLearning(int _index)
    {
        if (_index == 0) currentQuestionIndex = 0;

        MakeQuestion(Managers.Connector.cLearnSet.data.qsts[_index].textCn,
                    Managers.Connector.cLearnSet.data.qsts[_index].qstCn,
                    Managers.Connector.cLearnSet.data.qsts[_index].qstCransr,
                    Managers.Connector.cLearnSet.data.qsts[_index].qstWransr);
    }

    /// <summary>
    /// �޾ƿ� �����͸� ������ ������ ǥ��
    /// </summary>
    private void MakeQuestion(string textCn, string qstCn, string qstCransr, string qstWransr)
    {
        panel_diag_chooseDiff.SetActive(false);
        panel_question.SetActive(true);

        string correctAnswer;
        string[] wrongAnswers;

        Debug.Log(textCn);
        textDescription.text = textCn;
        textEquation.text = qstCn;

        correctAnswer = qstCransr;
        wrongAnswers = qstWransr.Split(',');

        int ansrCount = Mathf.Clamp(wrongAnswers.Length, 0, 3) + 1;

        for (int i = 0; i < btAnsr.Length; i++)
        {
            if (i < ansrCount)
                btAnsr[i].gameObject.SetActive(true);
            else
                btAnsr[i].gameObject.SetActive(false);
        }

        int ansrIndex = Random.Range(0, ansrCount);

        for (int i = 0, q = 0; i < ansrCount; ++i, ++q)
        {
            if (i == ansrIndex)
            {
                textAnsr[i].text = correctAnswer;
                --q;
            }
            else
                textAnsr[i].text = wrongAnswers[q];
        }
        isSolvingQuestion = true;
    }

    /// <summary>
    /// ���� ���� �¾Ҵ� �� üũ
    /// </summary>
    public void SelectAnswer(int _idx)
    {
        bool isCorrect;
        string ansrCwYn = "N";

        switch (Managers.Game.CurrentStatus)
        {
            case Define.CurrentStatus.DIAGNOSIS:
                isCorrect = textAnsr[_idx].text.CompareTo(Managers.Connector.cDiagnotics.data.qstCransr) == 0 ? true : false;
                ansrCwYn = isCorrect ? "Y" : "N";

                isSolvingQuestion = false;

                Managers.Connector.Diagnosis_SelectAnswer(textAnsr[_idx].text, ansrCwYn, (int)(questionSolveTime * 1000));

                wj_displayText.SetState("������ ��", textAnsr[_idx].text, ansrCwYn, questionSolveTime + " ��");

                panel_question.SetActive(false);
                questionSolveTime = 0;
                break;

            case Define.CurrentStatus.LEARNING:
                isCorrect = textAnsr[_idx].text.CompareTo(Managers.Connector.cLearnSet.data.qsts[currentQuestionIndex].qstCransr) == 0 ? true : false;
                ansrCwYn = isCorrect ? "Y" : "N";

                // �׽�Ʈ�� �߰��� �κ�
                if (isCorrect)
                {
                    Debug.Log("����");
                    Managers.Game.Coin++;
                    Managers.Game.IsCorrect = true;
                    // TODO? CoinAnim Add?
                }
                else
                {
                    Debug.Log("����");
                    Managers.Game.IsCorrect = false;

                }

                isSolvingQuestion = false;
                currentQuestionIndex++;

                Managers.Connector.Learning_SelectAnswer(currentQuestionIndex, textAnsr[_idx].text, ansrCwYn, (int)(questionSolveTime * 1000));

                wj_displayText.SetState("����Ǯ�� ��", textAnsr[_idx].text, ansrCwYn, questionSolveTime + " ��");

                if (currentQuestionIndex >= 8)
                {
                    panel_question.SetActive(false);
                    wj_displayText.SetState("����Ǯ�� �Ϸ�", "", "", "");
                }
                else GetLearning(currentQuestionIndex);

                questionSolveTime = 0;
                break;
        }

    }

    public void DisplayCurrentState(string state, string myAnswer, string isCorrect, string svTime)
    {
        if (wj_displayText == null) return;

        wj_displayText.SetState(state, myAnswer, isCorrect, svTime);
    }

    #region Unity ButtonEvent
    public void ButtonEvent_ChooseDifficulty(int a)
    {
        Managers.Game.CurrentStatus = Define.CurrentStatus.DIAGNOSIS;
        Managers.Connector.FirstRun_Diagnosis(a);
    }
    public void ButtonEvent_GetLearning()
    {
        Managers.Connector.Learning_GetQuestion();
        wj_displayText.SetState("����Ǯ�� ��", "-", "-", "-");
    }
    #endregion
}
