using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextEffectManager
{
    /*public  class TXTCOLOR 
    {
        public string RED = "<code = FF0000>";
        public string BLUE = "<code = 0000FF>";
        public string GREEN = "<code = 00FF00>";
    }*/
   float timeForCharacter = 0.08f;
   float timeForCharacter_Fast = 0.01f;
   float characterTime;

   string tempDialogue;
   TextMeshProUGUI tempSave;

   //static bool isDialogEnd;
   public bool isTypingEnd = true;
   //int dialogNumber = 0;
   float timer;

   // 색상 변경 함수
   // 줄 바꿈 함수
   // 글자 크기 최대 함수
   // 타이핑 효과
   public void Typing(string dialouge, TextMeshProUGUI textObj)
   {
        //isDialogEnd  = false;
        tempDialogue = dialouge;
        tempSave = textObj;

        char[] chars = dialouge.ToCharArray();    
        CoroutineHandler.StartCoroutine(Typer(chars, textObj));

        tempSave.text ="";
        tempDialogue = null;
        tempSave = null;
   }

   public void SetFastSpeed()
   {
        characterTime = timeForCharacter_Fast;
   }

   public void SetNormalSpeed()
   {
        characterTime = timeForCharacter;
   }

   IEnumerator Typer(char[] chars, TextMeshProUGUI textObj)
   {
        int currentChar = 0;
        int charLength = chars.Length;
        isTypingEnd = false;
        while (currentChar < charLength)
        {
            if(timer >= 0)
            {
                yield return null;
                timer -= Time.deltaTime;
            }
            else
            {
                textObj.text += chars[currentChar++].ToString();
                timer = characterTime;
            }
        }
        if (currentChar >= charLength)
        {
            isTypingEnd = true;
            //dialogNumber++;
            yield break;
        }
   }
   // 글자 흔들림 효과
}
