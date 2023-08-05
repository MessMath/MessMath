using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_PvpGameResult : UI_Popup
{
    public bool _isWin = false;
    public enum Buttons
    {
        ToMainBtn,
    }

    public enum Texts
    {
        ResultText,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetButton((int)Buttons.ToMainBtn).gameObject.BindEvent(() => toMain());
        ResultText();

        Time.timeScale = 0;
        GetComponent<Canvas>().sortingOrder = 10;

        return true;
    }

    void ResultText()
    {
        if (_isWin)
        {
            GetText((int)Texts.ResultText).text = "Win";
        }
        else if (!_isWin)
        {
            GetText((int)Texts.ResultText).text = "Defeat";
        }
    }

    public void toMain()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.AutomaticallySyncScene = false;

        // Sound
        Managers.Sound.Play("ClickBtnEff");

        Managers.Scene.ChangeScene(Define.Scene.LobbyScene);
        Time.timeScale = 1;
    }
}
