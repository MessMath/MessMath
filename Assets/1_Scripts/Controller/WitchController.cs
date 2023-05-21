using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class WitchController : MonoBehaviour
{
    public int Hp = 100;
    public Image witchImg;
    public RectTransform HpBar;
    public int QusetionNumber;
    public TextMeshProUGUI QusetionNumberText;
    public bool isAlive = true;
    Color color;

    private void Awake()
    {
        HpBar = HpBar.gameObject.GetComponent<RectTransform>();
        witchImg = witchImg.gameObject.GetComponent<Image>();
        color = witchImg.color;
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

    private void Update()
    {
        GameWinPopup();
    }

    public void SetWitchHP(int damage)
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

    public void GameWinPopup()
    {
        if (Hp <= 0 && isAlive == true)
        {
            Managers.UI.ShowPopupUI<UI_GameWin>();
            isAlive = false;
        }
    }
}
