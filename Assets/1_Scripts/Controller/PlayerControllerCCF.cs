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
        // anim = GetComponent<Animator>();     // <- �ִϸ��̼� �߰��� �ּ� ����
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

    #region �÷��̾� ���� ����
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
    #endregion
}
