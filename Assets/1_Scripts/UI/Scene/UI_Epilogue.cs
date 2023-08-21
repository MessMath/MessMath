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

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));

        ImagesPreSetting();

        GetImage((int)Images.Image1).gameObject.BindEvent(() => GetImage((int)Images.Image2).gameObject.SetActive(true));
        GetImage((int)Images.Image2).gameObject.BindEvent(() => GetImage((int)Images.Image3).gameObject.SetActive(true));
        GetImage((int)Images.Image3).gameObject.BindEvent(ShowCredits);
        GetImage((int)Images.Credits).gameObject.SetActive(false);

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