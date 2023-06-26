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

        return true;
    }

    public void restart()
    {
        //SceneManager.LoadScene("StoryGameScene");

        // 아마 위 코드는 나중에 문제를 일으킬 거 같음. 
        // 이 팝업을 Fight1vs1Game에서도, StoryGame에서도 부르니까.
        // 그래서 아마도 필요할 것 같아서 아래 코드를 추가해놓았다.
        // 아래 코드가 필요하다면 대신해서 쓰자.
        // 이 코드의 의미는 "현재 Scene을 다시 로드한다"는 의미다.
        SceneManager.LoadScene(Managers.Scene.GetSceneName(Managers.Scene.CurrentSceneType));

        Time.timeScale = 1;
    }

    public void toMain()
    {
        SceneManager.LoadScene("LobbyScene");
        Time.timeScale = 1;
    }

}