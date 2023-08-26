using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_GameOver : UI_Popup
{
    public enum Buttons
    {
        RechallengeBtn,
        BackToLobbyBtn,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.RechallengeBtn).gameObject.BindEvent(Rechallenge);
        GetButton((int)Buttons.BackToLobbyBtn).gameObject.BindEvent(BackToLobby);

        Time.timeScale = 0;
        GetComponent<Canvas>().sortingOrder = 10;

        // Sound
        Managers.Sound.Play("DefeatEff");

        return true;
    }

    public void Rechallenge()
    {
        
        // Sound
        Managers.Sound.Play("ClickBtnEff");
        
        SceneManager.LoadScene(Managers.Scene.GetSceneName(Managers.Scene.CurrentSceneType));

        Time.timeScale = 1;
    }

    public void BackToLobby()
    {
        CoroutineHandler.StartCoroutine(SceneChangeAnimation_In_Lobby());
        Time.timeScale = 1;
    }

    IEnumerator SceneChangeAnimation_In_Lobby()
    {
        Managers.Sound.Play("ClickBtnEff");

        // Ani
        UI_LockTouch uI_LockTouch = Managers.UI.ShowPopupUI<UI_LockTouch>();
        SceneChangeAnimation_In anim = Managers.Resource.Instantiate("Animation/SceneChangeAnimation_In").GetOrAddComponent<SceneChangeAnimation_In>();
        anim.transform.SetParent(this.transform);
        anim.SetInfo(Define.Scene.LobbyScene, () => { Managers.Scene.ChangeScene(Define.Scene.LobbyScene); });

        yield return new WaitForSeconds(0.5f);
    }

}