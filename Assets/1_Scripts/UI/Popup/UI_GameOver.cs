using MessMathI18n;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_GameOver : UI_Popup
{
    string MyClothes;

    public enum Buttons
    {
        RePlayBtn,
        BackToLobbyBtn,
    }

    public enum Images
    {
        RePlayBtn,
        BackToLobbyBtn,
        Lose,
        Lose1,
        Players_Illust,
        Opps_Illust,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        GetButton((int)Buttons.RePlayBtn).gameObject.BindEvent(RePlay);
        GetButton((int)Buttons.BackToLobbyBtn).gameObject.BindEvent(BackToLobby);
        InitMyClothes();

        if (LocalizationManager.Get().GetSelectedLanguage() == Language.ENGLISH)
        {
            GetImage((int)Images.RePlayBtn).sprite = Managers.Resource.Load<Sprite>("Sprites/Story/ResultPopup/RePlay_ENG");
            GetImage((int)Images.BackToLobbyBtn).sprite = Managers.Resource.Load<Sprite>("Sprites/Pvp/ResultPopup/BackToLobby_ENG");
            GetImage((int)Images.Lose).sprite = Managers.Resource.Load<Sprite>("Sprites/Pvp/ResultPopup/Defeat_ENG");
            GetImage((int)Images.Lose1).sprite = Managers.Resource.Load<Sprite>("Sprites/Pvp/ResultPopup/Defeat2_ENG");
        }

        if (Managers.Scene.CurrentSceneType == Define.Scene.Fight1vs1GameScene)
        {
            GetImage((int)Images.Opps_Illust).sprite = GameObject.Find("MathMtcImage").GetComponent<Image>().sprite;
            GetImage((int)Images.Opps_Illust).GetComponent<RectTransform>().sizeDelta = GameObject.Find("MathMtcImage").GetComponent<RectTransform>().sizeDelta * 1.5f;
        }
        if (Managers.Scene.CurrentSceneType == Define.Scene.StoryGameScene)
        {
            GetImage((int)Images.Opps_Illust).sprite = GameObject.Find("WitchImage").GetComponent<Image>().sprite;
            GetImage((int)Images.Opps_Illust).GetComponent<RectTransform>().sizeDelta = GameObject.Find("WitchImage").GetComponent<RectTransform>().sizeDelta * 1.5f;
        }
        
        Time.timeScale = 0;
        GetComponent<Canvas>().sortingOrder = 10;

        // Sound
        Managers.Sound.Play("DefeatEff");

        return true;
    }

    async void InitMyClothes()
    {
        MyClothes = await Managers.DBManager.GetMyClothes(Managers.GoogleSignIn.GetUID());
        // ³ªÀÇ ¿Ê
        GetImage((int)Images.Players_Illust).sprite = Managers.Resource.Load<Sprite>("Sprites/Clothes/" + MyClothes + "_full");
    }

    public void RePlay()
    {
        // Sound
        Managers.Sound.Play("ClickBtnEff");
        
        CoroutineHandler.StartCoroutine(SceneChangeAnimation(Managers.Scene.CurrentSceneType));
        Time.timeScale = 1;
    }

    public void BackToLobby()
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