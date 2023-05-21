using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Net;

public class Calculate_Board : MonoBehaviour
{
    public TextMeshProUGUI PrintNumber;
    public WitchController WitchController;
    public PlayerControllerCCF playerController;
    TextMeshProUGUI expression;
    SpriteRenderer spriteRenderer;
    int _idxOfHeart;
    bool isHit = false;

    private void Start()
    {
        PrintNumber.text = "";
        expression = GetComponentInChildren<TextMeshProUGUI>();
        expression.text = "";
        _idxOfHeart = 0;
        for (int i = 0; i < 3; i++)
        {
            GameObject.Find($"Test_Player/Circle/heart{i}").SetActive(true);
        }
        spriteRenderer = playerController.GetComponent<SpriteRenderer>();
        
    }

    public void Calculate()
    {
        Debug.Log("Calculate");

        // 1. 만든 식이 부적절한 식일 경우 (e.g. 12+-) => Player의 Hp를 감소
        // 2. 만든 식의 결과가 Witch의 QusetionNumber와 일치하면 => Witch의 Hp를 감소
        // 3. 만든 식의 결과가 Witch의 QusetionNumber와 일치지 않으면 => Player의 Hp를 감소

        object result = null;
        string expressionToCalculate = expression.text.Replace("x","*");        // 곱하기 연산자 바꾸기!
        string printResult;

        expression.text = "";
        DataTable table = new DataTable();

        try
        {
            result = table.Compute(expressionToCalculate, "");     // 수식을 스트링으로 받아서 계산. 결과를 result에 저장.
            printResult = Math.Truncate(Convert.ToDouble(result)).ToString();
            Debug.Log($"\"{expressionToCalculate}\" result is : " + printResult);
        }
        catch (System.Exception e)                         // 연산이 불가능한 식일 경우 예외처리
        {
            Debug.Log($"\"{expressionToCalculate}\" is inappropriate expression! : {e}");
            printResult = "";
            damageToPlayer(1);
            return;
        }

        if (PrintNumber)     // 계산 결과를 잠깐 출력
        {
            PrintNumber.text = $"={printResult}";
            StartCoroutine(Waitfor2Sec());
        }

        if(printResult == "")
            damageToPlayer(1);
        else if (int.Parse(printResult) == WitchController.QusetionNumber) // 소숫점은 어떻게 처리할까? (지금은 소숫점일 때 이 행에서 오류난다)
            damageToWitch(15);
        else
            damageToPlayer(1);

    }

    IEnumerator Waitfor2Sec()           // 2초 지연 후에 PrintNumber 텍스트 삭제
    {
        yield return new WaitForSeconds(2.0f);
        Debug.Log("Wait2Sec");
        PrintNumber.text = "";
    }
    void damageToPlayer(int damage)
    {
        playerController.Hp -= damage;
        Debug.Log("player damage 1");
        WitchController.Questioning();
        GameObject.Find($"Test_Player/Circle/heart{_idxOfHeart}").SetActive(false);
        StartCoroutine("Hit");
        if (_idxOfHeart > playerController.Hp)
            return;
        else
            _idxOfHeart++;
    }
    void damageToWitch(int damage)
    {
        WitchController.Hp -= damage;
        WitchController.HpBar.value = WitchController.Hp;
        WitchController.Questioning();
    }
    IEnumerator Hit()
    {
        int countTime = 0;
        while(countTime < 10)
        {
            if (countTime % 2 == 0)
                spriteRenderer.color = new Color32(255, 255, 255, 90);
            else
                spriteRenderer.color = new Color32(255, 255, 255, 180);

            yield return new WaitForSeconds(0.2f);

            countTime++;
        }

        spriteRenderer.color = new Color32(255, 255, 255, 255);
        yield return null;
    }
}
