using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Windows;
using System.Data;
using System;
using TMPro;
using System.IO;
using Random = UnityEngine.Random;
using Unity.VisualScripting;

public class UI_StoryGame : UI_Scene
{
    enum Texts
    {
        Calculate_BoardText,
        PrintNumber_Text,
        QuestionNumber_Text,
        PreCalculation_Text,
        // 테스트용
        PhaseText,
    }

    enum Buttons
    {
        // 가호 버튼들
        SelectedGrace,
        SelectedGrace1,
        SelectedGrace2,
        AllErase,
        EqualButton,
        SettingBtn,
    }

    enum Images
    {
        BGIMG,
        WitchImage,
        WitchHPImage_MASK,
        WitchHPBar,
        WitchHPImage,
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
        Witch,
        JoyStick,
        ArrowController,
        Player,
        JoyStickPanel,
    }

    private void Awake()
    {
        Init();

        //StartCoroutine("SetGame");
        //edgeCollider = GetComponent<EdgeCollider2D>();

        StartCoroutine("SetArrowGenerationTime", 0.5f);
    }

    private void Start()
    {

    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));
        BindImage(typeof(Images));

        GetObject((int)GameObjects.JoyStickPanel).BindEvent(OnPointerDown, Define.UIEvent.PointerDown);
        GetObject((int)GameObjects.JoyStickPanel).BindEvent(OnPointerUp, Define.UIEvent.PointerUp);
        GetObject((int)GameObjects.JoyStickPanel).BindEvent(OnDrag, Define.UIEvent.Pressed);

        SettingGraceBtn();

        GetText((int)Texts.PrintNumber_Text).text = "";
        GetText((int)Texts.Calculate_BoardText).text = "";
        GetButton((int)Buttons.EqualButton).gameObject.BindEvent(Calculate);
        GetButton((int)Buttons.SettingBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ShowPopupUI<UI_Setting>(); });

        for (int i = 0; i < 3; i++)
        {
            GameObject.Find($"Player/Circle/heart{i}").SetActive(true);
        }
        Managers.Game._idxOfHeart = 0;

        canvas = gameObject.GetComponent<Canvas>();
        //outLine = transform.Find("Env").Find("JoyStick").GetComponent<RectTransform>();
        //handle = transform.Find("Env").Find("JoyStick").Find("Handle").GetComponent<RectTransform>(); ;
        canvasGroup = GetObject((int)GameObjects.JoyStick).GetComponent<RectTransform>().GetComponent<CanvasGroup>();

        // Sound
        Managers.Sound.Clear();
        Managers.Sound.Play("BattleBgm", Define.Sound.Bgm);

        // default damage is 15
        Managers.Game.Damage = 15;

        #region 가호 버튼 설정

        //GetButton((int)Buttons.SelectedGrace).gameObject.BindEvent(() => Managers.Grace.CallGrace("GraceOfNeumann"));

        if (PlayerPrefs.GetString("SelectedGrace0InStory") != "")
        {
            GetButton((int)Buttons.SelectedGrace).gameObject.BindEvent(() => Managers.Grace.CallGrace(PlayerPrefs.GetString("SelectedGrace0InStory")));
            GetButton((int)Buttons.SelectedGrace).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Grace/" + PlayerPrefs.GetString("SelectedGrace0InStory"));
        }

        if (PlayerPrefs.GetString("SelectedGrace1InStory") != "")
        {
            GetButton((int)Buttons.SelectedGrace1).gameObject.BindEvent(() => Managers.Grace.CallGrace(PlayerPrefs.GetString("SelectedGrace1InStory")));
            GetButton((int)Buttons.SelectedGrace1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Grace/" + PlayerPrefs.GetString("SelectedGrace1InStory"));
        }

        if (PlayerPrefs.GetString("SelectedGrace2InStory") != "")
        {
            GetButton((int)Buttons.SelectedGrace2).gameObject.BindEvent(() => Managers.Grace.CallGrace(PlayerPrefs.GetString("SelectedGrace2InStory")));
            GetButton((int)Buttons.SelectedGrace2).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Grace/" + PlayerPrefs.GetString("SelectedGrace2InStory"));
        }
        #endregion

        // 지우개 버튼
        GetButton((int)Buttons.AllErase).gameObject.BindEvent(() => AllErase());

        UnityEngine.Input.multiTouchEnabled = true;

        GetText((int)Texts.PhaseText).text = currentPhase.ToString();

        return true;
    }

    void SettingGraceBtn()
    {
        for (int i = 0; i < 3; i++)
        {
            if (PlayerPrefs.GetString($"SelectedGrace{i}InStory") == "")
                GetButton(i).gameObject.SetActive(false);
            else
                GetButton(i).gameObject.SetActive(true);
        }
    }

    #region 수식 계산

    void AllErase()
    {
        GetText((int)Texts.Calculate_BoardText).text = "";
        PreCalculate();
    }

    public void PreCalculate()
    {
        Debug.Log("PreCalculate");

        object result = null;
        string expressionToCalculate = GetText((int)Texts.Calculate_BoardText).text.Replace("×", "*").Replace("÷", "/");
        string printResult;

        DataTable table = new DataTable();

        if (expressionToCalculate == "")
        {
            GetText((int)Texts.PreCalculation_Text).text = "";
            return;
        }

        try
        {
            result = table.Compute(expressionToCalculate, "");
            printResult = Math.Truncate(Convert.ToDouble(result)).ToString();
            //Debug.Log($"\"{expressionToCalculate}\" result is : " + printResult);
        }
        catch (System.Exception)
        {
            //Debug.Log($"\"{expressionToCalculate}\" is inappropriate expression! : {e}");
            printResult = "";
            //damageToPlayer(1);
            return;
        }

        if (GetText((int)Texts.PreCalculation_Text))
        {
            GetText((int)Texts.PreCalculation_Text).text = $"={printResult}";
            //StartCoroutine(Waitfor2Sec());
        }
    }

    public void Calculate()
    {
        // Sound
        Managers.Sound.Play("ClickBtnEff");

        object result = null;
        string expressionToCalculate = GetText((int)Texts.Calculate_BoardText).text.Replace("×", "*").Replace("÷", "/");
        string printResult;

        GetText((int)Texts.Calculate_BoardText).text = "";
        GetText((int)Texts.PreCalculation_Text).text = "";
        DataTable table = new DataTable();

        try
        {
            result = table.Compute(expressionToCalculate, "");
            printResult = Math.Truncate(Convert.ToDouble(result)).ToString();
            //Debug.Log($"\"{expressionToCalculate}\" result is : " + printResult);
        }
        catch (System.Exception)
        {
            //Debug.Log($"\"{expressionToCalculate}\" is inappropriate expression! : {e}");
            printResult = "";
            damageToPlayer(1);
            return;
        }

        if (GetText((int)Texts.PrintNumber_Text))
        {
            GetText((int)Texts.PrintNumber_Text).text = $"={printResult}";
            StartCoroutine(Waitfor2Sec());
        }

        if (printResult == "")
            damageToPlayer(1);
        else if (int.Parse(printResult) == GetObject((int)GameObjects.Witch).GetOrAddComponent<WitchController>().QuestionNumber)
        {
            damageToWitch(Managers.Game.Damage);
        }
        else
            damageToPlayer(1);

    }

    IEnumerator Waitfor2Sec()
    {
        yield return new WaitForSeconds(2.0f);
        Debug.Log("Wait2Sec");
        GetText((int)Texts.PrintNumber_Text).text = "";
    }

    #endregion

    #region 데미지 주기

    public void damageToPlayer(int damage)
    {
        // Sound
        Managers.Sound.Play("AttackEff");

        GetObject((int)GameObjects.Player).GetOrAddComponent<PlayerController>()._hp -= damage;
        Debug.Log("player damage 1");

        if (GetObject((int)GameObjects.Player).GetOrAddComponent<PlayerController>()._hp <= 0)
        {
            Managers.UI.ShowPopupUI<UI_GameOver>();
            return;
        }

        GetObject((int)GameObjects.Witch).GetOrAddComponent<WitchController>().Questioning();
        GameObject.Find($"Player/Circle/heart{Managers.Game._idxOfHeart}").SetActive(false);
        GetObject((int)GameObjects.Player).GetOrAddComponent<PlayerController>().BlinkPlayerImg();
        if (Managers.Game._idxOfHeart > GetObject((int)GameObjects.Player).GetOrAddComponent<PlayerController>()._hp)
            return;
        else
            Managers.Game._idxOfHeart++;
    }

    public void damageToWitch(int damage)
    {
        // Sound
        Managers.Sound.Play("AttackEff");

        WitchController witchController = GetObject((int)GameObjects.Witch).GetOrAddComponent<WitchController>();
        witchController.SetWitchHP(damage);
        witchController.Questioning();
        if (witchController.Hp <= 0)
        {
            if (currentPhase == Phase.Phase1)
                ChangePhase(Phase.Phase2);
            else if (currentPhase == Phase.Phase2)
                ChangePhase(Phase.Phase3);
            else
                StartCoroutine(FinishMode());
        }

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

    string[] Operator = { "+", "-", "×", "÷" };

    // 페이즈 관리를 위한 열거형과 해당 타입 변수.
    public enum Phase
    {
        Phase1,
        Phase2,
        Phase3,
    }
    public Phase currentPhase = Phase.Phase1;

    // TODO
    // Phase에 따라 변하는 여러변수들을 배열형태로 선언.
    // (int)currentPhase로 인덱싱을 해보자.

    private int[] numberMin = { 1, 1, 10 };             // 등장 숫자 최소값
    private int[] numberMax = { 10, 100, 100 };         // 등장 숫자 최대값
    private float[] delayTime = { 1f, 0.8f, 0.6f };      // 화살 발사 딜레이
    private float[] speedMin = { 200f, 280f, 360f };     // 화살 속도 최소값
    private float[] speedMax = { 250f, 330f, 410f };     // 화살 속도 최대값

    private int MAX_NUM_ARROW = 3;
    private int MAX_SYMBOL_ARROW = 2;
    private int numArrowCnt = 0;
    private int symbolArrowCnt = 0;

    // 화살이 생성되는 시간 조절하는 함수 
    // 현재 화살 개수가 몇개 나왔는지 체크
    IEnumerator SetArrowGenerationTime(float delaytime)
    {
        float minDelaytime = delayTime[(int)currentPhase] * 0.5f;
        WaitForSeconds waitForSeconds = new WaitForSeconds(delaytime);

        ShootArrow();

        yield return waitForSeconds;
        StartCoroutine("SetArrowGenerationTime", Random.Range(minDelaytime, delayTime[(int)currentPhase]));
    }

    // 현재 플레이어의 위치를 향해 오브젝트 날리기 
    void ShootArrow()
    {
        GameObject arrowObj = MakeArrow();
        Arrow arrow = arrowObj.GetComponent<Arrow>();
        arrowObj.GetComponent<Rigidbody2D>().AddForce(arrow.direction.normalized * arrow.speed, ForceMode2D.Impulse);

        // Sound
        Managers.Sound.Play("ArrowEff");

        Debug.Log("------Shoot Arrow------");
        Debug.Log($"Arrow type: {arrow.type} num or operator: {arrow.tmp} speed: {arrow.speed} \n startPosition:{arrow.startPosition.x} , {arrow.startPosition.y} \n direction: {arrow.direction}");
    }

    // 화살 동적 생성하는 함수
    GameObject MakeArrow()
    {
        //GameObject arrowObject = Instantiate(Managers.Resource.Load<GameObject>($"Prefabs/Arrow"), GetObject((int)GameObjects.ArrowController).transform);
        //GameObject arrowObject = Managers.Resource.Instantiate("Arrow", GetObject((int)GameObjects.ArrowController).transform);
        GameObject arrowObject = Managers.Resource.Instantiate("Arrow", gameObject.transform.Find("ArrowController").transform);

        Arrow arrow = arrowObject.GetOrAddComponent<Arrow>();
        SetArrow(arrow);
        arrowObject.GetOrAddComponent<RectTransform>().position = arrow.startPosition;
        return arrowObject;
    }

    // 화살 설정하는 함수
    void SetArrow(Arrow arrow)
    {
        if (SetArrowType(arrow) == 0)
            SetArrowNum(arrow);
        else
            SetArrowOperator(arrow);
        SetArrowStartPosition(arrow);
        SetArrowDirection(arrow);
        SetArrowSpeed(arrow);
    }

    // 현재 생성된 화살의 타입 숫자인지 기호인지 설정하는 함수 
    int SetArrowType(Arrow arrow)
    {
        arrow.type = Random.Range(0, 2);

        if (arrow.type == 0)
        {
            numArrowCnt++;
            if (MAX_NUM_ARROW < numArrowCnt)
            {
                arrow.type = 1;
                numArrowCnt = 0;
            }
        }
        else if (arrow.type == 1)
        {
            symbolArrowCnt++;
            if (MAX_SYMBOL_ARROW < symbolArrowCnt)
            {
                arrow.type = 0;
                symbolArrowCnt = 0;
            }
        }
        return arrow.type;
    }

    void SetArrowNum(Arrow arrow)
    {
        arrow.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = Random.Range(numberMin[(int)currentPhase], numberMax[(int)currentPhase]).ToString();
    }

    void SetArrowOperator(Arrow arrow)
    {
        arrow.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = Operator[Random.Range(0, 4)];
    }

    // 화살의 생성 위치 조절하는 함수 
    void SetArrowStartPosition(Arrow arrow)
    {
        int randValue = Random.Range(0, 3);
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
        Vector2 newPos = new Vector2(-100, Random.Range(700, 1500));

        return newPos;
    }
    Vector2 GetRandPosOfUp()
    {
        Vector2 newPos = new Vector2(Random.Range(-100, 3300), 1500);

        return newPos;
    }
    Vector2 GetRandPosOfRight()
    {
        Vector2 newPos = new Vector2(3300, Random.Range(700, 1500));

        return newPos;
    }

    // 화살의 방향 조절하는 함수 
    // 현재 플레이어의 위치로 설정
    void SetArrowDirection(Arrow arrow)
    {
        //arrow.direction = GetObject((int)GameObjects.Player).transform.position - (Vector3)arrow.startPosition;
        arrow.direction = FindObjectOfType<PlayerController>().transform.position - (Vector3)arrow.startPosition;
        LookAt(GetObject((int)GameObjects.Player), arrow);     // Player를 바라보고 날라가게끔

        arrow.GetComponentInChildren<TextMeshProUGUI>().gameObject.transform.localRotation = Quaternion.Euler(0, 0, arrow.transform.rotation.eulerAngles.z * (-1.0f));
    }

    void LookAt(GameObject target, Arrow arrow)
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

    float referenceWidth = 3200f; // 기준 해상도의 너비
    float referenceHeight = 1440f; // 기준 해상도의 높이
    float currentWidth = Screen.width; // 현재 화면의 너비
    float currentHeight = Screen.height; // 현재 화면의 높이

    // 화살의 속도 조절하는 함수 
    void SetArrowSpeed(Arrow arrow)
    {
        float widthRatio = currentWidth / referenceWidth;
        float heightRatio = currentHeight / referenceHeight;

        arrow.speed = Random.Range(speedMin[(int)currentPhase], speedMax[(int)currentPhase]) * Mathf.Min(widthRatio, heightRatio);
    }

    #endregion

    #region 페이즈 관리

    /// <summary>
    /// 페이즈 변경
    /// </summary>
    /// <param name="phase">변경하고자 하는 페이즈</param>
    public void ChangePhase(Phase phase)
    {
        WitchController witchController = GetObject((int)GameObjects.Witch).GetOrAddComponent<WitchController>();
        witchController.Hp = 100;
        witchController.HpBar.fillAmount = 1f;      // 마녀 풀피 회복
        currentPhase = phase;

        // 디버그용
        GetText((int)Texts.PhaseText).text = currentPhase.ToString();

        // 2페이즈의 특수효과 시작, 간격은 일단 10초
        if (phase == Phase.Phase2)
        {
            // TODO 2페이즈 마녀 이미지 변환 및 애니메이션
            GetImage((int)Images.WitchImage).sprite = Managers.Resource.Load<Sprite>("Sprites/Character/witch/Phase2");
            GetImage((int)Images.BGIMG).sprite = Managers.Resource.Load<Sprite>("Sprites/background/BattlePhase2");
            StartCoroutine(SpecialEffects(5f));
        }

        if (phase == Phase.Phase3)
        {
            // TODO 3페이즈 마녀 이미지 변환 및 애니메이션
            GetImage((int)Images.WitchImage).sprite = Managers.Resource.Load<Sprite>("Sprites/Character/witch/Phase3");
            GetImage((int)Images.BGIMG).sprite = Managers.Resource.Load<Sprite>("Sprites/background/BattlePhase3");

            StopAllCoroutines();
            StartCoroutine(SetArrowGenerationTime(delayTime[(int)currentPhase]));

            StartCoroutine(SpecialEffectsForPhase3(1.0f));
        }

    }

    #region 2페이즈 특수 효과

    float _phase2Skill1Delay = 10f;
    IEnumerator SpecialEffects(float delay)
    {
        yield return new WaitForSeconds(delay);
        float minDelay = delay * 0.5f;

        StartCoroutine(MiddleDirectionOfArrow());

        StartCoroutine(SpecialEffects(Random.Range(minDelay, _phase2Skill1Delay)));
    }

    IEnumerator MiddleDirectionOfArrow()
    {
        StopCoroutine("SetArrowGenerationTime");
        yield return new WaitForSeconds(1.0f);

        Managers.Sound.Play("ArrowEff");

        for (int i = 0; i < 9; i++)
        {
            GameObject arrowObject = Managers.Resource.Instantiate("Arrow", gameObject.transform.Find("ArrowController").transform);

            Arrow arrow = arrowObject.GetOrAddComponent<Arrow>();

            if (SetArrowType(arrow) == 0)
                SetArrowNum(arrow);
            else
                SetArrowOperator(arrow);

            // 고정된 곳에서 화살 날라오기
            if (i == 0) arrow.startPosition = new Vector2(-100, 100);
            else if (i == 1) arrow.startPosition = new Vector2(3300, 100);
            else if (i == 2) arrow.startPosition = new Vector2(800, 1500);
            else if (i == 3) arrow.startPosition = new Vector2(-100, 700);
            else if (i == 4) arrow.startPosition = new Vector2(3300, 700);
            else if (i == 5) arrow.startPosition = new Vector2(1600, 1500);
            else if (i == 6) arrow.startPosition = new Vector2(-100, 1500);
            else if (i == 7) arrow.startPosition = new Vector2(3300, 1500);
            else if (i == 8) arrow.startPosition = new Vector2(2400, 1500);

            float x = Screen.width / 2;
            float y = Screen.height / 2;
            Vector3 middle = new(x, y, 0.0f);
            arrow.direction = middle - (Vector3)arrow.startPosition;
            LookAt(GetImage((int)Images.BGIMG).gameObject, arrow);

            arrow.GetComponentInChildren<TextMeshProUGUI>().gameObject.transform.localRotation = Quaternion.Euler(0, 0, arrow.transform.rotation.eulerAngles.z * (-1.0f));

            float widthRatio = currentWidth / referenceWidth;
            float heightRatio = currentHeight / referenceHeight;
            arrow.speed = speedMax[(int)currentPhase] * Mathf.Min(widthRatio, heightRatio);

            arrowObject.GetOrAddComponent<RectTransform>().position = arrow.startPosition;

            arrowObject.GetComponent<Rigidbody2D>().AddForce(arrow.direction.normalized * arrow.speed, ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(1.0f);
        StartCoroutine(SetArrowGenerationTime(delayTime[(int)currentPhase]));
    }

    #endregion

    #region 3페이즈 특수 효과

    float _phase3Skill1Delay = 10f;

    IEnumerator SpecialEffectsForPhase3(float delay)
    {
        yield return new WaitForSeconds(delay);
        float minDelay = delay * 0.5f;

        ChangeDirectionOfArrows();

        StartCoroutine(SpecialEffectsForPhase3(Random.Range(minDelay, _phase3Skill1Delay)));
    }

    public void ChangeDirectionOfArrows()
    {
        GameObject[] arrows = GameObject.FindGameObjectsWithTag("Arrow");

        foreach (GameObject arrow in arrows)
        {
            Vector2 curVec = new Vector2(arrow.GetComponent<Arrow>().direction.x, arrow.GetComponent<Arrow>().direction.y);
            Vector2 RandomVector2 = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            float speed = arrow.GetComponent<Arrow>().speed;
            arrow.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            arrow.GetComponent<Rigidbody2D>().AddForce(RandomVector2.normalized * speed, ForceMode2D.Impulse);
            
            float angle = Mathf.Atan2(curVec.y, curVec.x) * Mathf.Rad2Deg;
            float angle2 = Mathf.Atan2(RandomVector2.y, RandomVector2.x) * Mathf.Rad2Deg;
            
            Quaternion angleAxis = Quaternion.AngleAxis(angle2 + angle, Vector3.forward);
            //angleAxis = Quaternion.AngleAxis(angle + angle2 + 180, Vector3.forward);
            Quaternion rotation = Quaternion.Slerp(arrow.gameObject.GetComponentInChildren<Image>().transform.rotation, angleAxis, 1);

            arrow.transform.rotation = rotation;

            arrow.gameObject.GetComponentInChildren<Image>().transform.rotation = rotation;
            arrow.GetComponentInChildren<TextMeshProUGUI>().gameObject.transform.localRotation = Quaternion.Euler(0, 0, arrow.transform.rotation.eulerAngles.z * (-1.0f));

        }
    }
    #endregion

    #endregion

    #region 에필로그

    // 페이즈3에서 마지막에 마녀를 무찌를 때 호출되는 함수.
    IEnumerator FinishMode()
    {
        Time.timeScale = 0.3f;
        StartCoroutine(SetPitchLow());
        yield return new WaitForSecondsRealtime(3f);

        Time.timeScale = 1f;
        // TODO
        // 1. 팝업 없애고
        // 2. 화면 페이드 아웃 넣고
        // 3. 이후 에필로그 보여주는 Scene으로...
        Managers.UI.ShowPopupUI<UI_GameWin>();
    }

    IEnumerator SetPitchLow()
    {
        AudioSource[] AS = GameObject.Find("@Sound").transform.GetComponentsInChildren<AudioSource>();
        foreach (AudioSource a in AS)
        {
            a.pitch *= 0.9f;
        }
        yield return new WaitForSecondsRealtime(2.8f);
        foreach (AudioSource a in AS)
        {
            a.pitch = 1f;
        }
    }
    #endregion
}