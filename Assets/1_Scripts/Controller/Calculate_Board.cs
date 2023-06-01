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
            GameObject.Find($"Player/Circle/heart{i}").SetActive(true);
        }
        //spriteRenderer = playerController.GetComponent<SpriteRenderer>();
        
    }

    public void Calculate()
    {
        Debug.Log("Calculate");

        object result = null;
        string expressionToCalculate = expression.text.Replace("x","*"); 
        string printResult;

        expression.text = "";
        DataTable table = new DataTable();

        try
        {
            result = table.Compute(expressionToCalculate, ""); 
            printResult = Math.Truncate(Convert.ToDouble(result)).ToString();
            Debug.Log($"\"{expressionToCalculate}\" result is : " + printResult);
        }
        catch (System.Exception e) 
        {
            Debug.Log($"\"{expressionToCalculate}\" is inappropriate expression! : {e}");
            printResult = "";
            damageToPlayer(1);
            return;
        }

        if (PrintNumber)
        {
            PrintNumber.text = $"={printResult}";
            StartCoroutine(Waitfor2Sec());
        }

        if(printResult == "")
            damageToPlayer(1);
        else if (int.Parse(printResult) == WitchController.QusetionNumber)
            damageToWitch(15);
        else
            damageToPlayer(1);

    }

    IEnumerator Waitfor2Sec()
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
        GameObject.Find($"Player/Circle/heart{_idxOfHeart}").SetActive(false);
        playerController.BlinkPlayerImg();
        if (_idxOfHeart > playerController.Hp)
            return;
        else
            _idxOfHeart++;
    }

    void damageToWitch(int damage)
    {
        WitchController.SetWitchHP(damage);
        WitchController.Questioning();
    }
}
