using MessMathI18n;
using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Epilogue : UI_Scene
{
    enum Images
    {
        Image1,
        Image2,
        Image3,
        FadeIn, 
        Credits,
    }

    enum Texts
    {
        °³¹ß,
        ¿ÀÇÕÁöÁ¹_´ëÀå_¼­ÇöÀç1,
        ¿ÀÇÕÁöÁ¹_¼­Àå_À±Áö¿¬1,
        ¿ÀÇÕÁöÁ¹_½ÉÀå_Á¤¼öÁø1,
        ¿ÀÇÕÁöÁ¹_²¿Àå_¹èÁ¤ÈÆ1,
        ¾ÆÆ®,
        ¿ÀÇÕÁöÁ¹_»õÀå_¼­¹Î¿µ1,
        ±âÈ¹,
        ¿ÀÇÕÁöÁ¹_´ëÀå_¼­ÇöÀç2,
        ¿ÀÇÕÁöÁ¹_¼­Àå_À±Áö¿¬2,
        ¿ÀÇÕÁöÁ¹_½ÉÀå_Á¤¼öÁø2,
        ¿ÀÇÕÁöÁ¹_²¿Àå_¹èÁ¤ÈÆ2,
        ¿ÀÇÕÁöÁ¹_»õÀå_¼­¹Î¿µ2,
        ¾Æ¹«Æ°_Å©·¹µ÷,
        Team_¿ÀÇÕÁöÁ¹_Presents,
        AndYou,
        BackToLobby,
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

        ImagesPreSetting();

        GetImage((int)Images.Image1).gameObject.BindEvent(() => GetImage((int)Images.Image2).gameObject.SetActive(true));
        GetImage((int)Images.Image2).gameObject.BindEvent(() => GetImage((int)Images.Image3).gameObject.SetActive(true));
        GetImage((int)Images.Image3).gameObject.BindEvent(ShowCredits);
        GetImage((int)Images.Credits).gameObject.SetActive(false);

        GetText((int)Texts.°³¹ß).text = I18n.Get(I18nDefine.LOBBY_SETTING);




        GetButton((int)Buttons.BackToLobbyBtn).gameObject.BindEvent(()=>{ Managers.Sound.Play("ClickBtnEff");  Managers.Scene.ChangeScene(Define.Scene.LobbyScene); });

        StartCoroutine(FadeIn());

        return true;
    }

    void ImagesPreSetting()
    {
        GetImage((int)Images.Image1).gameObject.SetActive(true);
        GetImage((int)Images.Image2).gameObject.SetActive(false);
        GetImage((int)Images.Image3).gameObject.SetActive(false);
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