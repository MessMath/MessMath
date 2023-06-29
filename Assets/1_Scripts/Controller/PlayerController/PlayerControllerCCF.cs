using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControllerCCF : UI_Base
{
    public int _hp;
    public Vector2 _inputVec;
    public float _speed = 10;
    Rigidbody2D _rigid;
    Image _image;
    public GameObject _onCalculateBoard;
    public UI_Fight1vs1Game _fight1vs1sceneUi;

    bool _isAlive = true;

    void Start()
    {
        _hp = 3;
        _speed = 400;
        _rigid = gameObject.GetOrAddComponent<Rigidbody2D>();
        _image = gameObject.GetOrAddComponent<Image>();

        if (Managers.Scene.CurrentSceneType == Define.Scene.StoryGameScene)
        {
            _onCalculateBoard = gameObject.transform.parent.GetComponentInChildren<TextMeshProUGUI>().gameObject;
        }
        else
        {
            _onCalculateBoard = gameObject.transform.parent.GetComponentInChildren<TEXDraw>().gameObject;
            _fight1vs1sceneUi = GameObject.Find("UI_Fight1vs1Game").GetComponent<UI_Fight1vs1Game>();
        }


    }

    private void Update()
    {
        GameOverPopup();
        //StartCoroutine(Managers.Blessing.tempBlessing());
    }

    void FixedUpdate() 
    {
        Vector2 nextVec = _inputVec * _speed * Time.fixedDeltaTime;
        _rigid.MovePosition(_rigid.position + nextVec);

        if (Managers.Game.Horizontal != 0 || Managers.Game.Vertical != 0)
            MoveControl();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어가 화살과 충돌하지 않도록 할 때
        if(Managers.Grace.playerCollisionOff)
        {
            Destroy(collision.gameObject);
            return;
        }

        if (collision.gameObject.tag == "Arrow")
        {
            Debug.Log("Hit Arrow!");
            Arrow arrow = collision.gameObject.GetOrAddComponent<Arrow>();
            string symbol = arrow.gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
            Destroy(collision.gameObject);

            _onCalculateBoard.GetComponent<TextMeshProUGUI>().text += symbol;
        }
        else if(collision.gameObject.tag == "ArrowOnlyin1vs1")
        {
            ArrowOnlyin1vs1 arrow = collision.GetComponent<ArrowOnlyin1vs1>();
            _fight1vs1sceneUi.wj_sample1vs1.SelectAnswer(arrow.text);
            Destroy(collision.gameObject);
        }

    }

    private void MoveControl()
    {
        gameObject.GetComponent<RectTransform>().position += Vector3.up * _speed * Time.deltaTime * Managers.Game.Vertical;
        gameObject.GetComponent<RectTransform>().position += Vector3.right * _speed * Time.deltaTime * Managers.Game.Horizontal;
    }

    void OnMove(InputValue value)
    { 
        _inputVec = value.Get<Vector2>();
    }

    public void GameOverPopup()
    {
        if (_hp <= 0 && _isAlive == true)
        {
            Managers.UI.ShowPopupUI<UI_GameOver>();
            _isAlive = false;
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
                _image.color = new Color32(255, 255, 255, 90);
            else
                _image.color = new Color32(255, 255, 255, 180);

            yield return waitForSeconds;

            countTime++;
        }

        _image.color = Color.white;
        yield return null;
    }
}
