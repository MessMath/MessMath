using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Setting : UI_Popup
{
    enum Images
    {
        BGImage,
        Panel,
    }

    enum Texts
    {
    
    }

    enum Buttons
    {
        ContinueBtn,
        SwitchLanguageBtn,
        ExitBtn,
    }

    enum GameObjects
    {
    
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindButton(typeof(Buttons));

        
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


        Time.timeScale = 0.0f;
        return true;
    }

    IEnumerator SceneChangeAnimation_In_Lobby()
    {
        // Ani
        UI_ChangeScenePopup uI_ChangeScenePopup = Managers.UI.ShowPopupUI<UI_ChangeScenePopup>();

        yield return new WaitForSeconds(1.3f);
        Managers.UI.ClosePopupUI(uI_ChangeScenePopup);

        Managers.Scene.ChangeScene(Define.Scene.LobbyScene);
    }

}
