using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RPCSychronizer : MonoBehaviourPun
{
    UI_PvpGameScene ui_PvpGameScene;

    void Start()
    {
        ui_PvpGameScene = GameObject.Find("UI_PvpGameScene").GetComponent<UI_PvpGameScene>();
    }

    [PunRPC]
    public void Answer(string printResult, int QusetionNumber, int Who)
    {
        if (int.Parse(printResult) == QusetionNumber && Who == 1)
        {
            ui_PvpGameScene._player1Score++;
            ui_PvpGameScene.ScoreSet();
            ui_PvpGameScene.Questioning();
        }
        else if (int.Parse(printResult) == QusetionNumber && Who == 2)
        {
            ui_PvpGameScene._player2Score++;
            ui_PvpGameScene.ScoreSet();
            ui_PvpGameScene.Questioning();
        }
    }
}
