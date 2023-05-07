using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerControllerCCF : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed = 3;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    public GameObject onCalculateBoardText;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        // anim = GetComponent<Animator>();     // <- �ִϸ��̼� �߰��� �ּ� ����
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "SymbolArrow")
            return;

        GameObject arrow = collision.gameObject;
        string symbol = arrow.GetComponent<ArrowController>().Symbol;
        Destroy(collision.gameObject);

        onCalculateBoardText.GetComponent<TextMeshProUGUI>().text += symbol;
    }

    void FixedUpdate() 
    {
        // �÷��̾��� ��ġ �̵�
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        // fixedDeltaTime : ���� ������ �ϳ��� �Һ��� �ð�
        // ��� ����� �����ӿ��� ������ �������� �������� �ڵ� 
        rigid.MovePosition(rigid.position + nextVec); // ���� ��ġ + �����̴� ���Ͱ�
    }
    void OnMove(InputValue value)
    { // Player Input�� ���� WASD�Է°��� �޴´�(normalized�� ���Ͱ��� inputVec�� ����)
        inputVec = value.Get<Vector2>();
    }
    void LateUpdate()
    {
        //anim.SetFloat("Speed", inputVec.magnitude);   // <- �ִϸ��̼� �߰��� �ּ� ����

        if (inputVec.x != 0)
        {
            spriter.flipX = (inputVec.x < 0);
        }
    }
    
}
