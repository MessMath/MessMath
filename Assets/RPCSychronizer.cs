using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RPCSychronizer : MonoBehaviourPun
{
    UI_PvpGameScene uI_PvpGameScene;

    void Start()
    {
        uI_PvpGameScene = GameObject.Find("UI_PvpGameScene").GetComponent<UI_PvpGameScene>();
    }

    [PunRPC]
    public void Answer(string printResult, int QusetionNumber, int Who)
    {
        if (int.Parse(printResult) == QusetionNumber && Who == 1)
        {
            uI_PvpGameScene._player1Score++;
            uI_PvpGameScene.ScoreSet();
            uI_PvpGameScene.Questioning();
        }
        else if (int.Parse(printResult) == QusetionNumber && Who == 2)
        {
            uI_PvpGameScene._player2Score++;
            uI_PvpGameScene.ScoreSet();
            uI_PvpGameScene.Questioning();
        }
    }
}
