using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class ArrowController : MonoBehaviour
{
    //public string Symbol;       // Symbol을 Inspector에서 간편하게 설정할 수 있도록,
    //TextMeshPro tmp;            // 화살의 Symbol이 표시될 TextMeshPro
    string[] Operator = { "+", "-", "x", "/" };
    public GameObject player;

    private const int MAX_NUM_ARROW = 3;
    private const int MAX_SYMBOL_ARROW = 2;
    private int numArrowCnt = 0;
    private int symbolArrowCnt = 0;
    public GameObject arrowPrefab;
    EdgeCollider2D edgeCollider;
    public TextMeshProUGUI SetText;

    private void Start()
    {
        // StartCoroutine("SetGame");
        //if (player == null)
        player = GameObject.FindGameObjectWithTag("Player"); // Player를 찾지못하는 오류를 고치기 위한 코드
        edgeCollider = GetComponent<EdgeCollider2D>();

        StartCoroutine("SetArrowGenerationTime", 3f);
    }

    IEnumerator SetGame()
    {
        Time.timeScale = 0.0f;
        Debug.Log("SetGame");
        SetText.text = "3";
        yield return new WaitForSecondsRealtime(1.0f);
        SetText.text = "2";
        yield return new WaitForSecondsRealtime(1.0f);
        SetText.text = "1";
        yield return new WaitForSecondsRealtime(1.0f);
        SetText.enabled = false;
        Time.timeScale = 1.0f;
        Debug.Log("StartGame");
    }

    void LookAt(GameObject target, Arrow arrow)
    {
        if (target != null)
        {
            Vector2 direction = arrow.direction;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion angleAxis = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
            Quaternion rotation = Quaternion.Slerp(arrow.transform.rotation, angleAxis, 1);

            arrow.transform.rotation = rotation;
        }
    }

    // 화살이 생성되는 시간 조절하는 함수 
    // 현재 화살 개수가 몇개 나왔는지 체크
    IEnumerator SetArrowGenerationTime(float delayTime)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(delayTime);

        ShootArrow();
        yield return waitForSeconds;
        StartCoroutine("SetArrowGenerationTime", 1f);
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
        arrow.tmp.text = Random.Range(1, 10).ToString();
    }

    void SetArrowOperator(Arrow arrow)
    {
        arrow.tmp.text = Operator[Random.Range(0, 4)].ToString();   // 50%의 확률로 Symbol이 사칙연산 중 하나의 기호에 해당한다.
    }

    // 화살의 방향 조절하는 함수 
    // 현재 플레이어의 위치로 설정
    void SetArrowDirection(Arrow arrow)
    {
        arrow.direction = player.transform.position - (Vector3)arrow.startPosition;
        LookAt(player, arrow);     // Player를 바라보고 날라가게끔

        arrow.GetComponentInChildren<RectTransform>().localRotation = Quaternion.Euler(0, 0, arrow.transform.rotation.eulerAngles.z * (-1.0f));
    }

    // 화살의 속도 조절하는 함수 
    void SetArrowSpeed(Arrow arrow)
    {
        arrow.speed = Random.Range(5.0f, 7.0f);
    }
    
    // 화살의 이미지 수정하는 함수
    void SetArrowSprite(Arrow arrow)
    {
        //Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Effects");
        //SpriteRenderer spriteR = arrow.gameObject.GetComponentInChildren<SpriteRenderer>();
        //spriteR.sprite = sprites[Random.Range(0,4)];
    }

    Vector2 GetRandPosOfLeft()
    {
        Vector2 newPos = new Vector2(Random.Range(edgeCollider.points[0].x, edgeCollider.points[1].x), Random.Range(edgeCollider.points[0].y, edgeCollider.points[1].y));
        return newPos;
    }

    Vector2 GetRandPosOfUp()
    {
        Vector2 newPos = new Vector2(Random.Range(edgeCollider.points[1].x, edgeCollider.points[2].x), Random.Range(edgeCollider.points[1].y, edgeCollider.points[2].y));
        return newPos;
    }

    Vector2 GetRandPosOfRight()
    {
        Vector2 newPos = new Vector2(Random.Range(edgeCollider.points[2].x, edgeCollider.points[3].x), Random.Range(edgeCollider.points[2].y, edgeCollider.points[3].y));
        return newPos;
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

    // 화살 설정하는 함수
    void SetArrow(Arrow arrow)
    {
        arrow.tmp = arrow.GetComponentInChildren<TextMeshPro>();

        if (SetArrowType(arrow) == 0)
            SetArrowNum(arrow);
        else
            SetArrowOperator(arrow);
        SetArrowStartPosition(arrow);
        SetArrowDirection(arrow);
        SetArrowSpeed(arrow);
        SetArrowSprite(arrow);
    }

    // 화살 동적 생성하는 함수
    GameObject MakeArrow()
    {
        GameObject arrowObject = Instantiate(arrowPrefab, this.transform);
        Arrow arrow = arrowObject.GetComponent<Arrow>();
        SetArrow(arrow);
        arrowObject.transform.position = arrow.startPosition;
        return arrowObject;
    }

    // 현재 플레이어의 위치를 향해 오브젝트 날리기 
    void ShootArrow()
    {
        GameObject arrowObj = MakeArrow();
        Arrow arrow = arrowObj.GetComponent<Arrow>();
        arrowObj.GetComponent<Rigidbody2D>().AddForce(arrow.direction.normalized * arrow.speed, ForceMode2D.Impulse);

        Debug.Log("------Shoot Arrow------");
        Debug.Log($"Arrow type: {arrow.type} num or operator: {arrow.tmp.text} speed: {arrow.speed} \n startPosition:{arrow.startPosition.x} , {arrow.startPosition.y} \n direction: {arrow.direction}");
    }
}
