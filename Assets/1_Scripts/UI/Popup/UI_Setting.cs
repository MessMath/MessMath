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

    enum Buttons
    {
        ContinueBtn,
        KorianBtn,
        EnglishBtn,
        ExitBtn,
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
            else Managers.Scene.ChangeScene(Define.Scene.LobbyScene);
        });
        GetButton((int)Buttons.KorianBtn).gameObject.BindEvent(OnClickedKorBtn);
        GetButton((int)Buttons.EnglishBtn).gameObject.BindEvent(OnClickedEnBtn);
        SetLanguageBtn();

        Time.timeScale = 0.0f;
        return true;
    }

    void SetLanguageBtn()
    {
        if(LocalizationManager.Get().GetSelectedLaguage() == Language.KOREAN)
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
    }

    void OnClickedEnBtn()
    {
        GetButton((int)Buttons.KorianBtn).gameObject.GetComponent<Image>().color = Color.grey;
        GetButton((int)Buttons.EnglishBtn).gameObject.GetComponent<Image>().color = Color.white;
        LocalizationManager.Get().SetLanguage(Language.ENGLISH);
    }
}
