using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class WitchController : MonoBehaviour
{
    public int Hp = 100;
    public RectTransform HpBar;
    public int QusetionNumber;
    public TextMeshProUGUI QusetionNumberText;
    public bool isAlive = true;

    private void Awake()
    {
        HpBar = HpBar.gameObject.GetComponent<RectTransform>();
        Questioning();
        // TODO 테스트 후 지우기
        StartCoroutine("DamageWitch", 1f);
    }

    // TODO 테스트 후 지우기
    IEnumerator DamageWitch (float delayTime)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(delayTime);
        SetWitchHP(10);
        yield return waitForSeconds;
        StartCoroutine("DamageWitch", 1f);
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
