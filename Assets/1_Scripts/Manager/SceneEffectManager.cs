using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneEffectManager
{
    public void ChangeBackground(Image img, string imgName)
    {
        if(imgName=="") return;
        if(imgName=="NULL") 
        {
            img.color = Color.black; 
            return;
        }
        img.color = Color.white;
        img.sprite = Resources.Load("Sprites/Story/Background/" + imgName, typeof(Sprite)) as Sprite;
    }

    public void ChangeCharacter(Image playerImg, Image characterImg, string characterName, string expression)
    {
        switch(characterName)
        {
            case "주인공":
                playerImg.color = Color.white;
                ChangeExpression(playerImg, expression);
                break;
            case "선생님":
                characterImg.color = Color.white;
                characterImg.sprite = Resources.Load("Sprites/Story/Characters/teacher", typeof(Sprite)) as Sprite;
                break;
            case "가우스":
                characterImg.color = Color.white;
                characterImg.sprite = Resources.Load("Sprites/Story/Characters/gaus", typeof(Sprite)) as Sprite;
                break;
            case "":
                playerImg.color = Color.clear;
                characterImg.color = Color.clear;
                break;
        }
    }

    void ChangeExpression(Image img, string expression)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Story/Characters/Expression");
        switch(expression)
        {
            case "SOMBER\r":
                img.sprite = sprites[4];
                break;
            case "CURIOSITY\r":
                img.sprite = sprites[6];
                break;
            case "DISGUSTED\r":
                img.sprite = sprites[5];
                break;
            case "SUPRISED\r":
                img.sprite = sprites[5];
                break;
            case "":
                break;
        }
    }

    public void SceneEffect(Image img, Button nxtBtn, string sceneEffect)
    {
        switch(sceneEffect)
        {
            case "FadeOut":
                CoroutineHandler.StartCoroutine(FadeOut(img, nxtBtn));
                break;
            case "FadeIn":
                CoroutineHandler.StartCoroutine(FadeIn(img, nxtBtn));
                break;
            case "":
                break;
        }
    }

    IEnumerator FadeOut(Image img, Button nxtBtn)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.01f);
        float fadeCnt = 0;
        while (fadeCnt < 1.0f)
        {
            fadeCnt += 0.01f;
            yield return waitForSeconds;
            img.color = new Color(0,0,0,fadeCnt);
        }
        CoroutineHandler.StartCoroutine(FadeIn(img,nxtBtn));
    }

    IEnumerator FadeIn(Image img, Button nxtBtn)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.01f);
        float fadeCnt = 1.0f;
        while (fadeCnt > 0)
        {
            fadeCnt -= 0.01f;
            yield return waitForSeconds;
            img.color = new Color(0,0,0,fadeCnt);
        }
        nxtBtn.interactable = true;
    }
}

