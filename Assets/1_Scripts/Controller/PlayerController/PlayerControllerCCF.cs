using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class PlayerControllerCCF : MonoBehaviour
{
    public int Hp;
    public Vector2 inputVec;
    public float speed = 10;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    public GameObject onCalculateBoardText;
    bool isAlive = true;

    private JoyStickController joystick;

    void Awake()
    {
        Hp = 3;
        speed = 400;
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        joystick = GameObject.FindObjectOfType<JoyStickController>();
    }

    private void Update()
    {
        GameOverPopup();
        //StartCoroutine(Managers.Blessing.tempBlessing());
    }

    void FixedUpdate() 
    {
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
            MoveControl();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Arrow")
            return;
        Debug.Log("화살 맞았는데용");
        Arrow arrow = collision.gameObject.GetComponent<Arrow>();
        string symbol = arrow.tmp.text;
        Destroy(collision.gameObject);

        onCalculateBoardText.GetComponent<TextMeshProUGUI>().text += symbol;
    }

    private void MoveControl()
    {
        gameObject.GetComponent<RectTransform>().position += Vector3.up * speed * Time.deltaTime * joystick.Vertical;
        gameObject.GetComponent<RectTransform>().position += Vector3.right * speed * Time.deltaTime * joystick.Horizontal;
    }

    void OnMove(InputValue value)
    { 
        inputVec = value.Get<Vector2>();
    }

    public void GameOverPopup()
    {
        if (Hp <= 0 && isAlive == true)
        {
            Managers.UI.ShowPopupUI<UI_GameOver>();
            isAlive = false;
        }
    }

    public void BlinkPlayerImg()
    {
        StartCoroutine("Hit");
    }

    IEnumerator Hit()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.15f);
        int countTime = 0;
        while(countTime < 10)
        {
            if (countTime % 2 == 0)
                spriteRenderer.color = new Color32(255, 255, 255, 90);
            else
                spriteRenderer.color = new Color32(255, 255, 255, 180);

            yield return waitForSeconds;

            countTime++;
        }

        spriteRenderer.color = Color.white;
        yield return null;
    }
}
