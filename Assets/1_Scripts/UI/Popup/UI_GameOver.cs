using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_GameOver : UI_Popup
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
        Managers.Sound.Play("DefeatEff");

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
        // Sound
        Managers.Sound.Play("ClickBtnEff");

        Managers.Scene.ChangeScene(Define.Scene.LobbyScene);
        Time.timeScale = 1;
    }

}