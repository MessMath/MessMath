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
    public RectTransform _rectTransform;

    bool _isAlive = true;

    // 멀티 관련 변수들
    public PhotonView PV;
    bool isSet = false;
    Vector3 curPos;

    Color oppsColor = new Color(1f, 0.6f, 0.6f);
    Color myColor = new Color(0.6f, 0.6f, 1f);

    // 추가된 변수
    private Vector3 predictedPosition;
    private float lastUpdateTime;
    private float updateInterval = 0.1f; // 예측 업데이트 간격

    float referenceWidth = 3200f; // 기준 해상도의 너비
    float referenceHeight = 1440f; // 기준 해상도의 높이
    float currentWidth = Screen.width; // 현재 화면의 너비
    float currentHeight = Screen.height; // 현재 화면의 높이

    float widthRatio;
    float heightRatio;
    float adjustedSpeed;

    void Awake()
    {
        _hp = 3;
        _speed = 400;
        _rigid = gameObject.GetOrAddComponent<Rigidbody2D>();
        _image = gameObject.GetOrAddComponent<Image>();
        PV = gameObject.GetComponent<PhotonView>();

        _onCalculateBoard = GameObject.Find("Calculate_BoardText").GetComponent<TextMeshProUGUI>();
        _rectTransform = GetComponent<RectTransform>();

        transform.SetParent(GameObject.Find("UI_PvpGameScene").transform);
        _rectTransform.anchoredPosition = Vector3.zero;

        // 상대방과 나의 색 설정
        _image.color = !PV.IsMine ? oppsColor : myColor;

        // 속도를 해상도에 맞춰 조절
        widthRatio = currentWidth / referenceWidth;
        heightRatio = currentHeight / referenceHeight;

        adjustedSpeed = _speed * Mathf.Min(widthRatio, heightRatio);
    }

    void Update()
    {
        if (PV.IsMine)
        {
            Vector2 nextVec = _inputVec * adjustedSpeed * Time.fixedDeltaTime;
            _rigid.MovePosition(_rigid.position + nextVec);

            if (Managers.Game.Horizontal != 0 || Managers.Game.Vertical != 0)
                MoveControl();
        }
        else
        {
            // 예측 위치로 부드럽게 이동
            if (Time.time - lastUpdateTime > updateInterval)
            {
                predictedPosition = curPos + (curPos - _rectTransform.position);
                lastUpdateTime = Time.time;
            }
            _rectTransform.position = Vector3.Lerp(_rectTransform.position, predictedPosition, Time.deltaTime / updateInterval);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PV.IsMine) return;
        if (collision.CompareTag("Arrow"))
        {
            ArrowOnlyinPvp arrow = collision.gameObject.GetOrAddComponent<ArrowOnlyinPvp>();
            string symbol = arrow.text;
            collision.gameObject.GetPhotonView().RPC("DestroyRPC", RpcTarget.AllViaServer);

            _onCalculateBoard.text += symbol;
            transform.parent.GetComponent<UI_Scene>().Invoke("PreCalculate", 0);
        }
    }

    private void MoveControl()
    {
        gameObject.GetComponent<RectTransform>().position += Vector3.up * adjustedSpeed * Time.deltaTime * Managers.Game.Vertical;
        gameObject.GetComponent<RectTransform>().position += Vector3.right * adjustedSpeed * Time.deltaTime * Managers.Game.Horizontal;
    }

    public void GameOverPopup()
    {
        if (_hp <= 0 && _isAlive == true)
        {
            Managers.UI.ShowPopupUI<UI_GameOver>();
            _isAlive = false;
        }
    }

    Vector3 transPosIntoRatio()
    {
        float actualH = Screen.width / 3200f * 1440f;
        float xRatio = _rectTransform.position.x / Screen.width;
        float yRatio = (_rectTransform.position.y - ((Screen.height - actualH) / 2f)) / actualH;

        return new Vector3(xRatio, yRatio, 0f);
    }

    void transRatioIntoPos(Vector3 vector3)
    {
        float xRatio = vector3.x;
        float yRatio = vector3.y;
        float actualH = Screen.width / 3200f * 1440f;

        float xPos = Screen.width * xRatio;
        float yPos = actualH * yRatio + ((Screen.height - actualH)/2f);

        curPos = new Vector3(xPos, yPos, 0f);
    }

    //위치 변수 동기화
    //위치동기화는 Photon Transform View보다 이렇게가 Better
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transPosIntoRatio());

            if (!isSet)
            {
                stream.SendNext(_rectTransform.localScale);
                isSet = true;   
            }

        }
        else
        {
            transRatioIntoPos((Vector3)stream.ReceiveNext());

            if (!isSet)
            {
                _rectTransform.localScale = (Vector3)stream.ReceiveNext();
                isSet = true;
            }

            // 예측 위치 업데이트
            predictedPosition = curPos + (curPos - _rectTransform.position);
            lastUpdateTime = Time.time;
        }
    }
}
