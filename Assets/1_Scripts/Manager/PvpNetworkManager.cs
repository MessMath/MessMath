using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PvpNetworkManager : MonoBehaviourPunCallbacks
{
    PhotonView PV;

    public bool fullRoom;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 2 };
        PhotonNetwork.JoinRandomOrCreateRoom(roomOptions: roomOptions);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 2 };
        PhotonNetwork.JoinRandomOrCreateRoom(roomOptions: roomOptions);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // TODO
        // 대기!
        // 플레이어가 둘이 찻다! -> Scene 이동
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                PhotonNetwork.LoadLevel("PvpGameScene");
            }
        }
    }

    public void Spawn()
    {
        GameObject player = PhotonNetwork.Instantiate("Prefabs/PlayerOnlyinPvp", new Vector2(Screen.width / 2f, Screen.height / 2f), Quaternion.identity);
        player.transform.localScale = new Vector3(1, 1, 1);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
    }
}