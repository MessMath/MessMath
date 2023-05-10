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

    private void Start()
    {
        PrintNumber.text = "";
        expression = GetComponentInChildren<TextMeshProUGUI>();
        expression.text = "";
    }

    public void Calculate()
    {
        Debug.Log("Calculate");

        // 1. ���� ���� �������� ���� ��� (e.g. 12+-) => Player�� Hp�� ����
        // 2. ���� ���� ����� Witch�� QusetionNumber�� ��ġ�ϸ� => Witch�� Hp�� ����
        // 3. ���� ���� ����� Witch�� QusetionNumber�� ��ġ�� ������ => Player�� Hp�� ����

        object result = null;
        string expressionToCalculate = expression.text;
        string printResult;

        expression.text = "";
        DataTable table = new DataTable();

        try
        {
            result = table.Compute(expressionToCalculate, "");     // ������ ��Ʈ������ �޾Ƽ� ���. ����� result�� ����.
            printResult = result.ToString();
        }
        catch (System.Exception e)                         // ������ �Ұ����� ���� ��� ����ó��
        {
            Debug.Log($"inappropriate expression! : {e}");
            printResult = "";
            damageToPlayer(1);
            return;
        }

        if (PrintNumber)     // ��� ����� ��� ���
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

    IEnumerator Waitfor2Sec()           // 2�� ���� �Ŀ� PrintNumber �ؽ�Ʈ ����
    {
        yield return new WaitForSeconds(2.0f);  
        Debug.Log("Wait2Sec");
        PrintNumber.text = "";
    }
    void damageToPlayer(int damage)
    {
        playerController.Hp -= damage;
    }
    void damageToWitch(int damage)
    {
        WitchController.Hp -= damage;
    }

}
