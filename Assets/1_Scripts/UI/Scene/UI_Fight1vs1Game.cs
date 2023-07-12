using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using WjChallenge;

public class UI_Fight1vs1Game : UI_Scene
{
    enum Texts
    {
    }

    enum Buttons
    {
        // 가호 버튼들
        SelectedGrace,
        SelectedGrace1,
        SelectedGrace2,
        SettingBtn,
    }

    enum Images
    {
        BGIMG,
        MathMtcImage,
        MathMtcHPImage_MASK,
        MathMtcHPBar,
        MathMtcHPImage,
        JoyStickHandle,
        Circle,
        heart0,
        heart1,
        heart2,
    }

    enum GameObjects
    {
        DeadLine,
        Wall1,
        Wall2,
        Wall3,
        Wall4,
        MathMtc,
        JoyStick,
        ArrowController,
        Player,
        WJ_Sample1vs1,
        Calculate_BoardtextCn,
        Calculate_BoardqstCn,
        TEXDrawPool,
    }

    public WJ_Sample1vs1 wj_sample1vs1;
    public bool GameStarted = false;
    public TEXDraw[] pool;
    TEXDraw[] TEXDrawPool;
    public WitchController witchController;

    public int QstMaxNum; // 최대 문제 갯수
    public int SolvedQstNum; // 지금까지 푼 문제의 갯수

    private void Awake()
    {
        Init();
        //StartCoroutine("SetArrowGenerationTime", 0.5f); // <-- BeforeFight1vs1팝업에서 대신.
    }

