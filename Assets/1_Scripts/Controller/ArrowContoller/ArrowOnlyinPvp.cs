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

    private void Awake()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        PV = GetComponent<PhotonView>();

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
        if(PV.IsMine && collision.CompareTag("Player"))
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
        else if ((transform.position - curPos).sqrMagnitude >= 100) transform.position = curPos;
        else transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);

        tmp.text = text;
    }

    [PunRPC]
    void DestroyRPC()
    {
        Destroy(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
            
            if(!isSet)      // 초기 값 설정
            {
                stream.SendNext(transform.rotation);
                stream.SendNext(tmp.transform.rotation);
                stream.SendNext(text);
                
                isSet = true;
            }

        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();

            if (!isSet)      // 초기 값 설정
            {
                transform.rotation = (Quaternion)stream.ReceiveNext();
                tmp.transform.rotation = (Quaternion)stream.ReceiveNext();
                text = (string)stream.ReceiveNext();

                isSet = true;
            }

        }
    }
}

