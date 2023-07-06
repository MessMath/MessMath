using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class WitchController : MonoBehaviour
{
    public float Hp = 100f;
    public Image witchImg;
    public RectTransform HpBar;
    public int QusetionNumber;
    public TextMeshProUGUI QusetionNumberText;
    Color color;

    private void Awake()
    {
        HpBar = HpBar.gameObject.GetComponent<RectTransform>();
        witchImg = witchImg.gameObject.GetComponent<Image>();
        color = witchImg.color;
        if(Managers.Scene.CurrentSceneType == Define.Scene.StoryGameScene)
            Questioning();
    }

    IEnumerator BlinkWitchImg(float delayTime)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.15f);
        int countTime = 0;
        while(countTime < 10)
        {
            if (countTime % 2 == 0)
                witchImg.color = new Color32(255, 255, 255, 90);
            else
                witchImg.color = new Color32(255, 255, 255, 180);

            yield return waitForSeconds;

            countTime++;
        }

        witchImg.color = Color.white;
        yield return null;
    }

    public void SetWitchHP(float damage)
    {
        Hp -= damage;
        float damaged = (100-Hp) * 14.4f;
        HpBar.offsetMax = new Vector2(-0, -damaged);
        StartCoroutine("BlinkWitchImg", .1f);
    }

    public void Questioning()
    {
        QusetionNumber = Random.Range(10, 100);
        QusetionNumberText.text = $"{QusetionNumber}";
    }

}
