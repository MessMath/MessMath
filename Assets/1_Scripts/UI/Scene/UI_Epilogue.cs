using MessMathI18n;
using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Epilogue : UI_Scene
{
    enum Images
    {
        Image1,
        Image2,
        Image3,
        Image4,
        FadeIn, 
        Credits,
    }

    enum Texts
    {
        Development,
        Context1,
        Context2,
        Context3,
        Context4,
        Art,
        Context5,
        Design,
        Context6,
        Context7,
        Context8,
        Context9,
        Context10,
        AnywayCredit,
        Team_Presents,
    }

    enum Buttons
    {
        BackToLobbyBtn,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindButton(typeof(Buttons));

        Managers.Sound.Play("EpilogueSealingBGM");

        GetImage((int)Images.Image1).gameObject.BindEvent(() => { GetImage((int)Images.Image2).gameObject.SetActive(true); Managers.Sound.Play("합격통보음"); });
        GetImage((int)Images.Image2).gameObject.BindEvent(() => { GetImage((int)Images.Image3).gameObject.SetActive(true); Managers.Sound.Clear(); Managers.Sound.Play("EpilogueAwardedBGM"); });
        GetImage((int)Images.Image3).gameObject.BindEvent(() => GetImage((int)Images.Image4).gameObject.SetActive(true));
        GetImage((int)Images.Image4).gameObject.BindEvent(ShowCredits);

        GetImage((int)Images.Credits).transform.Find("Development").GetComponent<TextMeshProUGUI>().text = I18n.Get(I18nDefine.Development);
        GetImage((int)Images.Credits).transform.Find("Context1").GetComponent<TextMeshProUGUI>().text = I18n.Get(I18nDefine.오합지졸_대장_서현재);
        GetImage((int)Images.Credits).transform.Find("Context2").GetComponent<TextMeshProUGUI>().text = I18n.Get(I18nDefine.오합지졸_서장_윤지연);
        GetImage((int)Images.Credits).transform.Find("Context3").GetComponent<TextMeshProUGUI>().text = I18n.Get(I18nDefine.오합지졸_심장_정수진);
        GetImage((int)Images.Credits).transform.Find("Context4").GetComponent<TextMeshProUGUI>().text = I18n.Get(I18nDefine.오합지졸_꼬장_배정훈);
        GetImage((int)Images.Credits).transform.Find("Art").GetComponent<TextMeshProUGUI>().text = I18n.Get(I18nDefine.Art);
        GetImage((int)Images.Credits).transform.Find("Context5").GetComponent<TextMeshProUGUI>().text = I18n.Get(I18nDefine.오합지졸_새장_서민영);
        GetImage((int)Images.Credits).transform.Find("Design").GetComponent<TextMeshProUGUI>().text = I18n.Get(I18nDefine.Design);
        GetImage((int)Images.Credits).transform.Find("Context6").GetComponent<TextMeshProUGUI>().text = I18n.Get(I18nDefine.오합지졸_대장_서현재);
        GetImage((int)Images.Credits).transform.Find("Context7").GetComponent<TextMeshProUGUI>().text = I18n.Get(I18nDefine.오합지졸_서장_윤지연);
        GetImage((int)Images.Credits).transform.Find("Context8").GetComponent<TextMeshProUGUI>().text = I18n.Get(I18nDefine.오합지졸_심장_정수진);
        GetImage((int)Images.Credits).transform.Find("Context9").GetComponent<TextMeshProUGUI>().text = I18n.Get(I18nDefine.오합지졸_꼬장_배정훈);
        GetImage((int)Images.Credits).transform.Find("Context10").GetComponent<TextMeshProUGUI>().text = I18n.Get(I18nDefine.오합지졸_새장_서민영);
        GetImage((int)Images.Credits).transform.Find("AnywayCredit").GetComponent<TextMeshProUGUI>().text = I18n.Get(I18nDefine.아무튼_크레딧);
        GetImage((int)Images.Credits).transform.Find("Team_Presents").GetComponent<TextMeshProUGUI>().text = I18n.Get(I18nDefine.Team_오합지졸_Presents);

        GetButton((int)Buttons.BackToLobbyBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.Scene.ChangeScene(Define.Scene.LobbyScene); });
        
        if(LocalizationManager.Get().GetSelectedLanguage() == Language.ENGLISH)
            GetButton((int)Buttons.BackToLobbyBtn).GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>("Sprites/Pvp/ResultPopup/BackToLobby_ENG");

        ImagesPreSetting();

        GetImage((int)Images.Credits).gameObject.SetActive(false);

        StartCoroutine(FadeIn());

        return true;
    }

    void ImagesPreSetting()
    {
        GetImage((int)Images.Image1).gameObject.SetActive(true);
        GetImage((int)Images.Image2).gameObject.SetActive(false);
        GetImage((int)Images.Image3).gameObject.SetActive(false);
        GetImage((int)Images.Image4).gameObject.SetActive(false);
    }

    IEnumerator FadeIn()
    {
        Image FadeIn = transform.Find("FadeIn").GetComponent<Image>();
        Color fadecolor = FadeIn.color;
        FadeIn.gameObject.SetActive(true);

        float time = 0f;
        float FadingTime = 1f;

        float start = 1f;
        float end = 0f;

        while (FadeIn.color.a > 0f)
        {
            time += Time.deltaTime / FadingTime;

            fadecolor.a = Mathf.Lerp(start, end, time);

            FadeIn.color = fadecolor;

            yield return null;
        }
        FadeIn.gameObject.SetActive(false);
    }

    void ShowCredits()
    {
        GetImage((int)Images.Credits).gameObject.SetActive(true);
    }

}