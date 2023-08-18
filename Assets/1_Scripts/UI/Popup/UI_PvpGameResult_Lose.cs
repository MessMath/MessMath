using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_PvpGameResult_Lose : UI_Popup
{
    public enum Buttons
    {
        NewMatchingBtn,
        BackToMainBtn,
    }

    public enum Texts
    {
        
    }
    
    public enum Images
    {

    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        GetButton((int)Buttons.NewMatchingBtn).gameObject.BindEvent(NewMatching);
        GetButton((int)Buttons.BackToMainBtn).gameObject.BindEvent(toMain);

        Managers.Sound.Play("DefeatEff");

        Time.timeScale = 0;
        GetComponent<Canvas>().sortingOrder = 10;

        return true;
    }

    public void NewMatching()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.AutomaticallySyncScene = false;

        // Sound
        Managers.Sound.Play("ClickBtnEff");

        Managers.Scene.ChangeScene(Define.Scene.PvpMatchingScene);
        Time.timeScale = 1;
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
