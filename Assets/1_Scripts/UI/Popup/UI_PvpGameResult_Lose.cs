﻿using MessMathI18n;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UI_PvpGameResult_Win;

public class UI_PvpGameResult_Lose : UI_Popup
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
        Lose,
        Lose1,
        Players_Illust,
        Opps_Illust,
        OppsTier,
    }

    public enum GameObjects
    {
        MyResult,
        OppsResult,
    }

    public string PlayerName;
    public int PlayerScore;
    public string PlayerClothes;

    public Player OppPlayer;
    public string OppPlayersName;
    public int OppPlayersScore;
    public string OppPlayersCloth;

    public int DecreasingScore = -8;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindObject(typeof(GameObjects));

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

        InitPlayerInfo();

        // 내 닉네임 가져오기
        GetText((int)Texts.MyNickname).text = PlayerName;

        // 상대방의 Score (Tier)
        GetImage((int)Images.OppsTier).sprite = Managers.Resource.Load<Sprite>("Sprites/Tier/T" + ((OppPlayersScore / 100) + 1).ToString());
        // 상대방의 옷
        GetImage((int)Images.Opps_Illust).sprite = Managers.Resource.Load<Sprite>("Sprites/Clothes/" + OppPlayersCloth + "_full");
        // 상대방의 하트갯수
        CopyAllChildren(GameObject.Find("OppsScores"), GetObject((int)GameObjects.OppsResult));

        // 나의 Score
        ChangeScore();
        // 나의 하트갯수
        CopyAllChildren(GameObject.Find("MyScores"), GetObject((int)GameObjects.MyResult));

        Exit();

        GetComponent<Canvas>().sortingOrder = 10;

        return true;
    }

    async void InitPlayerInfo()
    {
        PlayerName = await Managers.DBManager.GetNickName(Managers.GoogleSignIn.GetUID());
        // 내 닉네임 가져오기
        GetText((int)Texts.MyNickname).text = PlayerName;

        PlayerClothes = await Managers.DBManager.GetMyClothes(Managers.GoogleSignIn.GetUID());
        // 나의 옷
        GetImage((int)Images.Players_Illust).sprite = Managers.Resource.Load<Sprite>("Sprites/Clothes/" + PlayerClothes + "_full");
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

    IEnumerator CountDown(float target, float current, TextMeshProUGUI tmp)
    {
        tmp.text = ((int)current).ToString();

        yield return new WaitForSeconds(1f);

        float duration = 3f; // 카운팅에 걸리는 시간 설정. 
        float offset = (current - target) / duration;

        while (current > target)
        {
            current -= offset * Time.deltaTime;
            tmp.text = ((int)current).ToString();
            yield return null;
        }
        current = target;
        tmp.text = ((int)current).ToString();
    }

    void ChangeScore()
    {
        int curScore = 0; ;

        // 점수 등락
        var gettingScore = Managers.DBManager.GetScore(Managers.GoogleSignIn.GetUID()).GetAwaiter();
        gettingScore.OnCompleted(() => {

            curScore = gettingScore.GetResult();

        });

        if ((curScore / 100) > (OppPlayersScore / 100))
            DecreasingScore -= 6;

        int resultScore = ((curScore + DecreasingScore) < 0) ? 0 : (curScore + DecreasingScore);
        Managers.DBManager.SetScore(resultScore);

        // 점수 등락 시각적으로 표현
        StartCoroutine(CountDown(resultScore, curScore, GetText((int)Texts.MyScore)));
    }

    public void CopyAllChildren(GameObject sourceObject, GameObject targetObject)
    {
        int childCount = sourceObject.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = sourceObject.transform.GetChild(i);
            GameObject copiedChild = Instantiate(child.gameObject, targetObject.transform);
        }
    }

}
