using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using System;
using TMPro;
using System.Linq;
using Random = UnityEngine.Random;
using Photon.Pun;
using Photon.Realtime;
using System.Threading.Tasks;

public class UI_PvpGameScene : UI_Scene
{
    PhotonView PhotonView;
    public int QusetionNumber;
    public int _player1Score = 0;
    public int _player2Score = 0;

    Player OppPlayer;
    string OppPlayersName;
    int OppScore;
    string OppPlayersCloth;

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
        ToMainBtn,
    }

    enum Images
    {
        BGIMG,
        JoyStickHandle,
        MyScore1,
        MyScore2,
        MyScore3,
        OpponentScore1,
        OpponentScore2,
        OpponentScore3,
    }

    enum GameObjects
    {
        DeadLine,
        Wall1,
        Wall2,
        Wall3,
        Wall4,
        JoyStick,
        ArrowController,
        JoyStickPanel,
        MyScores,
        OppsScores,
    }

    private void Awake()
    {
        Init();

    }

    private void Start()
    {
        StartCoroutine("SetArrowGenerationTime", 0.5f);

        Managers.Network.Spawn();
    }

    Player[] playerList;

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

        GetText((int)Texts.PrintNumber_Text).text = "";
        GetText((int)Texts.Calculate_BoardText).text = "";
        GetButton((int)Buttons.EqualButton).gameObject.BindEvent(Calculate);

        canvas = gameObject.GetComponent<Canvas>();
        canvasGroup = GetObject((int)GameObjects.JoyStick).GetComponent<RectTransform>().GetComponent<CanvasGroup>();

        // 배경 숫자
        Questioning();

        #region 사운드 설정
        Managers.Sound.Clear();
        Managers.Sound.Play("PvpGameBgm", Define.Sound.Bgm);
        #endregion

        // 지우개 버튼
        GetButton((int)Buttons.AllErase).gameObject.BindEvent(() => AllErase());

        // 뒤로가기 버튼
        GetButton((int)Buttons.ToMainBtn).gameObject.BindEvent(() => {
            Managers.Sound.Play("ClickBtnEff");
            Managers.UI.ShowPopupUI<UI_CheckToLobby>();
        });

        playerList = PhotonNetwork.PlayerList;

        // 팝업에서 쓸 상대방 정보 준비
        ReadyOppsData();

        Managers.Scene.CurrentSceneType = Define.Scene.PvpGameScene;

        PhotonNetwork.AutomaticallySyncScene = false;

        return true;
    }
    
    void ReadyOppsData()
    {
        OppsDataReadyAsync();

        string myCloth = "";

        var gettigCloth = Managers.DBManager.GetMyClothes(Managers.GoogleSignIn.GetUID()).GetAwaiter();
        gettigCloth.OnCompleted(() => {

            myCloth = gettigCloth.GetResult();

        });

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            string cloth = player.GetComponent<PhotonView>().IsMine ? myCloth : OppPlayersCloth;
            player.GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>("Sprites/Clothes/" + cloth);
        }
    }

    public void OppsDataReadyAsync()
    {
        OppPlayer = GetOppPlayer();

        var gettingOppPlayerName = Managers.DBManager.ReadDataAsync(OppPlayer.NickName, "nickname").GetAwaiter();
        gettingOppPlayerName.OnCompleted(() => {
            OppPlayersName = gettingOppPlayerName.GetResult();
        });

        var gettingOppScore = Managers.DBManager.GetScore(OppPlayer.NickName).GetAwaiter();
        gettingOppScore.OnCompleted(() =>
        {
            OppScore = gettingOppScore.GetResult();
        });


        var gettingOppPlayersCloth = Managers.DBManager.ReadDataAsync(OppPlayer.NickName, "myClothes").GetAwaiter();
        gettingOppPlayersCloth.OnCompleted(() =>
        {
            OppPlayersCloth = gettingOppPlayersCloth.GetResult();
        });

    }

    public void ScoreSet()
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            ActivateNChildren(GetObject((int)GameObjects.MyScores), _player1Score);
            ActivateNChildren(GetObject((int)GameObjects.OppsScores), _player2Score);
        }
        else
        {
            ActivateNChildren(GetObject((int)GameObjects.MyScores), _player2Score);
            ActivateNChildren(GetObject((int)GameObjects.OppsScores), _player1Score);
        }

        if (_player1Score == 3 || _player2Score == 3)
            PvpResult();
    }

    public void ActivateNChildren(GameObject parentObject, int numberOfChildrenToActivate)
    {
        if (numberOfChildrenToActivate == 0)
            return;

        int childCount = parentObject.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = parentObject.transform.GetChild(i);

            if (!child.gameObject.activeSelf)
                child.gameObject.SetActive(true);

            if (i == numberOfChildrenToActivate - 1)
                break;
        }
    }

    public void PvpResult(bool UnconditionallyWin = false)
    {
        if(UnconditionallyWin)
        {
            ShowResultPopup(OppPlayer, true);
            return;
        }

        if (_player1Score == 3 || _player2Score == 3)
        {
            bool isWin = (PhotonNetwork.LocalPlayer.ActorNumber == 1 && _player1Score == 3) || (PhotonNetwork.LocalPlayer.ActorNumber == 2 && _player2Score == 3);
            ShowResultPopup(OppPlayer, isWin);
        }
    }

    private void ShowResultPopup(Player player, bool isWin)
    {
        if (isWin)
        {
            UI_PvpGameResult_Win p = Managers.UI.ShowPopupUI<UI_PvpGameResult_Win>();
            p.OppPlayer = player;
            p.OppPlayersName = OppPlayersName;
            p.OppPlayersScore = OppScore;
            p.OppPlayersCloth = OppPlayersCloth;
        }
        else
        {
            UI_PvpGameResult_Lose p = Managers.UI.ShowPopupUI<UI_PvpGameResult_Lose>();
            p.OppPlayer = player;
            p.OppPlayersName = OppPlayersName;
            p.OppPlayersScore = OppScore;
            p.OppPlayersCloth = OppPlayersCloth;
        }

    }

    Player GetOppPlayer()
    {
        for (int i = 0; i <= playerList.Count(); i++)
        {
            Debug.Log("All __" + playerList[i]);
            if (playerList[i].ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
            {
                Debug.Log(playerList[i]);
                return playerList[i];
            }
        }
        return null;
    }

    #region 수식 계산

    PhotonView RPCSychronizer = null;

    public void Questioning()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        QusetionNumber = Random.Range(10, 50);
        StartCoroutine(TryQnumSync());
    }

    private IEnumerator TryQnumSync()
    {
        while (RPCSychronizer == null)
        {
            GameObject foundObject = GameObject.FindGameObjectWithTag("RPCSychronizer");
            if (foundObject != null)
            {
                RPCSychronizer = foundObject.GetComponent<PhotonView>();
            }
            yield return null;
        }
        RPCSychronizer.RPC("QuestioningNumSync", RpcTarget.AllViaServer, QusetionNumber);
    }

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
        }
        catch (System.Exception)
        {
            printResult = "";
            return;
        }

        if (GetText((int)Texts.PreCalculation_Text))
        {
            GetText((int)Texts.PreCalculation_Text).text = $"={printResult}";
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
        }
        catch (System.Exception)
        {
            printResult = "";
            return;
        }

        if (GetText((int)Texts.PrintNumber_Text))
        {
            GetText((int)Texts.PrintNumber_Text).text = $"={printResult}";
            StartCoroutine(Waitfor2Sec());
        }

        PhotonView = GameObject.FindGameObjectWithTag("RPCSychronizer").GetComponent<PhotonView>();
        PhotonView.RPC("Answer", RpcTarget.AllViaServer, printResult, QusetionNumber, PhotonNetwork.LocalPlayer.ActorNumber);
    }

    IEnumerator Waitfor2Sec()
    {
        yield return new WaitForSeconds(2.0f);
        Debug.Log("Wait2Sec");
        GetText((int)Texts.PrintNumber_Text).text = "";
    }

    #endregion

    #region 조이스틱

    private float deadZone = 0;
    private float hadndleRange = 0.8f;
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

    private const int MAX_NUM_ARROW = 3;
    private const int MAX_SYMBOL_ARROW = 2;
    private int numArrowCnt = 0;
    private int symbolArrowCnt = 0;

    private bool firstCycle = true;

    // 화살이 생성되는 시간 조절하는 함수 
    // 현재 화살 개수가 몇개 나왔는지 체크
    IEnumerator SetArrowGenerationTime(float delayTime)
    {
        yield return new WaitUntil(() => FindObjectOfType<PlayerControllerOnlyinPvp>() != null);

        if (!PhotonNetwork.LocalPlayer.IsMasterClient) yield break;

        WaitForSeconds waitForSeconds = new WaitForSeconds(delayTime);

        ShootArrow();

        yield return waitForSeconds;
        StartCoroutine("SetArrowGenerationTime", 1f);
    }

    // 현재 플레이어의 위치를 향해 오브젝트 날리기 
    public void ShootArrow()
    {
        // 화살 속성 설정
        int type = SetArrowType();
        string text = type == 0 ? SetArrowNum() : SetArrowOperator();
        Vector2 startPosition = SetArrowStartPosition();

        // 화살 오브젝트 생성
        GameObject arrowObj = PhotonNetwork.Instantiate("Prefabs/ArrowOnlyinPvp", startPosition, Quaternion.identity);
        arrowObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        ArrowOnlyinPvp arrow = arrowObj.GetComponent<ArrowOnlyinPvp>();

        // 화살 속성 적용
        arrow.type = type;
        arrow.text = text;
        arrow.startPosition = startPosition;

        SetArrowDirection(arrow);
        SetArrowSpeed(arrow);

        // 화살 발사
        arrowObj.GetComponent<Rigidbody2D>().AddForce(arrow.direction.normalized * arrow.speed, ForceMode2D.Impulse);

        Debug.Log("------Shoot Arrow------");
        Debug.Log($"Arrow type: {arrow.type} num or operator: {arrow.text} speed: {arrow.speed} \n startPosition:{arrow.startPosition.x} , {arrow.startPosition.y} \n direction: {arrow.direction}");

    }

    // 현재 생성된 화살의 타입 숫자인지 기호인지 설정하는 함수 
    int SetArrowType()
    {
        int type = UnityEngine.Random.Range(0, 2);

        if (type == 0)
        {
            numArrowCnt++;
            if (MAX_NUM_ARROW < numArrowCnt)
            {
                type = 1;
                numArrowCnt = 0;
            }
        }
        else if (type == 1)
        {
            symbolArrowCnt++;
            if (MAX_SYMBOL_ARROW < symbolArrowCnt)
            {
                type = 0;
                symbolArrowCnt = 0;
            }
        }
        return type;
    }

    string SetArrowNum()
    {
        return Random.Range(1, 10).ToString();
    }

    string SetArrowOperator()
    {
        return Operator[Random.Range(0, 4)];   // 50%의 확률로 Symbol이 사칙연산 중 하나의 기호에 해당한다.
    }

    // 화살의 생성 위치 조절하는 함수 
    Vector2 SetArrowStartPosition()
    {
        int randValue = Random.Range(0, 3);
        return GetRandPos(randValue);
    }

    Vector2 GetRandPos(int index)
    {
        RectTransform rt = GetComponent<RectTransform>();
        Vector2[] points = GetObject((int)GameObjects.ArrowController).GetComponent<EdgeCollider2D>().points;

        Vector2 p1 = points[index] + (Vector2)rt.position;
        Vector2 p2 = points[index + 1] + (Vector2)rt.position;

        return new Vector2(Random.Range(p1.x, p2.x), Random.Range(p1.y, p2.y));
    }

    private PlayerControllerOnlyinPvp[] PlayerList;

    // 화살의 방향 조절하는 함수 
    // 현재 플레이어의 위치로 설정
    void SetArrowDirection(ArrowOnlyinPvp arrow)
    {
        if(firstCycle)
            PlayerList = GameObject.FindObjectsOfType<PlayerControllerOnlyinPvp>();

        int randValue = Random.Range(0, PlayerList.Length);

        arrow.direction = PlayerList[randValue].transform.position - (Vector3)arrow.startPosition;
        LookAt(PlayerList[randValue].gameObject, arrow);

        arrow.GetComponentInChildren<TextMeshProUGUI>().gameObject.transform.localRotation = Quaternion.Euler(0, 0, arrow.transform.rotation.eulerAngles.z * (-1.0f));
    }

    void LookAt(GameObject target, ArrowOnlyinPvp arrow)
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
    void SetArrowSpeed(ArrowOnlyinPvp arrow)
    {
        float widthRatio = currentWidth / referenceWidth;
        float heightRatio = currentHeight / referenceHeight;

        arrow.speed = Random.Range(200.0f, 250.0f) * Mathf.Min(widthRatio, heightRatio);
    }

    #endregion

}
