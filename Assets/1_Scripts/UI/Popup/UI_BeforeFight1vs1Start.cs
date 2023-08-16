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

        
        //CoroutineHandler.StartCoroutine(SceneChangeAnimation_Out());
        //Time.timeScale = 0;
        GetComponent<Canvas>().sortingOrder = 10;

        return true;
    }

    void reqQsts()
    {
        Time.timeScale = 1;
        Managers.Connector.Learning_GetQuestion();
        UI_Fight1Vs1Game.StartCoroutine("SetArrowGenerationTime", 1f);
        ClosePopupUI();
        UI_Fight1Vs1Game.Invoke("RefreshUI", 0.2f);
        UI_Fight1Vs1Game.GameStarted = true;
    }

    IEnumerator SceneChangeAnimation_Out()
    {
        // Ani
        UI_ChangeScenePopup uI_ChangeScenePopup = Managers.UI.ShowPopupUI<UI_ChangeScenePopup>();

        yield return new WaitForSeconds(1.3f);
        Managers.UI.ClosePopupUI(uI_ChangeScenePopup);
    }

}