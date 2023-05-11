using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerControllerCCF : MonoBehaviour
{
    public int Hp;
    public Vector2 inputVec;
    public float speed = 3;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    public GameObject onCalculateBoardText;

    void Awake()
    {
        Hp = 3;
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        // anim = GetComponent<Animator>();     // <- 애니메이션 추가시 주석 해제
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Arrow")
            return;

        Arrow arrow = collision.gameObject.GetComponentInParent<Arrow>();
        string symbol = arrow.tmp.text;
        Destroy(collision.transform.parent.gameObject);

        onCalculateBoardText.GetComponent<TextMeshProUGUI>().text += symbol;
    }

    #region 플레이어 조작 관련
    void FixedUpdate() 
    {
        // 플레이어의 위치 이동
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        // fixedDeltaTime : 물리 프레임 하나가 소비한 시간
        // 모든 경우의 프레임에서 동일한 움직임을 갖기위한 코드 
        rigid.MovePosition(rigid.position + nextVec); // 현재 위치 + 움직이는 벡터값
    }
    void OnMove(InputValue value)
    { // Player Input을 통해 WASD입력값을 받는다(normalized된 벡터값을 inputVec에 저장)
        inputVec = value.Get<Vector2>();
    }
    void LateUpdate()
    {
        //anim.SetFloat("Speed", inputVec.magnitude);   // <- 애니메이션 추가시 주석 해제

        if (inputVec.x != 0)
        {
            spriter.flipX = (inputVec.x < 0);
        }
    }
    #endregion
}
