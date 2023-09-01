using JetBrains.Annotations;
using MessMathI18n;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        MyNickname,
        MyScore,
        OppsNickname,
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

    public Player OppPlayer;

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

        // 내 닉네임 가져오기
        GetText((int)Texts.MyNickname).text = Managers.UserMng.GetNickname();
        // 상대방 닉네임 가져오기 (DB를 참조해서)
        GetText((int)Texts.OppsNickname).text = Managers.DBManager.ReadData(OppPlayer.NickName, "nickname");

        Debug.Log($"<color=yellow> MyNickname : {Managers.UserMng.GetNickname()} </color>");
        Debug.Log($"<color=yellow> OppsNickname : {Managers.DBManager.ReadData(OppPlayer.NickName, "nickname")} </color>");

        ChangeScore();

        Exit();

        Time.timeScale = 0;
        GetComponent<Canvas>().sortingOrder = 10;

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

    void Exit()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.AutomaticallySyncScene = false;
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

    IEnumerator CountUp(float target, float current, TextMeshProUGUI tmp)
    {
        float duration = 3f; // 카운팅에 걸리는 시간 설정. 
        float offset = (target - current) / duration;

        while (current < target)
        {
            current += offset * Time.deltaTime;
            tmp.text = ((int)current).ToString();
            yield return null;

        }
        current = target;
        tmp.text = ((int)current).ToString();
    }

    void ChangeScore()
    {
        // 점수 등락
        int curScore = Managers.UserMng.GetScore();
        Managers.UserMng.SetUserScore(curScore + 100);

        // 점수 등락 시각적으로 표현
        StartCoroutine(CountUp(curScore + 100, curScore, GetText((int)Texts.MyScore)));
    }

}
