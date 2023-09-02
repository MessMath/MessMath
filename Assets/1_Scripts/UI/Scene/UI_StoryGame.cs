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
using DG.Tweening;

public class UI_StoryGame : UI_Scene
{
    enum Texts
    {
        Calculate_BoardText,
        PrintNumber_Text,
        QuestionNumber_Text,
        PreCalculation_Text,
    }

    enum Buttons
    {
        // 가호 버튼들
        SelectedGrace,
        SelectedGrace1,
        SelectedGrace2,
        AllErase,
        EqualButton,
        ExitBtn,
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
        FadeOut,
        Aura,
        EasyWand,
        Wand,
        NormalWandAttImage,
        NormalEye,
        NormalEyeLight,
        NormalAttEff1,
        NormalAttEff2,
        NormalAttEff3,
        W_Twinkle_1,
        W_Twinkle_2,
        W_H_Attack_Before,
        W_h_attack_1,
        W_h_attack_2,
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

        CoroutineHandler.StartCoroutine(SceneChangeAnimation_Out());

        StartCoroutine("SetArrowGenerationTime", 0.5f);
        StartCoroutine("EasyModeAttack");
    }

    #region 씬 변환 애니메이션
    IEnumerator SceneChangeAnimation_Out()
    {
        // Ani
        UI_LockTouch uI_LockTouch = Managers.UI.ShowPopupUI<UI_LockTouch>();
        SceneChangeAnimation_Out anim = Managers.Resource.Instantiate("Animation/SceneChangeAnimation_Out").GetOrAddComponent<SceneChangeAnimation_Out>();
        anim.transform.SetParent(uI_LockTouch.transform);

        yield return new WaitForSeconds(0.3f);
        Managers.UI.ClosePopupUI(uI_LockTouch);
    }
    #endregion

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
        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ShowPopupUI<UI_CheckToLobby>(); });

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

        // default damage is 5
        Managers.Game.Damage = 5;

        #region 가호 버튼 설정

        if (PlayerPrefs.GetString("SelectedGrace0InStory") != "")
        {
            GetButton((int)Buttons.SelectedGrace).gameObject.BindEvent(() => Managers.Grace.CallGrace(PlayerPrefs.GetString("SelectedGrace0InStory"), GetButton((int)Buttons.SelectedGrace).gameObject));
            GetButton((int)Buttons.SelectedGrace).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Grace/" + PlayerPrefs.GetString("SelectedGrace0InStory"));
        }

        if (PlayerPrefs.GetString("SelectedGrace1InStory") != "")
        {
            GetButton((int)Buttons.SelectedGrace1).gameObject.BindEvent(() => Managers.Grace.CallGrace(PlayerPrefs.GetString("SelectedGrace1InStory"), GetButton((int)Buttons.SelectedGrace1).gameObject));
            GetButton((int)Buttons.SelectedGrace1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Grace/" + PlayerPrefs.GetString("SelectedGrace1InStory"));
        }

        if (PlayerPrefs.GetString("SelectedGrace2InStory") != "")
        {
            GetButton((int)Buttons.SelectedGrace2).gameObject.BindEvent(() => Managers.Grace.CallGrace(PlayerPrefs.GetString("SelectedGrace2InStory"), GetButton((int)Buttons.SelectedGrace2).gameObject));
            GetButton((int)Buttons.SelectedGrace2).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Grace/" + PlayerPrefs.GetString("SelectedGrace2InStory"));
        }
        #endregion

        // 지우개 버튼
        GetButton((int)Buttons.AllErase).gameObject.BindEvent(() => AllErase());

        UnityEngine.Input.multiTouchEnabled = true;

        #region 애니 준비
        // 2페이즈 관련 끄기
        GetImage((int)Images.Wand).gameObject.SetActive(false);
        GetImage((int)Images.NormalWandAttImage).gameObject.SetActive(false);
        GetImage((int)Images.NormalEye).gameObject.SetActive(false);
        GetImage((int)Images.NormalEyeLight).gameObject.SetActive(false);
        GetImage((int)Images.NormalAttEff1).gameObject.SetActive(false);
        GetImage((int)Images.NormalAttEff2).gameObject.SetActive(false);
        GetImage((int)Images.NormalAttEff3).gameObject.SetActive(false);


        // 3페이즈 관련 끄기
        GetImage((int)Images.W_Twinkle_1).gameObject.SetActive(false);
        GetImage((int)Images.W_Twinkle_2).gameObject.SetActive(false);
        GetImage((int)Images.W_H_Attack_Before).gameObject.SetActive(false);
        GetImage((int)Images.W_h_attack_1).gameObject.SetActive(false);
        GetImage((int)Images.W_h_attack_2).gameObject.SetActive(false);
        #endregion

        GetImage((int)Images.FadeOut).gameObject.SetActive(false);

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
        DamageText(damage);

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
            {
                Managers.DBManager.SetIsKilledWitch(true);
                Managers.DBManager.SetMyClothes("the_wise"); // 대현자 해금
                Managers.DBManager.SetObtainedClothes("the_wise");
                Managers.Sound.Clear();
                StartCoroutine(Epilogue());
            }
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
        arrow.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
    }

    void SetArrowOperator(Arrow arrow)
    {
        string oper = Operator[Random.Range(0, 4)];
        arrow.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = oper;
        arrow.gameObject.GetComponentInChildren<TextMeshProUGUI>().outlineWidth = 0.2f;
        arrow.gameObject.GetComponentInChildren<TextMeshProUGUI>().fontSize = 350;
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

    private int[] numberMin = { 1, 1, 1 };             // 등장 숫자 최소값
    private int[] numberMax = { 10, 15, 20 };         // 등장 숫자 최대값
    private float[] delayTime = { 1f, 0.8f, 0.6f };      // 화살 발사 딜레이
    private float[] speedMin = { 200f, 280f, 360f };     // 화살 속도 최소값
    private float[] speedMax = { 250f, 330f, 410f };     // 화살 속도 최대값

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

        // 2페이즈의 특수효과 시작, 간격은 일단 10초
        if (phase == Phase.Phase2)
        {
            StopCoroutine("EasyModeAttack"); // 이지 어택 애니 종료

            // 2페이즈 마녀 이미지 변환 및 애니메이션
            StartCoroutine(WitchChangeAnimation_Normal());

            GetImage((int)Images.EasyWand).gameObject.SetActive(false);
            GetImage((int)Images.WitchHPBar).color = new Color(119 / 255f, 255 / 255f, 245 / 255f, 1f);
            GetImage((int)Images.Wand).gameObject.SetActive(true);
            GetImage((int)Images.NormalEye).gameObject.SetActive(true);
            GetImage((int)Images.NormalEyeLight).gameObject.SetActive(true);
            GetImage((int)Images.WitchImage).sprite = Managers.Resource.Load<Sprite>("Sprites/Character/witch/W_nomal");
            GetImage((int)Images.Aura).sprite = Managers.Resource.Load<Sprite>("Sprites/Character/witch/W_nomal_aura");
            GetImage((int)Images.Wand).sprite = Managers.Resource.Load<Sprite>("Sprites/Character/witch/W_nomal_wand");
            GetImage((int)Images.BGIMG).sprite = Managers.Resource.Load<Sprite>("Sprites/background/BattlePhase2");
            StartCoroutine(SpecialEffectsForPhase2(10f));
        }

        if (phase == Phase.Phase3)
        {
            StopCoroutine("NormalModeAttack"); // 노말 어택 애니 종료
            StopAllCoroutines();

            // 2페이즈 관련 끄기
            GetImage((int)Images.NormalEye).gameObject.SetActive(false);
            GetImage((int)Images.NormalEyeLight).gameObject.SetActive(false);
            GetImage((int)Images.NormalWandAttImage).gameObject.SetActive(false);
            GetImage((int)Images.NormalAttEff1).gameObject.SetActive(false);
            GetImage((int)Images.NormalAttEff2).gameObject.SetActive(false);
            GetImage((int)Images.NormalAttEff3).gameObject.SetActive(false);


            // 3페이즈 마녀 이미지 변환 및 애니메이션
            StartCoroutine(WitchChangeAnimation_Hard());

            GetImage((int)Images.WitchHPBar).color = new Color(100 / 255f, 0f, 200 / 255f, 1f);
            GetImage((int)Images.EasyWand).gameObject.SetActive(false);
            GetImage((int)Images.Wand).gameObject.SetActive(false);
            GetImage((int)Images.W_H_Attack_Before).gameObject.SetActive(true);
            GetImage((int)Images.W_Twinkle_1).gameObject.SetActive(true);
            GetImage((int)Images.W_Twinkle_2).gameObject.SetActive(true);
            GetImage((int)Images.WitchImage).sprite = Managers.Resource.Load<Sprite>("Sprites/Character/witch/Witch_Hard/W_hard");
            GetImage((int)Images.Aura).sprite = Managers.Resource.Load<Sprite>("Sprites/Character/witch/Witch_Hard/W_aura");
            GetImage((int)Images.BGIMG).sprite = Managers.Resource.Load<Sprite>("Sprites/background/BattlePhase3");

        }

    }

    #region 마녀 페이즈 애니메이션
    IEnumerator WitchChangeAnimation_Normal()
    {
        Managers.Sound.Clear();
        StopCoroutine("SetArrowGenerationTime");

        // Sound
        Managers.Sound.Play("페이즈전환Eff");

        // Ani
        UI_LockTouch uI_LockTouch = Managers.UI.ShowPopupUI<UI_LockTouch>();
        SceneChangeAnimation_In anim = Managers.Resource.Instantiate("Animation/WitchChangeAnimation_Normal").GetOrAddComponent<SceneChangeAnimation_In>();
        anim.transform.Find("NormalWitchText").transform.DOShakePosition(10, 20);
        anim.transform.SetParent(uI_LockTouch.transform);
        anim.SetInfo(Define.Scene.StoryGameScene, () => { });

        yield return new WaitForSeconds(8.3f);
        Managers.UI.ClosePopupUI(uI_LockTouch);

        Managers.Sound.Play("BattleBgm", Define.Sound.Bgm);
    }

    IEnumerator WitchChangeAnimation_Hard()
    {
        Managers.Sound.Clear();
        StopCoroutine("SetArrowGenerationTime");

        // Sound
        Managers.Sound.Play("페이즈전환Eff");

        // Ani
        UI_LockTouch uI_LockTouch = Managers.UI.ShowPopupUI<UI_LockTouch>();
        SceneChangeAnimation_In anim = Managers.Resource.Instantiate("Animation/WitchChangeAnimation_Hard").GetOrAddComponent<SceneChangeAnimation_In>();
        anim.transform.Find("HardWitchText").transform.DOShakePosition(10, 30);
        anim.transform.SetParent(uI_LockTouch.transform);
        anim.SetInfo(Define.Scene.StoryGameScene, () => { });

        yield return new WaitForSeconds(6.3f);
        Managers.UI.ClosePopupUI(uI_LockTouch);

        Managers.Sound.Play("BattleBgm", Define.Sound.Bgm);

        StartCoroutine(SetArrowGenerationTime(delayTime[(int)currentPhase]));

        StartCoroutine(SpecialEffectsForPhase3(1.0f));
    }
    #endregion

    #region 2페이즈 특수 효과

    float _phase2Skill1Delay = 10f;
    IEnumerator SpecialEffectsForPhase2(float delay)
    {
        yield return new WaitForSeconds(delay);

        StartCoroutine(MiddleDirectionOfArrow());

        StartCoroutine(NormalModeAttack()); // 노말 모드 어택 애니

        StartCoroutine(SpecialEffectsForPhase2(Random.Range(_phase2Skill1Delay / 2f, _phase2Skill1Delay)));
    }

    IEnumerator MiddleDirectionOfArrow()
    {
        StopCoroutine("SetArrowGenerationTime");
        yield return new WaitForSeconds(1.0f);

        Managers.Sound.Play("3페이즈스킬");

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

        StartCoroutine(HardModeAttack()); // 하드 모드 어택 애니

        ChangeDirectionOfArrows();

        StartCoroutine(SpecialEffectsForPhase3(Random.Range(_phase3Skill1Delay / 2f, _phase3Skill1Delay)));
    }

    public void ChangeDirectionOfArrows()
    {
        GameObject[] arrows = GameObject.FindGameObjectsWithTag("Arrow");

        Managers.Sound.Play("3페이즈스킬");

        foreach (GameObject arrow in arrows)
        {
            Vector2 curVec = new Vector2(arrow.GetComponent<Arrow>().direction.x, arrow.GetComponent<Arrow>().direction.y);
            Vector2 RandomVector2 = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            float speed = arrow.GetComponent<Arrow>().speed;
            arrow.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            arrow.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Sprites/Effects/EnergyBall");
            arrow.GetComponentInChildren<Image>().SetNativeSize();
            if (arrow.GetComponent<Arrow>().type == 1)
                arrow.GetComponentInChildren<TextMeshProUGUI>().fontSize = 400;
            else if (arrow.GetComponent<Arrow>().type == 0)
            {
                arrow.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                arrow.GetComponentInChildren<TextMeshProUGUI>().fontSize = 300;
            }
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

    #region 마녀 공격 애니

    #region EasyModeAttack
    // TODO EASY
    IEnumerator EasyModeAttack()
    {
        GetImage((int)Images.WitchImage).sprite = Managers.Resource.Load<Sprite>("Sprites/Character/witch/W_easy_attack");

        yield return new WaitForSeconds(1.0f); 

        GetImage((int)Images.WitchImage).sprite = Managers.Resource.Load<Sprite>("Sprites/Character/witch/W_easy");

        yield return new WaitForSeconds(UnityEngine.Random.Range(1.0f, 5.0f));
        StartCoroutine("EasyModeAttack");
    }
    #endregion

    #region NormalModeAttack

    // TODO Noraml
    IEnumerator NormalModeAttack()
    {
        // TODO 사운드 추가

        //StartCoroutine(NormalModeAttackWand());
        GetImage((int)Images.Wand).gameObject.GetComponent<Animator>().Play("NormalWandAttAni");
        GetImage((int)Images.WitchImage).sprite = Managers.Resource.Load<Sprite>("Sprites/Character/witch/W_nomal_attack_before");
        GetImage((int)Images.NormalEye).sprite = Managers.Resource.Load<Sprite>("Sprites/Character/witch/W_nomal_attack_before_eye");
        GetImage((int)Images.NormalEyeLight).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        GetImage((int)Images.WitchImage).sprite = Managers.Resource.Load<Sprite>("Sprites/Character/witch/W_nomal_attack");
        GetImage((int)Images.NormalEye).sprite = Managers.Resource.Load<Sprite>("Sprites/Character/witch/W_nomal_attack_eye");
        GetImage((int)Images.NormalWandAttImage).gameObject.SetActive(true);
        GetImage((int)Images.NormalAttEff1).gameObject.SetActive(true);
        GetImage((int)Images.NormalAttEff2).gameObject.SetActive(true);
        GetImage((int)Images.NormalAttEff3).gameObject.SetActive(true);
        yield return new WaitForSeconds(2.3f);

        GetImage((int)Images.NormalWandAttImage).gameObject.SetActive(false);
        GetImage((int)Images.NormalAttEff1).gameObject.SetActive(false);
        GetImage((int)Images.NormalAttEff2).gameObject.SetActive(false);
        GetImage((int)Images.NormalAttEff3).gameObject.SetActive(false);

        GetImage((int)Images.Wand).gameObject.GetComponent<Animator>().Play("NormalWandAttBackAni");
        GetImage((int)Images.WitchImage).sprite = Managers.Resource.Load<Sprite>("Sprites/Character/witch/W_nomal_attack_before");
        GetImage((int)Images.NormalEye).sprite = Managers.Resource.Load<Sprite>("Sprites/Character/witch/W_nomal_attack_before_eye");
        yield return new WaitForSeconds(0.3f);
        GetImage((int)Images.WitchImage).sprite = Managers.Resource.Load<Sprite>("Sprites/Character/witch/W_nomal");
        GetImage((int)Images.NormalEye).sprite = Managers.Resource.Load<Sprite>("Sprites/Character/witch/W_nomal_eye");
        GetImage((int)Images.NormalEyeLight).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);

    }

    IEnumerator NormalModeAttackWand()
    {
        GetImage((int)Images.Wand).transform.DOLocalMove(new Vector3(-500, -200), 0.3f, true).SetRelative().SetEase(Ease.Linear).Rewind();
        yield return new WaitForSeconds(0.4f);
        //GetImage((int)Images.Wand).sprite = Managers.Resource.Load<Sprite>("Sprites/Character/witch/W_nomal_attack_Eff_0_wand");
        yield return new WaitForSeconds(1.0f);
        //GetImage((int)Images.Wand).sprite = Managers.Resource.Load<Sprite>("Sprites/Character/witch/W_nomal_wand");
        //GetImage((int)Images.Wand).transform.DOLocalMove(new Vector3(500, 200), 0.3f, true).SetRelative().SetEase(Ease.Linear);

    }

    #endregion

    #region HardModeAttack

    // TODO HARD
    IEnumerator HardModeAttack()
    {
        GetImage((int)Images.W_H_Attack_Before).gameObject.SetActive(false);
        GetImage((int)Images.W_h_attack_1).gameObject.SetActive(true);
        GetImage((int)Images.W_h_attack_2).gameObject.SetActive(true);
        yield return new WaitForSeconds(3.0f);

        GetImage((int)Images.W_H_Attack_Before).gameObject.SetActive(true);
        GetImage((int)Images.W_h_attack_1).gameObject.SetActive(false);
        GetImage((int)Images.W_h_attack_2).gameObject.SetActive(false);
    }

    #endregion

    #endregion

    #region 에필로그

    // 페이즈3에서 마지막에 마녀를 무찌를 때 호출되는 함수.
    IEnumerator Epilogue()
    {
        Time.timeScale = 0.3f;

        yield return SetPitchLow();

        Time.timeScale = 1f;

        // TODO
        // 1. 팝업 없애고
        // 2. 화면 페이드 아웃 넣고
        // 3. 이후 에필로그 보여주는 Scene으로...

        StartCoroutine(FadeOut());
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

    IEnumerator FadeOut()
    {
        Image FadeOut = transform.Find("FadeOut").GetComponent<Image>();
        Color fadecolor = FadeOut.color;
        FadeOut.gameObject.SetActive(true);

        float time = 0f;
        float FadingTime = 1f;

        float start = 0f;
        float end = 1f;

        while (FadeOut.color.a < 1f)
        {
            time += Time.deltaTime / FadingTime;

            fadecolor.a = Mathf.Lerp(start, end, time);

            FadeOut.color = fadecolor;

            yield return null;
        }

        Managers.Scene.ChangeScene(Define.Scene.EpilogueScene);
    }

    #endregion

    #region 데미지 텍스트
    void DamageText(int damage)
    {
        GameObject text = Instantiate(Resources.Load<GameObject>("Prefabs/UI/SubItem/DamageText"));
        text.transform.SetAsLastSibling();
        text.GetComponent<UI_DamageText>().damage = damage;
    }
    #endregion
}