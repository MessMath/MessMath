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

        // �Ƹ� �� �ڵ�� ���߿� ������ ����ų �� ����. 
        // �� �˾��� Fight1vs1Game������, StoryGame������ �θ��ϱ�.
        // �׷��� �Ƹ��� �ʿ��� �� ���Ƽ� �Ʒ� �ڵ带 �߰��س��Ҵ�.
        // �Ʒ� �ڵ尡 �ʿ��ϴٸ� ����ؼ� ����.
        // �� �ڵ��� �ǹ̴� "���� Scene�� �ٽ� �ε��Ѵ�"�� �ǹ̴�.
        SceneManager.LoadScene(Managers.Scene.GetSceneName(Managers.Scene.CurrentSceneType));

        Time.timeScale = 1;
    }

    public void toMain()
    {
        SceneManager.LoadScene("LobbyScene");
        Time.timeScale = 1;
    }

}