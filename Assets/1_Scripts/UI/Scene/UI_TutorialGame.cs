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

public class UI_TutorialGame : UI_Scene
{
    enum Texts
    {
        Calculate_BoardText,
        PrintNumber_Text,
        QuestionNumber_Text,
    }

    enum Buttons
    {
        EqualButton,
        GrcOfGaussBtn,
        GrcOfPythagorasBtn,
        AllErase,
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

        BindEvent(gameObject, OnPointerDown, Define.UIEvent.PointerDown);
        BindEvent(gameObject, OnPointerUp, Define.UIEvent.PointerUp);
        BindEvent(gameObject, OnDrag, Define.UIEvent.Pressed);

        GetText((int)Texts.PrintNumber_Text).text = "";
        GetText((int)Texts.Calculate_BoardText).text = "";
        GetButton((int)Buttons.EqualButton).gameObject.BindEvent(Calculate);

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

        // Graces
        GetButton((int)Buttons.GrcOfGaussBtn).gameObject.BindEvent(() => Managers.Grace.CallGrace("GraceOfGauss"));
        GetButton((int)Buttons.GrcOfPythagorasBtn).gameObject.BindEvent(() => Managers.Grace.CallGrace("GraceOfPythagoras"));
        GetButton((int)Buttons.AllErase).gameObject.BindEvent(() => AllErase());

        GetText((int)Texts.QuestionNumber_Text).text = "8";

        PlayerPrefs.SetInt("DoTutorial", 2);
        ShowTutorialPopup();
        return true;
    }
    void ShowTutorialPopup()
    {
        Managers.UI.ShowPopupUI<UI_TutorialGamePopup>();
    }

    #region ���� ���

    void AllErase()
    {
        GetText((int)Texts.Calculate_BoardText).text = "";
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
            Debug.Log($"\"{expressionToCalculate}\" result is : " + printResult);
        }
        catch (System.Exception e)
        {
            Debug.Log($"\"{expressionToCalculate}\" is inappropriate expression! : {e}");
            printResult = "";
            //damageToPlayer(1);
            return;
        }

        if (GetText((int)Texts.PrintNumber_Text))
        {
            GetText((int)Texts.PrintNumber_Text).text = $"={printResult}";
            StartCoroutine(Waitfor2Sec());
        }
        switch (turn)
        {
            case 0:
                if (int.Parse(printResult) == 8)
                    damageToWitch(Managers.Game.Damage);
                    GetText((int)Texts.QuestionNumber_Text).text = "6";
                    turn++;
                break;
            case 1:
                if (int.Parse(printResult) == 6)
                    damageToWitch(Managers.Game.Damage);
                GetText((int)Texts.QuestionNumber_Text).text = "7";
                turn++;
                break;
            case 2:
                if (int.Parse(printResult) == 7)
                    damageToWitch(Managers.Game.Damage);
                GetText((int)Texts.QuestionNumber_Text).text = "5";
                break;
        }
    }

    IEnumerator Waitfor2Sec()
    {
        yield return new WaitForSeconds(2.0f);
        Debug.Log("Wait2Sec");
        GetText((int)Texts.PrintNumber_Text).text = "";
    }

    #endregion

    #region ������ �ֱ�

    void damageToPlayer(int damage)
    {
        GetObject((int)GameObjects.Player).GetOrAddComponent<PlayerControllerCCF>()._hp -= damage;
        Debug.Log("player damage 1");
        //GetObject((int)GameObjects.Witch).GetOrAddComponent<WitchController>().Questioning();
        GameObject.Find($"Player/Circle/heart{Managers.Game._idxOfHeart}").SetActive(false);
        GetObject((int)GameObjects.Player).GetOrAddComponent<PlayerControllerCCF>().BlinkPlayerImg();
        if (Managers.Game._idxOfHeart > GetObject((int)GameObjects.Player).GetOrAddComponent<PlayerControllerCCF>()._hp)
            return;
        else
            Managers.Game._idxOfHeart++;
    }

    void damageToWitch(int damage)
    {
        GetObject((int)GameObjects.Witch).GetOrAddComponent<WitchController>().SetWitchHP(damage);
        //GetObject((int)GameObjects.Witch).GetOrAddComponent<WitchController>().Questioning();
    }

    #endregion

    #region ���̽�ƽ

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

    #region ȭ�� ����

    string[] Operator = { "+", "-" };

    private const int MAX_NUM_ARROW = 3;
    private const int MAX_SYMBOL_ARROW = 2;
    private int numArrowCnt = 0;
    private int symbolArrowCnt = 0;
    //public GameObject arrowPrefab;
    //EdgeCollider2D edgeCollider;
    //public TextMeshProUGUI SetText;

    //IEnumerator SetGame()
    //{
    //    Time.timeScale = 0.0f;
    //    Debug.Log("SetGame");
    //    SetText.text = "3";
    //    yield return new WaitForSecondsRealtime(1.0f);
    //    SetText.text = "2";
    //    yield return new WaitForSecondsRealtime(1.0f);
    //    SetText.text = "1";
    //    yield return new WaitForSecondsRealtime(1.0f);
    //    SetText.enabled = false;
    //    Time.timeScale = 1.0f;
    //    Debug.Log("StartGame");
    //}

    // ȭ���� �����Ǵ� �ð� �����ϴ� �Լ� 
    // ���� ȭ�� ������ � ���Դ��� üũ
    IEnumerator SetArrowGenerationTime(float delayTime)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(delayTime);

        ShootArrow();

        yield return waitForSeconds;
        StartCoroutine("SetArrowGenerationTime", 1f);
    }

    // ���� �÷��̾��� ��ġ�� ���� ������Ʈ ������ 
    void ShootArrow()
    {
        GameObject arrowObj = MakeArrow();
        Arrow arrow = arrowObj.GetComponent<Arrow>();
        arrowObj.GetComponent<Rigidbody2D>().AddForce(arrow.direction.normalized * arrow.speed, ForceMode2D.Impulse);

        Debug.Log("------Shoot Arrow------");
        Debug.Log($"Arrow type: {arrow.type} num or operator: {arrow.tmp} speed: {arrow.speed} \n startPosition:{arrow.startPosition.x} , {arrow.startPosition.y} \n direction: {arrow.direction}");
    }

    // ȭ�� ���� �����ϴ� �Լ�
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

    // ȭ�� �����ϴ� �Լ�
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

    // ���� ������ ȭ���� Ÿ�� �������� ��ȣ���� �����ϴ� �Լ� 
    int SetArrowType(Arrow arrow)
    {
        arrow.type = UnityEngine.Random.Range(0, 2);

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

    // ȭ���� ���� ��ġ �����ϴ� �Լ� 
    void SetArrowStartPosition(Arrow arrow)
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

    // ȭ���� ���� �����ϴ� �Լ� 
    // ���� �÷��̾��� ��ġ�� ����
    void SetArrowDirection(Arrow arrow)
    {
        //arrow.direction = GetObject((int)GameObjects.Player).transform.position - (Vector3)arrow.startPosition;
        arrow.direction = FindObjectOfType<PlayerControllerCCF>().transform.position - (Vector3)arrow.startPosition;
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

    // ȭ���� �ӵ� �����ϴ� �Լ� 
    void SetArrowSpeed(Arrow arrow)
    {
        arrow.speed = UnityEngine.Random.Range(250.0f, 270.0f);
    }

    #endregion

}
