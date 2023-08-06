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

    public void ChangeCharacterBG(Image img, string characterName)
    {
        string imgName;
        switch(characterName)
        {
            case "주인공":
                imgName = "Script_player_UI";
                break;
            case "???":
                imgName = "Script_witch_UI";
                break;
            case "변절된 마법사":
                imgName = "Script_witch_UI";
                break;
            case "가우스":
                imgName = "Script_book_UI";
                break;
            default:
                imgName = "Script_Principal_UI";
                break;
        }
        
        img.sprite = Resources.Load("Sprites/UI/5_Story_UI/" + imgName, typeof(Sprite)) as Sprite;
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
        //Sprite expressionSprite = Resources.Load("Sprites/Story/Characters/Expression/Worry", typeof(Sprite)) as Sprite;
        img.sprite = Resources.Load("Sprites/Story/Characters/Expression/" + expression, typeof(Sprite)) as Sprite;
        Debug.Log(img.gameObject.name);
        Debug.Log(img.sprite);
        //Debug.Log(expressionSprite);
        Debug.Log("Sprites/Story/Characters/Expression/" + expression);
        Debug.Log("expression: " + expression);
        /*Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Story/Characters/Expression");
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
        }*/
    }

    public void SceneEffect(Image img, Button nxtBtn, string sceneEffect)
    {
        switch(sceneEffect)
        {
            case "FadeOut":
                nxtBtn.gameObject.SetActive(false);
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
            fadeCnt += 0.015f;
            yield return waitForSeconds;
            if(img != null )img.color = new Color(0,0,0,fadeCnt);
        }
        CoroutineHandler.StartCoroutine(FadeIn(img,nxtBtn));
    }

    IEnumerator FadeIn(Image img, Button nxtBtn)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.01f);
        float fadeCnt = 1.0f;
        while (fadeCnt > 0)
        {
            fadeCnt -= 0.015f;
            yield return waitForSeconds;
            if(img != null )img.color = new Color(0,0,0,fadeCnt);
            //Debug.Log(fadeCnt);
            if(fadeCnt <= 0.1) break;
        }
        //Debug.Log("done: " + fadeCnt);
        if(img.color == new Color(0,0,0,fadeCnt)) {
            nxtBtn.gameObject.SetActive(true);
            nxtBtn.transform.parent.transform.GetComponentInParent<UI_Story>().OnClickNxtBtn();
        }
    }
}

