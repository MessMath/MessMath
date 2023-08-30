using MessMathI18n;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_SelectLanguage : UI_Popup
{
    enum Buttons
    {
        KoreanBtn,
        EnglishBtn,
    }

    enum Images
    {
        Image,
        Korea,
        English,
    }

    enum Texts
    {
        NextText,
        KoreaText,
        EnglishText,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        GetText((int)Texts.NextText).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); OnClickedNextBtn(); });
        GetButton((int)Buttons.KoreanBtn).gameObject.BindEvent(OnClickedKorBtn);
        GetButton((int)Buttons.EnglishBtn).gameObject.BindEvent(OnClickedEnBtn);
        GetText((int)Texts.NextText).gameObject.SetActive(false);
        GetImage((int)Images.Image).gameObject.SetActive(false);
        Time.timeScale = 0;
        return true;
    }

    private void Update()
    {
        
    }

    void OnClickedNextBtn()
    {
        Time.timeScale = 1;
        Managers.UI.ClosePopupUI(this);
        Managers.Scene.ChangeScene(Define.Scene.MakeTxtFileScene);
        
    }

    void OnClickedKorBtn()
    {
        LocalizationManager.Get().SetLanguage(Language.KOREAN);

        GetText((int)Texts.KoreaText).gameObject.GetComponent<TextMeshProUGUI>().color = Color.grey;
        GetText((int)Texts.EnglishText).gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;
        GetImage((int)Images.Korea).gameObject.GetComponent<Image>().color = Color.grey;
        GetImage((int)Images.English).gameObject.GetComponent<Image>().color = Color.white;
        GetText((int)Texts.NextText).gameObject.SetActive(true);
        GetImage((int)Images.Image).gameObject.SetActive(true);
        Debug.Log("한글 버전 선택");
    }

    void OnClickedEnBtn()
    {
        LocalizationManager.Get().SetLanguage(Language.ENGLISH);

        GetText((int)Texts.KoreaText).gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;
        GetText((int)Texts.EnglishText).gameObject.GetComponent<TextMeshProUGUI>().color = Color.grey;
        GetImage((int)Images.Korea).gameObject.GetComponent<Image>().color = Color.white;
        GetImage((int)Images.English).gameObject.GetComponent<Image>().color = Color.grey;
        GetText((int)Texts.NextText).gameObject.SetActive(true);
        GetImage((int)Images.Image).gameObject.SetActive(true);
        Debug.Log("영어 버전 선택");
    }

}
