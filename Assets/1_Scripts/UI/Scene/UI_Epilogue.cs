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
        Image0,
        Image1,
        Image2,
        Image3,
        Award,
        Image4,
        Image5,
        Credits,
        Fade,
        FadeWhite,
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
        Award_Title,
        Award_AwardContext,
        Award_Nickname,
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
        BindText(typeof(Texts));

        Managers.Sound.Play("EpilogueSealingBGM");

        GetImage((int)Images.Image0).gameObject.BindEvent(() => { StartCoroutine(NextImage("FadeWhite", GetImage((int)Images.Image1).gameObject)); });
        GetImage((int)Images.Image1).gameObject.BindEvent(() => { GetImage((int)Images.Image2).gameObject.SetActive(true); Managers.Sound.Play("합격통보음"); });
        GetImage((int)Images.Image2).gameObject.BindEvent(() => { StartCoroutine(NextImage("Fade", GetImage((int)Images.Image3).gameObject));  Managers.Sound.Clear(); Managers.Sound.Play("EpilogueAwardedBGM"); });
        GetImage((int)Images.Image3).gameObject.BindEvent(() => { GetImage((int)Images.Award).gameObject.SetActive(true); });
        GetImage((int)Images.Award).gameObject.BindEvent(() => { GetImage((int)Images.Image4).gameObject.SetActive(true); });
        GetImage((int)Images.Image4).gameObject.BindEvent(() => { StartCoroutine(NextImage("FadeWhite", GetImage((int)Images.Image5).gameObject)); });
        GetImage((int)Images.Image5).gameObject.BindEvent(ShowCredits);

        #region Credit Texts
        GetText((int)Texts.Development).text = I18n.Get(I18nDefine.Development);
        GetText((int)Texts.Context1).text = I18n.Get(I18nDefine.오합지졸_대장_서현재);
        GetText((int)Texts.Context2).text = I18n.Get(I18nDefine.오합지졸_서장_윤지연);
        GetText((int)Texts.Context3).text = I18n.Get(I18nDefine.오합지졸_심장_정수진);
        GetText((int)Texts.Context4).text = I18n.Get(I18nDefine.오합지졸_꼬장_배정훈);
        GetText((int)Texts.Art).text = I18n.Get(I18nDefine.Art);
        GetText((int)Texts.Context5).text = I18n.Get(I18nDefine.오합지졸_새장_서민영);
        GetText((int)Texts.Design).text = I18n.Get(I18nDefine.Design);
        GetText((int)Texts.Context6).text = I18n.Get(I18nDefine.오합지졸_대장_서현재);
        GetText((int)Texts.Context7).text = I18n.Get(I18nDefine.오합지졸_서장_윤지연);
        GetText((int)Texts.Context8).text = I18n.Get(I18nDefine.오합지졸_심장_정수진);
        GetText((int)Texts.Context9).text = I18n.Get(I18nDefine.오합지졸_꼬장_배정훈);
        GetText((int)Texts.Context10).text = I18n.Get(I18nDefine.오합지졸_새장_서민영);
        GetText((int)Texts.AnywayCredit).text = I18n.Get(I18nDefine.아무튼_크레딧);
        GetText((int)Texts.Team_Presents).text = I18n.Get(I18nDefine.Team_오합지졸_Presents);
        #endregion

        GetButton((int)Buttons.BackToLobbyBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.Scene.ChangeScene(Define.Scene.LobbyScene); });
        
        GetText((int)Texts.Award_Title).text = I18n.Get(I18nDefine.Award_Title);
        GetText((int)Texts.Award_AwardContext).text = I18n.Get(I18nDefine.Award_AwardContext);
        SetNickname();

        if (LocalizationManager.Get().GetSelectedLanguage() == Language.ENGLISH)
            GetButton((int)Buttons.BackToLobbyBtn).GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>("Sprites/Pvp/ResultPopup/BackToLobby_ENG");

        ImagesPreSetting();

        GetImage((int)Images.Credits).gameObject.SetActive(false);

        StartCoroutine(FadeIn("Fade"));

        return true;
    }

    async void SetNickname()
    {
        GetText((int)Texts.Award_Nickname).text = await Managers.DBManager.GetNickName(Managers.GoogleSignIn.GetUID());
    }

    void ImagesPreSetting()
    {
        GetImage((int)Images.Image0).gameObject.SetActive(true);
        GetImage((int)Images.Image1).gameObject.SetActive(false);
        GetImage((int)Images.Image2).gameObject.SetActive(false);
        GetImage((int)Images.Image3).gameObject.SetActive(false);
        GetImage((int)Images.Award).gameObject.SetActive(false);
        GetImage((int)Images.Image4).gameObject.SetActive(false);
        GetImage((int)Images.Image5).gameObject.SetActive(false);
    }

    IEnumerator NextImage(string name,GameObject go)
    {
        yield return StartCoroutine(FadeOut(name));
        go.SetActive(true);
        yield return StartCoroutine(FadeIn(name));
    }

    IEnumerator FadeIn(string name)
    {
        Image FadeIn = transform.Find(name).GetComponent<Image>();
        Color fadecolor = FadeIn.color;

        float time = 0f;
        float FadingTime = 1f;

        float start = 1f;
        float end = 0f;

        while (FadeIn.color.a > 0f)
        {
            time += Time.smoothDeltaTime / FadingTime;

            fadecolor.a = Mathf.Lerp(start, end, time);

            FadeIn.color = fadecolor;

            yield return null;
        }
    }

    IEnumerator FadeOut(string name)
    {
        Image FadeOut = transform.Find(name).GetComponent<Image>();
        Color fadecolor = FadeOut.color;

        float time = 0f;
        float FadingTime = 1f;

        float start = 0f;
        float end = 1f;

        while (FadeOut.color.a < 1f)
        {
            time += Time.smoothDeltaTime / FadingTime;

            fadecolor.a = Mathf.Lerp(start, end, time);

            FadeOut.color = fadecolor;

            yield return null;
        }
    }

    void ShowCredits()
    {
        GetImage((int)Images.Credits).gameObject.SetActive(true);
    }

}