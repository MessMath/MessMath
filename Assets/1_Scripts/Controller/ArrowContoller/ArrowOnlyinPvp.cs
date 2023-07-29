using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[Serializable]
public class ArrowOnlyinPvp : MonoBehaviourPun, IPunObservable
{
    [field: SerializeField]
    public int type { get; set; }
    //public int num  {get; set;}
    //public char mathematicalSymbol  {get; set;}
    [field: SerializeField]
    public float speed { get; set; }
    [field: SerializeField]
    public Vector2 startPosition { get; set; }
    [field: SerializeField]
    public Vector2 direction { get; set; }
    //[field: SerializeField]
    //public TextMeshProUGUI tmp { get; set; }
    public string text;
    public TextMeshProUGUI tmp;            // 화살의 Symbol이 표시될 TextMeshPro

    PhotonView PV;
    Vector3 curPos;
    bool isSet = false;
    RectTransform RT;

    // 추가된 변수
    private Vector3 predictedPosition;
    private float lastUpdateTime;
    private float updateInterval = 0.1f; // 예측 업데이트 간격

    private void Awake()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        PV = GetComponent<PhotonView>();
        RT = GetComponent<RectTransform>();

        RT.SetParent(GameObject.Find("ArrowController").transform);
    }

    private void Start()
    {
        if (PV.IsMine)
        {
            // 텍스트를 초기 값으로 설정
            tmp.text = text;
        }
    }

    private void OnValidate()
    {
        if (tmp != null)
            tmp.text = text;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeadLine"))
        {
            PV.RPC("DestroyRPC", RpcTarget.AllViaServer);
        }
    }

    float initialPositionDelay = 0.5f; // 초기 위치 설정을 지연할 시간(초)
    float initialPositionTimer = 0f;

    private void Update()
    {
        if (PV.IsMine) return;

        if (initialPositionTimer < initialPositionDelay)
    {
            RT.position = curPos;
            initialPositionTimer += Time.deltaTime;
        }
    else
        {
            if (Time.time - lastUpdateTime > updateInterval)
        {
                predictedPosition = curPos + (curPos - RT.position);
                lastUpdateTime = Time.time;
            }
            RT.position = Vector3.Slerp(RT.position, predictedPosition, Time.deltaTime / updateInterval);
        }
    }

    [PunRPC]
    void DestroyRPC()
    {
        Destroy(gameObject);
    }

    Vector3 transPosIntoRatio()
    {
        float actualH = Screen.width / 3200f * 1440f;
        float xRatio = RT.position.x / Screen.width;
        float yRatio = (RT.position.y - ((Screen.height - actualH) / 2f)) / actualH;

        return new Vector3(xRatio, yRatio, 0f);
    }

    void transRatioIntoPos(Vector3 vector3)
    {
        float xRatio = vector3.x;
        float yRatio = vector3.y;
        float actualH = Screen.width / 3200f * 1440f;

        float xPos = Screen.width * xRatio;
        float yPos = actualH * yRatio + ((Screen.height - actualH) / 2f);

        curPos = new Vector3(xPos, yPos, 0f);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transPosIntoRatio());

            if (!isSet)      // 초기 값 설정
            {
                stream.SendNext(RT.localScale);
                stream.SendNext(RT.rotation);
                stream.SendNext(tmp.GetComponent<RectTransform>().rotation);
                stream.SendNext(text);

                isSet = true;
            }

        }
        else
        {
            transRatioIntoPos((Vector3)stream.ReceiveNext());

            if (!isSet)      // 초기 값 설정
            {
                RT.localScale = (Vector3)stream.ReceiveNext();
                RT.rotation = (Quaternion)stream.ReceiveNext();
                tmp.GetComponent<RectTransform>().rotation = (Quaternion)stream.ReceiveNext();
                text = (string)stream.ReceiveNext();

                tmp.text = text;

                isSet = true;
            }

            // 예측 위치 업데이트
            predictedPosition = curPos + (curPos - RT.position);
            lastUpdateTime = Time.time;
        }
    }
}

