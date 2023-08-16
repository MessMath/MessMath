using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_GameWin : UI_Popup
{ 
    public enum Buttons
    {
        RestartBtn,
        ToMainBtn,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.RestartBtn).gameObject.BindEvent(() => restart());
        GetButton((int)Buttons.ToMainBtn).gameObject.BindEvent(() => toMain());

        Time.timeScale = 0;
        GetComponent<Canvas>().sortingOrder = 10;

        // Sound
        Managers.Sound.Play("ClearEff");

        return true;
    }

    public void restart()
    {
        // Sound
        Managers.Sound.Play("ClickBtnEff");
        
        SceneManager.LoadScene(Managers.Scene.GetSceneName(Managers.Scene.CurrentSceneType));

        Time.timeScale = 1;
    }

    public void toMain()
    {
        CoroutineHandler.StartCoroutine(SceneChangeAnimation_In_Lobby());

        Time.timeScale = 1;
    }

    IEnumerator SceneChangeAnimation_In_Lobby()
    {
        // Ani
        UI_ChangeScenePopup uI_ChangeScenePopup = Managers.UI.ShowPopupUI<UI_ChangeScenePopup>();

        yield return new WaitForSeconds(1.3f);
        Managers.UI.ClosePopupUI(uI_ChangeScenePopup);

        Managers.Sound.Play("ClickBtnEff");
        Managers.Scene.ChangeScene(Define.Scene.LobbyScene);
    }

}