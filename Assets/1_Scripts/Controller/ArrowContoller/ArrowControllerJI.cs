using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowControllerJI : MonoBehaviour
{
    // 화살의 시작 위치 랜덤 생성 x = -11.1 ~ 11.11,  y = 5 고정

    private const float MIN_X = -11.1f;
    private const float MAX_X = 11.11f;
    private const int MAX_CNT = 10;
    private const int MAX_NUM_ARROW = 3;
    private const int MAX_SYMBOL_ARROW = 2;
    private int arrowCnt = 0;
    private int numArrowCnt = 0;
    private int symbolArrowCnt = 0;
    private Color red = new Color(1,0,0,1);
    private Color blue = new Color(0,0,1,1);
    public GameObject player;
    public GameObject arrowPrefab;
    
    void Start()
    {
        StartCoroutine("SetArrowGenerationTime",3f);
    }

    void Update()
    {
        // TODO: 플레이어 위치 정보 얻어오기(Player Controller 완성되면 진행)
    }

    // 화살이 생성되는 시간 조절하는 함수 
        // 현재 화살 개수가 몇개 나왔는지 체크
    IEnumerator SetArrowGenerationTime(float delayTime)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(delayTime);
        if(arrowCnt < MAX_CNT) {
            ShootArrow();
            yield return waitForSeconds;
            StartCoroutine("SetArrowGenerationTime", 3f);
        }
        else yield return waitForSeconds;
    }
    
    // 현재 생성된 화살의 타입 숫자인지 기호인지 설정하는 함수 
    // TODO: 해당 함수에선 현재 생성된 화살의 숫자와 기호 탐색 후 설정 해줘야 함
    int SetArrowType(Arrow arrow)
    {
        arrow.type = Random.Range(0, 2);
        if (arrow.type == 0) {
            numArrowCnt++;
            if(MAX_NUM_ARROW < numArrowCnt) {
                arrow.type = 1;
                numArrowCnt = 0;
                return arrow.type;
            }
        }
        symbolArrowCnt++;
        if(MAX_SYMBOL_ARROW < symbolArrowCnt) {
            arrow.type = 0;
            symbolArrowCnt = 0;
        }
        return arrow.type;
    }

    void SetArrowNum(Arrow arrow)
    {
        arrow.num = Random.Range(1, 10);
    }

    void SetArrowSymbol(Arrow arrow)
    {
        switch(Random.Range(0, 4))
        {
            case 0: {
                arrow.mathematicalSymbol = '+';
                break;
            }
            case 1: {
                arrow.mathematicalSymbol = '-';
                break;
            }
            case 2: {
                arrow.mathematicalSymbol = 'x';
                break;
            }
            case 3: {
                arrow.mathematicalSymbol = '/';
                break;
            }
        }
    }

    // 화살의 방향 조절하는 함수 
        // 현재 플레이어의 위치로 설정
    void SetArrowDirection(Arrow arrow)
    {
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
        arrow.direction = playerPos - arrow.startPosition;
    }
    
    // 화살의 속도 조절하는 함수 
    void SetArrowSpeed(Arrow arrow)
    {
        arrow.speed = Random.Range(0.5f, 0.9f);
    }

    // 화살의 생성 위치 조절하는 함수 
    void SetArrowStartPosition(Arrow arrow)
    {
        arrow.startPosition = new Vector2(Random.Range(MIN_X, MAX_X), 5f);
    }

    // 화살 설정하는 함수
    void SetArrow(Arrow arrow)
    {
        if(SetArrowType(arrow) == 0) 
            SetArrowNum(arrow);
        else 
            SetArrowSymbol(arrow);
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
        arrowCnt++;
        return arrowObject;
    }

    // 현재 플레이어의 위치를 향해 오브젝트 날리기 
    void ShootArrow()
    {
        GameObject arrowObj = MakeArrow();
        Arrow arrow =  arrowObj.GetComponent<Arrow>();
        Rigidbody2D arrowRigid = arrowObj.GetComponent<Rigidbody2D>();
        arrowRigid.AddForce(arrow.direction * arrow.speed, ForceMode2D.Impulse);

        SpriteRenderer spriteRenderer = arrowObj.GetComponent<SpriteRenderer>();
        if(arrow.type == 0) 
            spriteRenderer.color = red;
        else 
            spriteRenderer.color = blue;
        
        Debug.LogFormat("------ShootArrow{0}------", arrowCnt);
        Debug.Log("type: " + arrow.type);
        Debug.Log("num: " + arrow.num);
        Debug.Log("symbol: " + arrow.mathematicalSymbol);
        Debug.Log("speed" + arrow.speed);
        Debug.Log("startPosition: " + arrow.startPosition.x + ", " + arrow.startPosition.y);
        Debug.Log("direction: " + arrow.direction);
        Debug.Log("-------------------------");

        Destroy(arrowObj, 3f);
    }
}
