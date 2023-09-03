using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MessMathI18n;

public class UI_Setting : UI_Popup
{
    enum Images
    {
        BGImage,
        Panel,
    }

    enum Texts
    {
        ContinueText,
        //LogoutText,
        ExitText,
    }

    enum Buttons
    {
        ContinueBtn,
        KorianBtn,
        EnglishBtn,
        ExitBtn,
        //LogoutBtn,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetText((int)Texts.ContinueText).text = I18n.Get(I18nDefine.SETTING_CONTINUE);
        //GetText((int)Texts.LogoutText).text = I18n.Get(I18nDefine.SETTING_LOGOUT);
        GetText((int)Texts.ExitText).text = I18n.Get(I18nDefine.SETTING_EXIT);

        GetImage((int)Images.Panel).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); ClosePopupUI(); });

        GetButton((int)Buttons.ContinueBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); ClosePopupUI(); });
        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(() => { 
            Managers.Sound.Play("ClickBtnEff");
            Time.timeScale = 1.0f;
            if (Managers.Scene.CurrentSceneType == Define.Scene.LobbyScene)
            {
                Application.Quit();
                ClosePopupUI();
            }
            else CoroutineHandler.StartCoroutine(SceneChangeAnimation_In_Lobby());
        });
        GetButton((int)Buttons.KorianBtn).gameObject.BindEvent(OnClickedKorBtn);
        GetButton((int)Buttons.EnglishBtn).gameObject.BindEvent(OnClickedEnBtn);
        //GetButton((int)Buttons.LogoutBtn).gameObject.BindEvent(OnClickedLogoutBtn);
        SetLanguageBtn();

        Time.timeScale = 0.0f;
        return true;
    }

    //void OnClickedLogoutBtn()
    //{
    //    Managers.Sound.Play("ClickBtnEff");
    //    Managers.GoogleSignIn.SignOutFromGoogle();
    //    Managers.Scene.ChangeScene(Define.Scene.MainScene);
    //    Managers.UI.CloseAllPopupUI();
    //    Debug.Log("·Î±×¾Æ¿ô");
    //}

    IEnumerator SceneChangeAnimation_In_Lobby()
    {
        // Ani
        UI_LockTouch uI_LockTouch = Managers.UI.ShowPopupUI<UI_LockTouch>();
        SceneChangeAnimation_In anim = Managers.Resource.Instantiate("Animation/SceneChangeAnimation_In").GetOrAddComponent<SceneChangeAnimation_In>();
        anim.transform.SetParent(this.transform);
        anim.SetInfo(Define.Scene.LobbyScene, () => { Managers.Scene.ChangeScene(Define.Scene.LobbyScene); });

        yield return new WaitForSeconds(0.5f);
    }

    void SetLanguageBtn()
    {
        if(LocalizationManager.Get().GetSelectedLanguage() == Language.KOREAN)
        {
            OnClickedKorBtn();
        }
        else
        {
            OnClickedEnBtn();
        }
    }

    void OnClickedKorBtn()
    {
        GetButton((int)Buttons.KorianBtn).gameObject.GetComponent<Image>().color = Color.white;
        GetButton((int)Buttons.EnglishBtn).gameObject.GetComponent<Image>().color = Color.grey;
        LocalizationManager.Get().SetLanguage(Language.KOREAN);
        if (Managers.Scene.CurrentSceneType == Define.Scene.LobbyScene)
        {
            GameObject.Find("UI_Lobby").GetOrAddComponent<UI_Lobby>().RefreshUI();
        }
    }

    void OnClickedEnBtn()
    {
        GetButton((int)Buttons.KorianBtn).gameObject.GetComponent<Image>().color = Color.grey;
        GetButton((int)Buttons.EnglishBtn).gameObject.GetComponent<Image>().color = Color.white;
        LocalizationManager.Get().SetLanguage(Language.ENGLISH);
        if (Managers.Scene.CurrentSceneType == Define.Scene.LobbyScene)
        {
            GameObject.Find("UI_Lobby").GetOrAddComponent<UI_Lobby>().RefreshUI();
        }
    }
}
