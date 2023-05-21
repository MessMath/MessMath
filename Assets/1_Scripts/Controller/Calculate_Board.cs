using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class Calculate_Board : MonoBehaviour
{
    public TextMeshProUGUI PrintNumber;
    public WitchController WitchController;
    public PlayerControllerCCF playerController;
    TextMeshProUGUI expression;
    int _idxOfHeart;

    private void Start()
    {
        PrintNumber.text = "";
        expression = GetComponentInChildren<TextMeshProUGUI>();
        expression.text = "";
        _idxOfHeart = 0;
        for (int i = 0; i < 3; i++)
        {
           // GameObject.Find($"Test_Player/Circle/heart{i}").SetActive(true);
        }
        
    }

    public void Calculate()
    {
        Debug.Log("Calculate");

        // 1. ???? ???? ???????? ???? ??? (e.g. 12+-) => Player?? Hp?? ????
        // 2. ???? ???? ????? Witch?? QusetionNumber?? ?????? => Witch?? Hp?? ????
        // 3. ???? ???? ????? Witch?? QusetionNumber?? ????? ?????? => Player?? Hp?? ????

        object result = null;
        string expressionToCalculate = expression.text.Replace("x","*");        // ????? ?????? ????!
        string printResult;

        expression.text = "";
        DataTable table = new DataTable();

        try
        {
            result = table.Compute(expressionToCalculate, "");     // ?????? ????????? ???? ???. ????? result?? ????.
            printResult = Math.Truncate(Convert.ToDouble(result)).ToString();
            Debug.Log($"\"{expressionToCalculate}\" result is : " + printResult);
        }
        catch (System.Exception e)                         // ?????? ??????? ???? ??? ???????
        {
            Debug.Log($"\"{expressionToCalculate}\" is inappropriate expression! : {e}");
            printResult = "";
            damageToPlayer(1);
            return;
        }

        if (PrintNumber)     // ??? ????? ??? ???
        {
            PrintNumber.text = $"={printResult}";
            StartCoroutine(Waitfor2Sec());
        }

        if(printResult == "")
            damageToPlayer(1);
        else if (int.Parse(printResult) == WitchController.QusetionNumber) // ??????? ???? ??????? (?????? ??????? ?? ?? ???? ????????)
            damageToWitch(15);
        else
            damageToPlayer(1);

    }

    IEnumerator Waitfor2Sec()           // 2?? ???? ?��? PrintNumber ???? ????
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
        _idxOfHeart++;
    }
    void damageToWitch(int damage)
    {
        //WitchController.Hp -= damage;
        //WitchController.HpBar.value = WitchController.Hp;
        WitchController.SetWitchHP(damage);
        WitchController.Questioning();
    }
}
