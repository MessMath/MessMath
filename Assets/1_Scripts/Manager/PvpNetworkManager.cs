using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PvpNetworkManager : MonoBehaviourPunCallbacks
{
    PhotonView PV;

    public bool fullRoom;

    public void Connect()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = Managers.GoogleSignIn.GetUID();
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
        if (PhotonNetwork.IsMasterClient) { PhotonNetwork.Instantiate("Prefabs/RPCSychronizer", Vector3.zero, Quaternion.identity); }
        player.transform.localScale = new Vector3(1, 1, 1);
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.AutomaticallySyncScene = false;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.Disconnect();
        GameObject.Find("UI_PvpGameScene").GetComponent<UI_PvpGameScene>().PvpResult(true);
    }

}