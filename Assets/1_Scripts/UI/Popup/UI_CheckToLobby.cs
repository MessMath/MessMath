using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MessMathI18n;

public class UI_CheckToLobby : UI_Popup
{
    enum Images
    {
        BGImage,
        Panel,
    }

    enum Buttons
    {
        ExitBtn,
    }

    enum Texts
    {
        CheckToLobbyText,
        ExitBtnText,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetImage((int)Images.Panel).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); ClosePopupUI(); });
        GetText((int)Texts.ExitBtnText).text = I18n.Get(I18nDefine.BackToLobby);
        GetText((int)Texts.CheckToLobbyText).text = I18n.Get(I18nDefine.LOBBY_TO_LOBBY_CHECK);
        if (LocalizationManager.Get().GetSelectedLanguage() == Language.ENGLISH) GetText((int)Texts.CheckToLobbyText).fontSize = 80;

            GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(() => { 
            Managers.Sound.Play("ClickBtnEff");
            Time.timeScale = 1.0f;
            CoroutineHandler.StartCoroutine(SceneChangeAnimation_In_Lobby());
        });

        Time.timeScale = 0.0f;
        return true;
    }

    IEnumerator SceneChangeAnimation_In_Lobby()
    {
        // Ani
        UI_LockTouch uI_LockTouch = Managers.UI.ShowPopupUI<UI_LockTouch>();
        SceneChangeAnimation_In anim = Managers.Resource.Instantiate("Animation/SceneChangeAnimation_In").GetOrAddComponent<SceneChangeAnimation_In>();
        anim.transform.SetParent(this.transform);
        anim.SetInfo(Define.Scene.LobbyScene, () => { Managers.Scene.ChangeScene(Define.Scene.LobbyScene); });

        yield return new WaitForSeconds(0.5f);
    }
}
