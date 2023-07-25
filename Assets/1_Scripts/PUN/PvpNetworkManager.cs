using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PvpNetworkManager : MonoBehaviourPunCallbacks
{
    PhotonView PV;

    void Awake()
    {
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
    }

    private void Start()
    {
        Connect();
    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        PhotonNetwork.JoinOrCreateRoom("Room", roomOptions, null);
    }

    public override void OnJoinedRoom()
    {
        Spawn();
    }

    void Spawn()
    {
        Transform ui = transform.parent;
        PhotonNetwork.Instantiate("Prefabs/PlayerOnlyinPvp", new Vector2(1600, 720), Quaternion.identity);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
    }

}