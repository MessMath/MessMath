using JetBrains.Annotations;
using MessMathI18n;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PvpGameResult_Win : UI_Popup
{
    public enum Buttons
    {
        ReMatchBtn,
        BackToLobbyBtn,
    }

    public enum Texts
    {
        // TODO
        // 결과 통계 텍스트 만들어서,
        // 바인드하고,
        // 실제 경기에 대한 기록을
        // 시각화 해야 한다.
    }
    
    public enum Images
    {
        ReMatchBtn,
        BackToLobbyBtn,
        Win,
        Win1,
        Players_Illust,
        Opps_Illust,
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

        if (LocalizationManager.Get().GetSelectedLanguage() == Language.ENGLISH)
        {
            GetImage((int)Images.ReMatchBtn).sprite = Managers.Resource.Load<Sprite>("Sprites/Pvp/ResultPopup/Rematch_ENG");
            GetImage((int)Images.BackToLobbyBtn).sprite = Managers.Resource.Load<Sprite>("Sprites/Pvp/ResultPopup/BackToLobby_ENG");
            GetImage((int)Images.Win).sprite = Managers.Resource.Load<Sprite>("Sprites/Pvp/ResultPopup/Victory_ENG");
            GetImage((int)Images.Win1).sprite = Managers.Resource.Load<Sprite>("Sprites/Pvp/ResultPopup/Victory2_ENG");
        }

        Managers.Sound.Play("ClearEff");

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
