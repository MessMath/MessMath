using MessMathI18n;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_PvpGameResult_Lose : UI_Popup
{
    public enum Buttons
    {
        ReMatchBtn,
        BackToLobbyBtn,
    }

    public enum Texts
    {
        
    }
    
    public enum Images
    {
        ReMatchBtn,
        BackToLobbyBtn,
        Lose,
        Lose1,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        GetButton((int)Buttons.ReMatchBtn).gameObject.BindEvent(ReMatch);
        GetButton((int)Buttons.BackToLobbyBtn).gameObject.BindEvent(ToLobby);

        if(LocalizationManager.Get().GetSelectedLanguage() == Language.ENGLISH)
        {
            GetImage((int)Images.ReMatchBtn).sprite = Managers.Resource.Load<Sprite>("Sprites/Pvp/ResultPopup/Rematch_ENG");
            GetImage((int)Images.BackToLobbyBtn).sprite = Managers.Resource.Load<Sprite>("Sprites/Pvp/ResultPopup/BackToLobby_ENG");
            GetImage((int)Images.Lose).sprite = Managers.Resource.Load<Sprite>("Sprites/Pvp/ResultPopup/Defeat_ENG");
            GetImage((int)Images.Lose1).sprite = Managers.Resource.Load<Sprite>("Sprites/Pvp/ResultPopup/Defeat2_ENG");
        }

        Managers.Sound.Play("DefeatEff");

        Time.timeScale = 0;
        GetComponent<Canvas>().sortingOrder = 10;

        PhotonNetwork.Disconnect();
        PhotonNetwork.AutomaticallySyncScene = false;

        return true;
    }

    public void ReMatch()
    {
        // Sound
        Managers.Sound.Play("ClickBtnEff");

        CoroutineHandler.StartCoroutine(SceneChangeAnimation(Define.Scene.PvpMatchingScene));
        Time.timeScale = 1;
    }

    public void ToLobby()
    {
        // Sound
        Managers.Sound.Play("ClickBtnEff");

        CoroutineHandler.StartCoroutine(SceneChangeAnimation(Define.Scene.LobbyScene));
        Time.timeScale = 1;
    }

    IEnumerator SceneChangeAnimation(Define.Scene Scene)
    {
        // Ani
        UI_LockTouch uI_LockTouch = Managers.UI.ShowPopupUI<UI_LockTouch>();
        SceneChangeAnimation_Out anim = Managers.Resource.Instantiate("Animation/SceneChangeAnimation_In").GetOrAddComponent<SceneChangeAnimation_Out>();
        anim.transform.SetParent(this.transform);
        anim.SetInfo(Scene, () => { });

        yield return new WaitForSeconds(0.3f);
        Managers.UI.ClosePopupUI(uI_LockTouch);

        Managers.Sound.Play("ClickBtnEff");
        Managers.Scene.ChangeScene(Scene);
    }
}
