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

        return true;
    }

    public void restart()
    {
        SceneManager.LoadScene("CalculateScene");
        Time.timeScale = 1;
    }

    public void toMain()
    {
        SceneManager.LoadScene("MainScene");
        Time.timeScale = 1;
    }

}