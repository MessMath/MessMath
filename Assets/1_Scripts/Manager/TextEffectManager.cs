using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextEffectManager
{
    const string WHITE = "<color=#FFFFFF>";
    const string RED = "<color=#FF0000>";
    const string GREY = "<color=#808080>";
    const string COLORTAG = "</color>";
    const string SMALLSIZE = "<size=50>";
    const string MEDIUMSIZE = "<size=80>";
    const string LARGESIZE = "<size=100>";
    const string SIZETAG = "</size>";
    
    float timeForCharacter = 0.08f;
    float timeForCharacter_Fast = 0.01f;
    float characterTime;
    float timer;    

    string tempDialogue, replayDialogue;
    TextMeshProUGUI tempSave, replaySave;
    public bool isTypingEnd = true;
    bool t_ignore = false;
    string t_color, t_size, t_style;


    // 색상 변경 함수
    void CheckTextEffect(char c)
    {
        switch(c)
        {
            case 'ⓦ':
                t_color = "w"; t_ignore = true;
                break;
            case 'ⓡ':
                t_color = "r"; t_ignore = true;
                break;
            case 'ⓖ':
                t_color = "g"; t_ignore = true;
                break;
            case 'ⓢ':
                t_size = "s"; t_ignore = true;
                break;
            case 'ⓜ':
                t_size = "m"; t_ignore = true;
                break;
            case 'ⓛ':
                t_size = "l"; t_ignore = true;
                break;
            case 'ⓑ':
                t_style = "b"; t_ignore = true;
                break;
            case 'ⓘ':
                t_style = "i"; t_ignore = true;
                break;
            case 'ⓝ':
                t_size = "n"; t_style = "n"; t_ignore = true;
                break;
        }
    }
    string ChangeTxtEffect(char c)
    {   
        string t_letter = c.ToString();
        if(!t_ignore)
        {
            switch(t_size)
            {
                case "s":
                    t_letter = SMALLSIZE + t_letter + SIZETAG;
                    break;
                case "m":
                    t_letter = MEDIUMSIZE + t_letter + SIZETAG;
                    break;
                case "l":
                    t_letter = LARGESIZE + t_letter + SIZETAG;
                    break;
                case "n":
                    t_letter = "<size=70>" + t_letter + SIZETAG;
                    break;
            }
            switch(t_style)
            {
                case "b":
                    t_letter = "<b>" + t_letter + "</b>";
                    break;
                case "i":
                    t_letter = "<i>" + t_letter + "</i>";
                    break;
                case "n":
                    break;
            }
            switch(t_color)
            {
                case "w":
                    t_letter = WHITE + t_letter + COLORTAG;
                    break;
                case "r":
                    t_letter = RED + t_letter + COLORTAG;
                    break;
                case "g":
                    t_letter = GREY + t_letter + COLORTAG;
                    break;
            }
            return t_letter;
        }
        t_ignore=false;
        return "";
    }
    string ChangeReplayTxtEffect(char c)
    {
        string t_letter = c.ToString();
        if (!t_ignore)
        {
            switch (t_size)
            {
                case "s":
                    t_letter = SMALLSIZE + t_letter + SIZETAG;
                    break;
                case "m":
                    t_letter = MEDIUMSIZE + t_letter + SIZETAG;
                    break;
                case "l":
                    t_letter = LARGESIZE + t_letter + SIZETAG;
                    break;
            }
            switch (t_style)
            {
                case "b":
                    t_letter = "<b>" + t_letter + "</b>";
                    break;
                case "i":
                    t_letter = "<i>" + t_letter + "</i>";
                    break;
                case "n":
                    break;
            }
            switch (t_color)
            {
                case "w":
                    t_letter = WHITE + t_letter + COLORTAG;
                    break;
                case "r":
                    t_letter = RED + t_letter + COLORTAG;
                    break;
                case "g":
                    t_letter = GREY + t_letter + COLORTAG;
                    break;
            }
            return t_letter;
        }
        t_ignore = false;
        return "";
    }



    // 타이핑 효과
    public void Typing(string dialouge, TextMeshProUGUI textObj)
    {
        //dialouge = dialouge.Replace("\\r","\n");
        dialouge = dialouge.Replace("OOO", Managers.UserMng.GetNickname());
        dialouge = dialouge.Replace("\\n", "\n");
        tempDialogue = dialouge;
        tempSave = textObj;
        
        char[] chars = dialouge.ToCharArray();    
        CoroutineHandler.StartCoroutine(Typer(chars, textObj));

        tempSave.text ="";
        tempDialogue = null;
        tempSave = null;
    }

    public void ReplayTyping(string dialouge, TextMeshProUGUI textObj)
    {
        //dialouge = dialouge.Replace("\\r", "");
        dialouge = dialouge.Replace("OOO", Managers.UserMng.GetNickname());
        dialouge = dialouge.Replace("\\n", "\n");
        dialouge = dialouge.Replace("ⓝ", "");
        textObj.text = "";

        char[] chars = dialouge.ToCharArray();
        int currentChar = 0;
        int charLength = chars.Length;
        while (currentChar < charLength)
        {
            CheckTextEffect(chars[currentChar]);
            textObj.text += ChangeReplayTxtEffect(chars[currentChar++]);
        }
        textObj.text = "<size=42>" + textObj.text + SIZETAG;
    }

    public void ApplyTextEffect(string content, TextMeshProUGUI textObj, int textSize)
    {
        content = content.Replace("OOO", Managers.UserMng.GetNickname());
        content = content.Replace("\\n ", "\n");
        content = content.Replace("ⓝ", "");
        textObj.text = "";

        char[] chars = content.ToCharArray();
        int currentChar = 0;
        int charLength = chars.Length;
        while (currentChar < charLength)
        {
            CheckTextEffect(chars[currentChar]);
            textObj.text += ChangeReplayTxtEffect(chars[currentChar++]);
        }
        textObj.text = "<size=" + textSize.ToString() + ">" + textObj.text + SIZETAG;
    }

    IEnumerator Typer(char[] chars, TextMeshProUGUI textObj)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.01f);
        int currentChar = 0;
        int charLength = chars.Length;
        isTypingEnd = false;
        while (currentChar < charLength)
        {
            if(timer >= 0)
            {
                yield return waitForSeconds;
                timer -= Time.deltaTime;
            }
            else
            {
                CheckTextEffect(chars[currentChar]);
                textObj.text += ChangeTxtEffect(chars[currentChar++]);
                timer = characterTime;
            }
        }
        if (currentChar >= charLength)
        {
            isTypingEnd = true;
            yield break;
        }
    }

    public void SetFastSpeed()
    {
        characterTime = timeForCharacter_Fast;
    }

    public void SetNormalSpeed()
    {
        characterTime = timeForCharacter;
    }
    
    // 글자 흐려짐 효과 
    public IEnumerator FadeOut(TextMeshProUGUI tmp)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.01f);
        float fadeCnt = 0;
        while (fadeCnt < 1.0f)
        {
            fadeCnt += 0.01f;
            yield return waitForSeconds;
            tmp.color = new Color(0,0,0,fadeCnt);
        }
        CoroutineHandler.StartCoroutine(FadeIn(tmp));
    }

    public IEnumerator FadeIn(TextMeshProUGUI tmp)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.01f);
        float fadeCnt = 1.0f;
        while (fadeCnt > 0)
        {
            fadeCnt -= 0.01f;
            yield return waitForSeconds;
            tmp.color = new Color(0,0,0,fadeCnt);
        }
    }
}
