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
            case "Main character":
            case "주인공":
                imgName = "Script_player_UI";
                break;
            case "???":
                imgName = "Script_witch_UI";
                break;
            case "Renegade Witch":
            case "변절된 마법사":
                imgName = "Script_witch_UI";
                break;
            case "Gauss":
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
            case "Main character":
            case "주인공":
                playerImg.color = Color.white;
                ChangeExpression(playerImg, expression);
                break;
            case "Principal":
            case "선생님":
                characterImg.color = Color.white;
                characterImg.sprite = Resources.Load("Sprites/Story/Characters/teacher", typeof(Sprite)) as Sprite;
                break;
            case "Gauss":
            case "가우스":
                characterImg.color = Color.white;
                characterImg.sprite = Resources.Load("Sprites/Story/Characters/gauss", typeof(Sprite)) as Sprite;
                break;
            case "":
                playerImg.color = Color.clear;
                characterImg.color = Color.clear;
                break;
        }
    }

    void ChangeExpression(Image img, string expression)
    {
        switch(expression)
        {
            case "worry":
                //img.sprite = Resources.Load("Sprites/Story/Characters/Expression/worry", typeof(Sprite)) as Sprite;
                img.sprite = Resources.Load<Sprite>("Sprites/Story/Characters/Expression/worry");
                break;
            case "curiosity":
                //img.sprite = Resources.Load("Sprites/Story/Characters/Expression/curiosity", typeof(Sprite)) as Sprite;
                img.sprite = Resources.Load<Sprite>("Sprites/Story/Characters/Expression/curiosity");
                break;
            case "difficulty":
                //img.sprite = Resources.Load("Sprites/Story/Characters/Expression/difficulty", typeof(Sprite)) as Sprite;
                img.sprite = Resources.Load<Sprite>("Sprites/Story/Characters/Expression/difficulty");
                break;
            case "shock":
                //img.sprite = Resources.Load("Sprites/Story/Characters/Expression/shock", typeof(Sprite)) as Sprite;
                img.sprite = Resources.Load<Sprite>("Sprites/Story/Characters/Expression/shock");
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
                nxtBtn.gameObject.SetActive(false);
                CoroutineHandler.StartCoroutine(FadeOut(img, nxtBtn));
                break;
            case "FadeIn":
                CoroutineHandler.StartCoroutine(FadeIn(img, nxtBtn));
                break;
            case "OpenDoor":
                nxtBtn.gameObject.SetActive(false);
                CoroutineHandler.StartCoroutine(OpenDoor(img, nxtBtn));
                break;
            case "":
                break;
        }
    }

    IEnumerator FadeOut(Image img, Button nxtBtn)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.01f);
        float fadeCnt = 0;
        img.color = Color.black;
        while (fadeCnt < 1.0f)
        {
            fadeCnt += 0.015f;
            yield return waitForSeconds;
            if(img != null )img.color = new Color(0,0,0,fadeCnt);
            if(fadeCnt >= 0.9f) break;
        }
        //CoroutineHandler.StartCoroutine(FadeIn(img,nxtBtn));
        if(img.color == new Color(0,0,0,fadeCnt)) 
        {
            img.gameObject.GetComponentInParent<UI_Story>().OnClickNxtBtn();
        }
    }

    IEnumerator FadeIn(Image img, Button nxtBtn)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.01f);
        float fadeCnt = 1.0f;
        img.color = Color.black;
        while (fadeCnt > 0)
        {
            fadeCnt -= 0.015f;
            yield return waitForSeconds;
            if(img != null )img.color = new Color(0,0,0,fadeCnt);
            if(fadeCnt <= 0.1) break;
        }
        if(img.color == new Color(0,0,0,fadeCnt)) 
        {
            nxtBtn.gameObject.SetActive(true);
            nxtBtn.transform.parent.transform.GetComponentInParent<UI_Story>().OnClickNxtBtn();
        }
    }

    IEnumerator OpenDoor(Image img, Button nxtBtn)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.01f);
        img.color = Color.white;
        img.sprite = Resources.Load("Sprites/Story/open_door", typeof(Sprite)) as Sprite;
        float fadeCnt = 1.0f;
        while (fadeCnt < 4.0f)
        {
            fadeCnt += 0.05f;
            yield return waitForSeconds;
            if (img != null) img.rectTransform.localScale = new Vector3(fadeCnt, 1, 1);
            if (fadeCnt >= 3.9f) break;
        }
        if (img.rectTransform.localScale == new Vector3(fadeCnt, 1, 1))
        {
            img.rectTransform.localScale = new Vector3(1, 1, 1);
            img.color = new Color(0, 0, 0, 0);
            img.sprite = Resources.Load("Sprites/Story/Background/null", typeof(Sprite)) as Sprite;
            nxtBtn.gameObject.SetActive(true);
            nxtBtn.transform.parent.transform.GetComponentInParent<UI_Story>().OnClickNxtBtn();
        }
    }
}

