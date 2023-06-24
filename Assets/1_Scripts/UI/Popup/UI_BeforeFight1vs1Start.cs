using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_BeforeFight1vs1Start : UI_Popup
{
    public enum Buttons
    {
        reqQstsBtn,
    }

    public UI_Fight1vs1Game UI_Fight1Vs1Game;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.reqQstsBtn).gameObject.BindEvent(() => reqQsts());

        Time.timeScale = 0;
        GetComponent<Canvas>().sortingOrder = 10;

        return true;
    }

    void reqQsts()
    {
        Time.timeScale = 1;
        Managers.Connector.Learning_GetQuestion();
        UI_Fight1Vs1Game.StartCoroutine("SetArrowGenerationTime", 1f);
        ClosePopupUI();
        UI_Fight1Vs1Game.GameStarted = true;
    }
}