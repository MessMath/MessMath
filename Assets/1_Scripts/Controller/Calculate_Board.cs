using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class Calculate_Board : MonoBehaviour
{
    public GameObject target;

    public void Calculate()
    {
        Debug.Log("Calculate");

        object result;
        String printResult;

        try
        {
            DataTable table = new DataTable();      // ������ ��Ʈ������ �޾Ƽ� ���. ����� result�� ����.
            result = table.Compute(GetComponentInChildren<TextMeshProUGUI>().text, "");
            printResult = result.ToString();

        }
        catch(Exception e)                         // ������ �Ұ����� ���� ��� ����ó��
        {
            printResult = "";
        }

        GetComponentInChildren<TextMeshProUGUI>().text = "";

        if (target.GetComponent<TextMeshProUGUI>())     // ȭ�� �� ��� ����� ��� ���
        {
            target.GetComponent<TextMeshProUGUI>().text = printResult;
            StartCoroutine(Waitfor2Sec());
        }
    }

    IEnumerator Waitfor2Sec()
    {
        yield return new WaitForSeconds(2.0f);  // 2�� ���� �Ŀ� ���� ����
        Debug.Log("Wait2Sec");
        target.GetComponent<TextMeshProUGUI>().text = "";
    }
}
