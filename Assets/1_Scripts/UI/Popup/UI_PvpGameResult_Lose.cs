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

        PhotonNetwork.Disconnect();
        PhotonNetwork.AutomaticallySyncScene = false;

        return true;
    }

    public void NewMatching()
    {
        // Sound
        Managers.Sound.Play("ClickBtnEff");

        Managers.Scene.ChangeScene(Define.Scene.PvpMatchingScene);
        Time.timeScale = 1;
    }

    public void toMain()
    {
        // Sound
        Managers.Sound.Play("ClickBtnEff");

        CoroutineHandler.StartCoroutine(SceneChangeAnimation_In_Lobby());
        Time.timeScale = 1;
    }

    IEnumerator SceneChangeAnimation_In_Lobby()
    {
        // Ani
        UI_LockTouch uI_LockTouch = Managers.UI.ShowPopupUI<UI_LockTouch>();
        SceneChangeAnimation_Out anim = Managers.Resource.Instantiate("Animation/SceneChangeAnimation_In").GetOrAddComponent<SceneChangeAnimation_Out>();
        anim.transform.SetParent(this.transform);
        anim.SetInfo(Define.Scene.LobbyScene, () => { });

        yield return new WaitForSeconds(0.3f);
        Managers.UI.ClosePopupUI(uI_LockTouch);

        Managers.Sound.Play("ClickBtnEff");
        Managers.Scene.ChangeScene(Define.Scene.LobbyScene);
    }
}
