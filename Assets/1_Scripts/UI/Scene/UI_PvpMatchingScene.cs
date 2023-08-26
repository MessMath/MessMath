using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Windows;
using System.Data;
using System;
using TMPro;
using System.IO;
using Photon.Pun;
using Unity.VisualScripting;

public class UI_PvpMatchingScene : UI_Scene
{
    enum Texts
    {
        
    }

    enum Buttons
    {
        BackBtn,
    }

    enum Images
    {
        PlayerImageBackground,
        AnemyImageBackground,
        AnemyGraceBackground,
        PlayerGraceBackground,
        DecoImage,
        MatchingTime,
        PlayerImage,
        AnemyImage,
        FightImage,
    }

    enum GameObjects
    {

    }

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        // 서버에 연결!
        Managers.Network.Connect();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));
        BindImage(typeof(Images));

        GetButton((int)Buttons.BackBtn).gameObject.BindEvent(toMain);

        GetImage((int)Images.DecoImage).gameObject.SetActive(false);

        Managers.Sound.Clear();

        StartCoroutine(SceneChangeAnimation_Out());
        StartCoroutine("MatchingAni");
        return true;
    }

    public void toMain()
    {
        // Sound
        Managers.Sound.Play("ClickBtnEff");

        PhotonNetwork.Disconnect();
        PhotonNetwork.AutomaticallySyncScene = false;

        StartCoroutine(SceneChangeAnimation(Define.Scene.LobbyScene));
    }

    IEnumerator MatchingAni()
    {
        Managers.Sound.Play("휙");
        GetImage((int)Images.PlayerImageBackground).gameObject.GetOrAddComponent<Animator>().Play("MatchingPlayerAni");
        yield return new WaitForSeconds(0.1f);
        Managers.Sound.Play("휙");
        GetImage((int)Images.AnemyImageBackground).gameObject.GetOrAddComponent<Animator>().Play("MatchingAnemyPlayerAni");
        yield return new WaitForSeconds(0.1f);
        Managers.Sound.Play("휙");
        GetImage((int)Images.AnemyGraceBackground).gameObject.GetOrAddComponent<Animator>().Play("MatchingAnemyGraceAni");
        yield return new WaitForSeconds(0.1f);
        Managers.Sound.Play("휙");
        GetImage((int)Images.PlayerGraceBackground).gameObject.GetOrAddComponent<Animator>().Play("MatchingPlayerGrace");
        yield return new WaitForSeconds(0.1f);
        Managers.Sound.Play("휙");
        GetImage((int)Images.MatchingTime).gameObject.GetOrAddComponent<Animator>().Play("MatchingTime");
        yield return new WaitForSeconds(0.5f);
        Managers.Sound.Play("챙2");
        GetImage((int)Images.DecoImage).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        Managers.Sound.Play("챙3");
        GetImage((int)Images.PlayerImage).gameObject.GetOrAddComponent<Animator>().Play("MatchingPlayerImageAni");
        GetImage((int)Images.AnemyImage).gameObject.GetOrAddComponent<Animator>().Play("MatchingAnemyImageAni");
        yield return new WaitForSeconds(1.0f);

        GetImage((int)Images.FightImage).gameObject.GetOrAddComponent<Animator>().Play("MatchingFight");
        yield return new WaitForSeconds(0.15f);
        Managers.Sound.Play("FightEff");
        yield return new WaitForSeconds(0.5f);
        Managers.Sound.Play("DuongEff");

        Managers.Sound.Play("MatchingBgm", Define.Sound.Bgm);

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

    IEnumerator SceneChangeAnimation_Out()
    {
        // Ani
        UI_LockTouch uI_LockTouch = Managers.UI.ShowPopupUI<UI_LockTouch>();
        SceneChangeAnimation_Out anim = Managers.Resource.Instantiate("Animation/SceneChangeAnimation_Out").GetOrAddComponent<SceneChangeAnimation_Out>();
        anim.transform.SetParent(this.transform);
        anim.SetInfo(Define.Scene.Fight1vs1GameScene, () => { });

        yield return new WaitForSeconds(0.3f);
        Managers.UI.ClosePopupUI(uI_LockTouch);
    }
}