    private void Start()
    {
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));
        BindImage(typeof(Images));

        BindEvent(gameObject, OnPointerDown, Define.UIEvent.PointerDown);
        BindEvent(gameObject, OnPointerUp, Define.UIEvent.PointerUp);
        BindEvent(gameObject, OnDrag, Define.UIEvent.Pressed);

        SettingGraceBtn();

        GetButton((int)Buttons.SettingBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ShowPopupUI<UI_Setting>(); });

        for (int i = 0; i < 3; i++)
        {
            GameObject.Find($"Player/Circle/heart{i}").SetActive(true);
        }
        Managers.Game._idxOfHeart = 0;

        canvas = gameObject.GetComponent<Canvas>();
        canvasGroup = GetObject((int)GameObjects.JoyStick).GetComponent<RectTransform>().GetComponent<CanvasGroup>();
        TEXDrawPool = GetObject((int)GameObjects.TEXDrawPool).GetComponentsInChildren<TEXDraw>();
        wj_sample1vs1 = GetObject((int)GameObjects.WJ_Sample1vs1).GetComponent<WJ_Sample1vs1>();
        witchController = GetObject((int)GameObjects.MathMtc).GetOrAddComponent<WitchController>();

        #region 가호 버튼 설정

        //아인슈타인의 가호 테스트용
        //GetButton((int)Buttons.SelectedGrace).gameObject.BindEvent(() => Managers.Grace.CallGrace("GraceOfEinstein"));

        if (PlayerPrefs.GetString("SelectedGrace0InOneToOne") != "")
        {
            GetButton((int)Buttons.SelectedGrace).gameObject.BindEvent(() => Managers.Grace.CallGrace(PlayerPrefs.GetString("SelectedGrace0InOneToOne")));
            GetButton((int)Buttons.SelectedGrace).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Grace/" + PlayerPrefs.GetString("SelectedGrace0InOneToOne"));
        }

        if (PlayerPrefs.GetString("SelectedGrace1InOneToOne") != "")
        {
            GetButton((int)Buttons.SelectedGrace1).gameObject.BindEvent(() => Managers.Grace.CallGrace(PlayerPrefs.GetString("SelectedGrace1InOneToOne")));
            GetButton((int)Buttons.SelectedGrace1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Grace/" + PlayerPrefs.GetString("SelectedGrace1InOneToOne"));
        }

        if (PlayerPrefs.GetString("SelectedGrace2InOneToOne") != "")
        {
            GetButton((int)Buttons.SelectedGrace2).gameObject.BindEvent(() => Managers.Grace.CallGrace(PlayerPrefs.GetString("SelectedGrace2InOneToOne")));
            GetButton((int)Buttons.SelectedGrace2).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Grace/" + PlayerPrefs.GetString("SelectedGrace2InOneToOne"));
        }
        #endregion

        #region 수학자 세팅
        string imagePath = "Sprites/MathMtcInFight1vs1/";
        if (PlayerPrefs.GetString("Boss") == "Gauss")
            imagePath += "TempGauss";
        else if (PlayerPrefs.GetString("Boss") == "Pythagoras")
            imagePath += "TempPythagoras";
        else if (PlayerPrefs.GetString("Boss") == "Newton")
            imagePath += "TempNewton";
        GetImage((int)Images.MathMtcImage).gameObject.GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>(imagePath);

        // 풀어야하는 문항 수 지정
        QstMaxNum = PlayerPrefs.GetInt("QstLimit");
        SolvedQstNum = 0;
        #endregion

        #region 모드 설정
        Managers.Game.CurrentMode = Define.Mode.DoubleSolve;
        #endregion

        // 시작하기전에 팝업 등장!
        Managers.UI.ShowPopupUI<UI_BeforeFight1vs1Start>().UI_Fight1Vs1Game = this;

        return true;
    }

    private void Update()
    {
        if (!GameStarted) return;

        // 풀어야하는 문항수를 다 풀면
        if (SolvedQstNum >= QstMaxNum) { Managers.UI.ShowPopupUI<UI_GameWin>(); GameStarted = false; }
        if (wj_sample1vs1.currentQuestionIndex >= 8)
        {
            Managers.Connector.Learning_GetQuestion();
            return;
        }

    }

    void SettingGraceBtn()
    {
        for (int i = 0; i < 3; i++)
        {
            if (PlayerPrefs.GetString($"SelectedGrace{i}InOneToOne") == "")
                GetButton(i).gameObject.SetActive(false);
            else
                GetButton(i).gameObject.SetActive(true);
        }
    }

    void OnClickGraceBtn()
    {
        // Sound
        Managers.Sound.Play("ClickBtnEff");

        // TODO Ani

    }

    void RefreshUI()        // 문항이 보이는 영역 (보드) 새로고침
    {
        TEXDraw textCn = GetObject((int)GameObjects.Calculate_BoardtextCn).GetComponent<TEXDraw>();
        TEXDraw qstCn = GetObject((int)GameObjects.Calculate_BoardqstCn).GetComponent<TEXDraw>();
        int index = wj_sample1vs1.currentQuestionIndex;

        if (Managers.Connector.cLearnSet != null && index < 8)
        {
            textCn.text = Managers.Connector.cLearnSet.data.qsts[index].textCn;
            qstCn.text = Managers.Connector.cLearnSet.data.qsts[index].qstCn;
        }
        PoolUpdate();
    }

    void PoolUpdate()
    {
        if (Managers.Connector.cLearnSet == null || wj_sample1vs1.currentQuestionIndex >= 8)
            return;
        int cIndex = wj_sample1vs1.currentQuestionIndex;
        string correctAnswer = Managers.Connector.cLearnSet.data.qsts[cIndex].qstCransr;
        string[] wrongAnswers = Managers.Connector.cLearnSet.data.qsts[cIndex].qstWransr.Split(',');

        for (int i = 0; i < TEXDrawPool.Length; i++)
        {
            if (i == 0)
                TEXDrawPool[i].text = correctAnswer;
            else
                TEXDrawPool[i].text = wrongAnswers[i - 1];
        }
    }

    #region 수식 계산

    //public void Calculate()
    //{
    //    Debug.Log("Calculate");

    //    object result = null;
    //    string expressionToCalculate = GetObject((int)Texts.Calculate_BoardText).text.Replace("x", "*");
    //    //string expressionToCalculate = GetText((int)Texts.Calculate_BoardText).text.Replace("÷", "/");
    //    string printResult;

    //    GetObject((int)Texts.Calculate_BoardText).text = "";
    //    DataTable table = new DataTable();

    //    try
    //    {
    //        result = table.Compute(expressionToCalculate, "");
    //        printResult = Math.Truncate(Convert.ToDouble(result)).ToString();
    //        Debug.Log($"\"{expressionToCalculate}\" result is : " + printResult);
    //    }
    //    catch (System.Exception e)
    //    {
    //        Debug.Log($"\"{expressionToCalculate}\" is inappropriate expression! : {e}");
    //        printResult = "";
    //        damageToPlayer(1);
    //        return;
    //    }

    //    //if (GetText((int)Texts.PrintNumber_Text))
    //    //{
    //    //    GetText((int)Texts.PrintNumber_Text).text = $"={printResult}";
    //    //    StartCoroutine(Waitfor2Sec());
    //    //}

    //    if (printResult == "")
    //        damageToPlayer(1);
    //    else if (int.Parse(printResult) == GetObject((int)GameObjects.MathMtc).GetOrAddComponent<WitchController>().QusetionNumber)
    //        damageToWitch(15);
    //    else
    //        damageToPlayer(1);

    //}

    //IEnumerator Waitfor2Sec()
    //{
    //    yield return new WaitForSeconds(2.0f);
    //    Debug.Log("Wait2Sec");
    //    GetText((int)Texts.PrintNumber_Text).text = "";
    //}

    #endregion

    #region 데미지 주기

    public void damageToPlayer(int damage)
    {

        GetObject((int)GameObjects.Player).GetOrAddComponent<PlayerControllerCCF>()._hp -= damage;
        Debug.Log("player damage 1");

        if (GetObject((int)GameObjects.Player).GetOrAddComponent<PlayerControllerCCF>()._hp <= 0)
        {
            Managers.UI.ShowPopupUI<UI_GameOver>();
            return;
        }

        GameObject.Find($"Player/Circle/heart{Managers.Game._idxOfHeart}").SetActive(false);
        GetObject((int)GameObjects.Player).GetOrAddComponent<PlayerControllerCCF>().BlinkPlayerImg();
        if (Managers.Game._idxOfHeart > GetObject((int)GameObjects.Player).GetOrAddComponent<PlayerControllerCCF>()._hp)
            return;
        else
            Managers.Game._idxOfHeart++;
    }

    public void damageToWitch(float damage)
    {
        witchController.SetWitchHP(damage);

        if (witchController.Hp <= 0)
        {
            Managers.UI.ShowPopupUI<UI_GameWin>();
            return;
        }

        if ((QstMaxNum - SolvedQstNum) % 4 == 0)
            Managers.Debuff.CallDebuff(PlayerPrefs.GetString("Boss"));
    }

    #endregion

    #region 조이스틱

    private float deadZone = 0;
    private float hadndleRange = 0.8f;
    //private Vector3 input = Vector3.zero;
    private Canvas canvas;

    private CanvasGroup canvasGroup;

    public void OnPointerDown()
    {
        canvasGroup.alpha = 1f;
        GetObject((int)GameObjects.JoyStick).GetComponent<RectTransform>().position = UnityEngine.Input.mousePosition;
    }

    public void OnDrag()
    {
        Vector2 radius = GetObject((int)GameObjects.JoyStick).GetComponent<RectTransform>().sizeDelta / 2;
        Managers.Game._input = (UnityEngine.Input.mousePosition - (Vector3)GetObject((int)GameObjects.JoyStick).GetComponent<RectTransform>().position) / (radius * canvas.scaleFactor);

        HandleInput(Managers.Game._input.magnitude, Managers.Game._input.normalized);
        GetImage((int)Images.JoyStickHandle).gameObject.GetComponent<RectTransform>().anchoredPosition = Managers.Game._input * radius * hadndleRange / 3;
    }

    private void HandleInput(float magnitude, Vector2 normalised)
    {
        if (magnitude > deadZone)
        {
            if (magnitude > 1)
            {
                Managers.Game._input = normalised;
            }
        }
        else
        {
            Managers.Game._input = Vector2.zero;
        }
    }

    public void OnPointerUp()
    {
        Managers.Game._input = Vector2.zero;
        GetImage((int)Images.JoyStickHandle).gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        canvasGroup.alpha = 0f;
    }

    #endregion

    #region 화살 관리

    string[] Operator = { "+", "-", "x", "/" };

    //private const int MAX_NUM_ARROW = 3;
    //private const int MAX_SYMBOL_ARROW = 2;
    //private int numArrowCnt = 0;
    //private int symbolArrowCnt = 0;

    // 화살이 생성되는 시간 조절하는 함수 
    // 현재 화살 개수가 몇개 나왔는지 체크
    IEnumerator SetArrowGenerationTime(float delayTime)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(delayTime);

        yield return waitForSeconds;

        ShootArrow();

        StartCoroutine("SetArrowGenerationTime", 1f);
    }

    // 현재 플레이어의 위치를 향해 오브젝트 날리기 
    void ShootArrow()
    {
        GameObject arrowObj = MakeArrow();
        ArrowOnlyin1vs1 arrow = arrowObj.GetComponent<ArrowOnlyin1vs1>();
        arrowObj.GetComponent<Rigidbody2D>().AddForce(arrow.direction.normalized * arrow.speed, ForceMode2D.Impulse);

        Debug.Log("------Shoot Arrow------");
        Debug.Log($"Arrow type: {arrow.type} num or operator: {arrow.tmp} speed: {arrow.speed} \n startPosition:{arrow.startPosition.x} , {arrow.startPosition.y} \n direction: {arrow.direction}");
    }

    // 화살 동적 생성하는 함수
    GameObject MakeArrow()
    {
        //GameObject arrowObject = Instantiate(Managers.Resource.Load<GameObject>($"Prefabs/Arrow"), GetObject((int)GameObjects.ArrowController).transform);
        //GameObject arrowObject = Managers.Resource.Instantiate("Arrow", GetObject((int)GameObjects.ArrowController).transform);
        GameObject arrowObject = Managers.Resource.Instantiate("ArrowOnlyin1vs1", gameObject.transform.Find("ArrowController").transform);

        ArrowOnlyin1vs1 arrow = arrowObject.GetOrAddComponent<ArrowOnlyin1vs1>();
        SetArrow(arrow);

        arrowObject.GetOrAddComponent<RectTransform>().position = arrow.startPosition;
        return arrowObject;
    }

    // 화살 설정하는 함수
    void SetArrow(ArrowOnlyin1vs1 arrow)
    {
        //if (SetArrowType(arrow) == 0)
        //    SetArrowNum(arrow);
        //else
        //    SetArrowOperator(arrow);
        SetArrowValue(arrow);

        SetArrowStartPosition(arrow);
        SetArrowDirection(arrow);
        SetArrowSpeed(arrow);
    }

    // 화살이 들고있는 값을 설정하는 함수
    void SetArrowValue(ArrowOnlyin1vs1 arrow)
    {
        int randmax = TEXDrawPool.Length;
        int rand = UnityEngine.Random.Range(0, randmax);
        arrow.tmp = TEXDrawPool[rand];              // 화살의 텍스트로 무작위 답
        arrow.text = TEXDrawPool[rand].text;
        arrow.GetComponentInChildren<TEXDraw>().text = TEXDrawPool[rand].text;
    }

    // 화살의 생성 위치 조절하는 함수 
    void SetArrowStartPosition(ArrowOnlyin1vs1 arrow)
    {
        int randValue = UnityEngine.Random.Range(0, 3);
        switch (randValue)
        {
            case 0:
                arrow.startPosition = GetRandPosOfLeft();
                break;
            case 1:
                arrow.startPosition = GetRandPosOfUp();
                break;
            case 2:
                arrow.startPosition = GetRandPosOfRight();
                break;
        }
    }

    Vector2 GetRandPosOfLeft()
    {
        //Vector2 newPos = new Vector2
        //    (UnityEngine.Random.Range(GetObject((int)GameObjects.ArrowController).GetOrAddComponent<EdgeCollider2D>().points[0].x, 
        //    GetObject((int)GameObjects.ArrowController).GetOrAddComponent<EdgeCollider2D>().points[1].x), 
        //    UnityEngine.Random.Range(GetObject((int)GameObjects.ArrowController).GetOrAddComponent<EdgeCollider2D>().points[0].y, 
        //    GetObject((int)GameObjects.ArrowController).GetOrAddComponent<EdgeCollider2D>().points[1].y));
        Vector2 newPos = new Vector2(-100, UnityEngine.Random.Range(700, 1500));
        return newPos;
    }

    Vector2 GetRandPosOfUp()
    {
        //Vector2 newPos = new Vector2
        //    (UnityEngine.Random.Range(GetObject((int)GameObjects.ArrowController).GetOrAddComponent<EdgeCollider2D>().points[1].x,
        //    GetObject((int)GameObjects.ArrowController).GetOrAddComponent<EdgeCollider2D>().points[2].x),
        //    UnityEngine.Random.Range(GetObject((int)GameObjects.ArrowController).GetOrAddComponent<EdgeCollider2D>().points[1].y,
        //    GetObject((int)GameObjects.ArrowController).GetOrAddComponent<EdgeCollider2D>().points[2].y));
        Vector2 newPos = new Vector2(UnityEngine.Random.Range(-100, 3300), 1500);

        return newPos;
    }

    Vector2 GetRandPosOfRight()
    {
        //Vector2 newPos = new Vector2
        //    (UnityEngine.Random.Range(GetObject((int)GameObjects.ArrowController).GetOrAddComponent<EdgeCollider2D>().points[2].x,
        //    GetObject((int)GameObjects.ArrowController).GetOrAddComponent<EdgeCollider2D>().points[3].x),
        //    UnityEngine.Random.Range(GetObject((int)GameObjects.ArrowController).GetOrAddComponent<EdgeCollider2D>().points[2].y,
        //    GetObject((int)GameObjects.ArrowController).GetOrAddComponent<EdgeCollider2D>().points[3].y));
        Vector2 newPos = new Vector2(3300, UnityEngine.Random.Range(700, 1500));

        return newPos;
    }

    // 화살의 방향 조절하는 함수 
    // 현재 플레이어의 위치로 설정
    void SetArrowDirection(ArrowOnlyin1vs1 arrow)
    {
        //arrow.direction = GetObject((int)GameObjects.Player).transform.position - (Vector3)arrow.startPosition;
        arrow.direction = FindObjectOfType<PlayerControllerCCF>().transform.position - (Vector3)arrow.startPosition;
        LookAt(GetObject((int)GameObjects.Player), arrow);     // Player를 바라보고 날라가게끔

        arrow.GetComponentInChildren<TEXDraw>().gameObject.transform.localRotation = Quaternion.Euler(0, 0, arrow.transform.rotation.eulerAngles.z * (-1.0f));
    }

    void LookAt(GameObject target, ArrowOnlyin1vs1 arrow)
    {
        if (target != null)
        {
            Vector2 direction = arrow.direction;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion angleAxis = Quaternion.AngleAxis(angle + 180f, Vector3.forward);
            Quaternion rotation = Quaternion.Slerp(arrow.gameObject.GetComponentInChildren<Image>().transform.rotation, angleAxis, 1);

            arrow.transform.rotation = rotation;

            arrow.gameObject.GetComponentInChildren<Image>().transform.rotation = rotation;
        }
    }

    // 화살의 속도 조절하는 함수 
    void SetArrowSpeed(ArrowOnlyin1vs1 arrow)
    {
        arrow.speed = UnityEngine.Random.Range(200.0f, 250.0f);
    }

    #endregion

}