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

        GetButton((int)Buttons.ToMainBtn).gameObject.BindEvent(toMain);
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
            // Sound
            Managers.Sound.Play("ClearEff");
        }
        else if (!_isWin)
        {
            GetText((int)Texts.ResultText).text = "Defeat";
            // Sound
            Managers.Sound.Play("DefeatEff");
        }
    }

    public void toMain()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.AutomaticallySyncScene = false;

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
