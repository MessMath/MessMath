using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    //public string Symbol;       // Symbol을 Inspector에서 간편하게 설정할 수 있도록,
    //TextMeshPro tmp;            // 화살의 Symbol이 표시될 TextMeshPro
    string[] Operator = { "+", "-", "×", "÷" };
    public GameObject player;

    private const float MIN_X = -11.1f;
    private const float MAX_X = 11.11f;
    private const int MAX_NUM_ARROW = 3;
    private const int MAX_SYMBOL_ARROW = 2;
    private int numArrowCnt = 0;
    private int symbolArrowCnt = 0;
    private Color red = new Color(1, 0, 0, 1);
    private Color blue = new Color(0, 0, 1, 1);
    public GameObject arrowPrefab;

    private void Start()
    {
        //if (player == null)
        player = GameObject.FindGameObjectWithTag("Player"); // Player를 찾지못하는 오류를 고치기 위한 코드

        StartCoroutine("SetArrowGenerationTime", 3f);
    }

    void LookAt(GameObject target, Arrow arrow)
    {
        if (target != null)
        {
            Vector2 direction = arrow.direction;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion angleAxis = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
            Quaternion rotation = Quaternion.Slerp(transform.rotation, angleAxis, 1);

            transform.rotation = rotation;
        }
    }

    // 화살이 생성되는 시간 조절하는 함수 
    // 현재 화살 개수가 몇개 나왔는지 체크
    IEnumerator SetArrowGenerationTime(float delayTime)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(delayTime);

        ShootArrow();
        yield return waitForSeconds;
        StartCoroutine("SetArrowGenerationTime", 3f);
    }

    // 현재 생성된 화살의 타입 숫자인지 기호인지 설정하는 함수 
    // TODO: 해당 함수에선 현재 생성된 화살의 숫자와 기호 탐색 후 설정 해줘야 함
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
        arrow.speed = Random.Range(3.0f, 5.0f);
    }

    // 화살의 생성 위치 조절하는 함수 
    void SetArrowStartPosition(Arrow arrow)
    {
        arrow.startPosition = new Vector2(Random.Range(MIN_X, MAX_X), 5f);
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

        Debug.Log("------ShootArrow------");
        Debug.Log("type: " + arrow.type);
        Debug.Log("num or operator: " + arrow.tmp.text);
        Debug.Log("speed" + arrow.speed);
        Debug.Log("startPosition: " + arrow.startPosition.x + ", " + arrow.startPosition.y);
        Debug.Log("direction: " + arrow.direction);
        Debug.Log("-------------------------");

        Destroy(arrowObj, 3f);
    }
}
