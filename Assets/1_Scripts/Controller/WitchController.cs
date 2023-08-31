using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class WitchController : MonoBehaviour
{
    public float Hp = 100f;
    public Image witchImg;
    public Image HpBar;
    public int QuestionNumber;
    public TextMeshProUGUI QuestionNumberText;
    Color32 color;

    private void Awake()
    {
        HpBar = HpBar.gameObject.GetComponent<Image>();
        witchImg = witchImg.gameObject.GetComponent<Image>();
        color = witchImg.color;
        if(Managers.Scene.CurrentSceneType == Define.Scene.StoryGameScene)
            Questioning();
    }

    IEnumerator BlinkWitchImg(float delayTime)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.15f);
        int countTime = 0;

        while (countTime < 10)
        {
            if (countTime % 2 == 0)
                witchImg.color = new Color32(color.r, color.g, color.b, 90);
            else
                witchImg.color = new Color32(color.r, color.g, color.b, 180);

            yield return waitForSeconds;

            countTime++;
        }

        witchImg.color = color;
        yield return null;
    }

    public void SetWitchHP(float damage)
    {
        Hp -= damage;
        float curHpRatio = Hp / 100;
        float damaged = (100-Hp) * 14.4f;
        if(damaged <= 0f)
        {
            HpBar.fillAmount = 0;
        }
        else
        {
            HpBar.fillAmount = curHpRatio;
        }
        StartCoroutine("BlinkWitchImg", .1f);
    }

    public void Questioning()
    {
        QuestionNumber = Random.Range(10, 100);
        QuestionNumberText.text = $"{QuestionNumber}";
    }

}
