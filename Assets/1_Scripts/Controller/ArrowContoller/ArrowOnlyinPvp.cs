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
    Quaternion textRot;
    bool isSet = false;
    RectTransform RT;

    private void Awake()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        PV = GetComponent<PhotonView>();
        RT = GetComponent<RectTransform>();

        transform.SetParent(GameObject.Find("ArrowController").transform);
    }

    private void Start()
    {
        tmp.text = text;
    }

    private void OnValidate()
    {
        if (tmp != null)
            tmp.text = text;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PV.IsMine && collision.CompareTag("Player"))
        {
            PV.RPC("DestroyRPC", RpcTarget.AllViaServer);
        }
        else if (collision.gameObject.tag == "DeadLine")
        {
            PV.RPC("DestroyRPC", RpcTarget.AllViaServer);
        }
    }

    private void Update()
    {
        if (PV.IsMine) return;

        // isMine이 아닌것들은 부드럽게 위치 동기화
        else if ((RT.position - curPos).sqrMagnitude >= 100) RT.position = curPos;
        else RT.position = Vector3.Lerp(RT.position, curPos, Time.deltaTime * 10);

        tmp.text = text;
    }

    [PunRPC]
    void DestroyRPC()
    {
        Destroy(gameObject);
    }

    public Vector3 Ratio()
    {
        int width = Screen.width;
        int height = Screen.height;

        float ratioX = transform.position.x / width;
        float ratioY = transform.position.y / height;

        return new Vector3(ratioX, ratioY, 0);
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

                isSet = true;
            }

        }
    }
}

