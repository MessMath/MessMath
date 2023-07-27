using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// 오로지 PVP모드에서만 사용될 플레이어 코드
public class PlayerControllerOnlyinPvp : MonoBehaviourPun, IPunObservable
{
    public int _hp;
    public Vector2 _inputVec;
    public float _speed = 10;
    Rigidbody2D _rigid;
    Image _image;
    public TextMeshProUGUI _onCalculateBoard;

    bool _isAlive = true;

    // 멀티 관련 변수들
    PhotonView PV;
    Vector3 curPos;

    Color oppsColor = new Color(1f, 0.6f, 0.6f);
    Color myColor = new Color(0.6f, 0.6f, 1f);

    void Start()
    {
        _hp = 3;
        _speed = 400;
        _rigid = gameObject.GetOrAddComponent<Rigidbody2D>();
        _image = gameObject.GetOrAddComponent<Image>();
        PV = gameObject.GetComponent<PhotonView>();

        _onCalculateBoard = GameObject.Find("Calculate_BoardText").GetComponent<TextMeshProUGUI>();

        transform.SetParent(GameObject.Find("UI_PvpGameScene").transform);

        // 상대방과 나의 색 설정
        if (!PV.IsMine)
            _image.color = oppsColor;
        else 
            _image.color = myColor;
    }

    void FixedUpdate() 
    {
        if (PV.IsMine)
        {
            Vector2 nextVec = _inputVec * _speed * Time.fixedDeltaTime;
            _rigid.MovePosition(_rigid.position + nextVec);

            if (Managers.Game.Horizontal != 0 || Managers.Game.Vertical != 0)
                MoveControl();
        }

        // isMine이 아닌것들은 부드럽게 위치 동기화
        else if ((transform.position - curPos).sqrMagnitude >= 100) transform.position = curPos;
        else transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PV.IsMine) return;
        if (collision.gameObject.tag == "Arrow")
        {
            ArrowOnlyinPvp arrow = collision.gameObject.GetOrAddComponent<ArrowOnlyinPvp>();
            string symbol = arrow.text;

            _onCalculateBoard.text += symbol;
            transform.parent.GetComponent<UI_Scene>().Invoke("PreCalculate",0);
        }
    }

    private void MoveControl()
    {
        gameObject.GetComponent<RectTransform>().position += Vector3.up * _speed * Time.deltaTime * Managers.Game.Vertical;
        gameObject.GetComponent<RectTransform>().position += Vector3.right * _speed * Time.deltaTime * Managers.Game.Horizontal;
    }

    public void GameOverPopup()
    {
        if (_hp <= 0 && _isAlive == true)
        {
            Managers.UI.ShowPopupUI<UI_GameOver>();
            _isAlive = false;
        }
    }

    //위치 변수 동기화
    //위치동기화는 Photon Transform View보다 이렇게가 Better
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
        }
    }
}
