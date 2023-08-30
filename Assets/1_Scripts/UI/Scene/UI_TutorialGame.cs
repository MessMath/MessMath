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

public class UI_TutorialGame : UI_Scene
{
    WitchController witchController;
    enum Texts
    {
        Calculate_BoardText,
        PrintNumber_Text,
        QuestionNumber_Text,
        PreCalculation_Text,
    }

    enum Buttons
    {
        AllErase,
        EqualButton,
        // 가호 버튼들
        SelectedGrace,
        SelectedGrace1,
        SelectedGrace2,
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
    }

    int turn;

    private void Awake()
    {
        Init();

        //StartCoroutine("SetGame");
        //edgeCollider = GetComponent<EdgeCollider2D>();

        StartCoroutine("SetArrowGenerationTime", 0.5f);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));
        BindImage(typeof(Images));

        BindEvent(gameObject, OnPointerDown, Define.UIEvent.PointerDown);
        BindEvent(gameObject, OnPointerUp, Define.UIEvent.PointerUp);
        BindEvent(gameObject, OnDrag, Define.UIEvent.Pressed);

        GetText((int)Texts.PrintNumber_Text).text = "";
        GetText((int)Texts.Calculate_BoardText).text = "";
        GetButton((int)Buttons.EqualButton).gameObject.BindEvent(Calculate);
        witchController = GetObject((int)GameObjects.Witch).GetComponent<WitchController>();

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
        Managers.Game.Damage = 45;

        #region 가호 버튼 설정

        GetButton((int)Buttons.SelectedGrace).gameObject.BindEvent(() => Managers.Grace.CallGrace("GraceOfGauss", GetButton((int)Buttons.SelectedGrace).gameObject));
        GetButton((int)Buttons.SelectedGrace).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Grace/" + "GraceOfGauss");

        #endregion        
        GetButton((int)Buttons.AllErase).gameObject.BindEvent(() => AllErase());

        // 지우개 버튼
        GetText((int)Texts.QuestionNumber_Text).text = "8";

        PlayerPrefs.SetInt("DoTutorial", 2);
        ShowTutorialPopup();
        return true;
    }
    void ShowTutorialPopup()
    {
        Managers.UI.ShowPopupUI<UI_TutorialPopup>();
    }

    #region 수식 계산

    void AllErase()
    {
        GetText((int)Texts.Calculate_BoardText).text = "";
        PreCalculate();
    }

    public void PreCalculate()
    {
        //Debug.Log("PreCalculate");

        object result = null;
        string expressionToCalculate = GetText((int)Texts.Calculate_BoardText).text.Replace("×", "*").Replace("÷", "/");
        string printResult;

        if(expressionToCalculate.Trim() == "")
        {
            GetText((int)Texts.PreCalculation_Text).text = "";
            return;
        }

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
            Debug.Log($"\"{expressionToCalculate}\" result is : " + printResult);
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
        Debug.Log("Calculate");

        // Sound
        Managers.Sound.Play("ClickBtnEff");

        object result = null;
        string expressionToCalculate = GetText((int)Texts.Calculate_BoardText).text.Replace("x", "*");
        //string expressionToCalculate = GetText((int)Texts.Calculate_BoardText).text.Replace("��", "/");
        string printResult;

        GetText((int)Texts.Calculate_BoardText).text = "";
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
            //damageToPlayer(1);
            return;
        }

        if (GetText((int)Texts.PrintNumber_Text))
        {
            GetText((int)Texts.PrintNumber_Text).text = $"={printResult}";
            StartCoroutine(Waitfor2Sec());
        }

        if (turn == 0 && int.Parse(printResult) == 8)
        {
            damageToWitch(Managers.Game.Damage);
            GetText((int)Texts.QuestionNumber_Text).text = "6";
            turn++;
        }
        else if (turn == 1 && int.Parse(printResult) == 6)
        {
            damageToWitch(Managers.Game.Damage);
            GetText((int)Texts.QuestionNumber_Text).text = "7";
            turn++;
        }
        else if (turn == 2 && int.Parse(printResult) == 7)
        {
            damageToWitch(Managers.Game.Damage);
            GetText((int)Texts.QuestionNumber_Text).text = "5";
        }

        PreCalculate();
    }

    IEnumerator Waitfor2Sec()
    {
        yield return new WaitForSeconds(2.0f);
        Debug.Log("Wait2Sec");
        GetText((int)Texts.PrintNumber_Text).text = "";
    }

    #endregion

    #region 데미지 주기

    void damageToPlayer(int damage)
    {
        // Sound
        Managers.Sound.Play("AttackEff");

        GetObject((int)GameObjects.Player).GetOrAddComponent<PlayerController>()._hp -= damage;
        Debug.Log("player damage 1");
        //GetObject((int)GameObjects.Witch).GetOrAddComponent<WitchController>().Questioning();
        GameObject.Find($"Player/Circle/heart{Managers.Game._idxOfHeart}").SetActive(false);
        GetObject((int)GameObjects.Player).GetOrAddComponent<PlayerController>().BlinkPlayerImg();
        if (Managers.Game._idxOfHeart > GetObject((int)GameObjects.Player).GetOrAddComponent<PlayerController>()._hp)
            return;
        else
            Managers.Game._idxOfHeart++;
    }

    void damageToWitch(int damage)
    {
        // Sound
        Managers.Sound.Play("AttackEff");

        GetObject((int)GameObjects.Witch).GetOrAddComponent<WitchController>().SetWitchHP(damage);
        //GetObject((int)GameObjects.Witch).GetOrAddComponent<WitchController>().Questioning();
        if(witchController.Hp <= 0)
        {
            Managers.Scene.ChangeScene(Define.Scene.LobbyScene);
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

    #region 화살관리

    string[] Operator = { "+", "-" };

    private const int MAX_NUM_ARROW = 3;
    private const int MAX_SYMBOL_ARROW = 2;
    private int numArrowCnt = 0;
    private int symbolArrowCnt = 0;

    // 화살이 생성되는 시간 조절하는 함수 
    // 현재 화살 개수가 몇개 나왔는지 체크
    IEnumerator SetArrowGenerationTime(float delayTime)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(delayTime);

        ShootArrow();

        yield return waitForSeconds;
        StartCoroutine("SetArrowGenerationTime", 1f);
    }

    // 현재 플레이어의 위치를 향해 오브젝트 날리기 
    void ShootArrow()
    {
        GameObject arrowObj = MakeArrow();
        Arrow arrow = arrowObj.GetComponent<Arrow>();
        arrowObj.GetComponent<Rigidbody2D>().AddForce(arrow.direction.normalized * arrow.speed, ForceMode2D.Impulse);

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

    // 화살이 들고있는 값을 설정하는 함수 
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
        arrow.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = UnityEngine.Random.Range(1, 5).ToString();
    }

    void SetArrowOperator(Arrow arrow)
    {
        arrow.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = Operator[UnityEngine.Random.Range(0, 2)];   // 50%�� Ȯ���� Symbol�� ��Ģ���� �� �ϳ��� ��ȣ�� �ش��Ѵ�.
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
        LookAt(GetObject((int)GameObjects.Player), arrow);     // Player�� �ٶ󺸰� ���󰡰Բ�

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

        arrow.speed = Random.Range(200.0f, 250.0f) * Mathf.Min(widthRatio, heightRatio);
    }

    #endregion

}
