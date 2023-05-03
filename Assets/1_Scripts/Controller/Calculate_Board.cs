using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Calculate_Board : MonoBehaviour
{
    public GameObject target;

    public void Calculate()
    {
        Debug.Log("Calculate");

        DataTable table = new DataTable();      // ������ ��Ʈ������ �޾Ƽ� ���. ����� result�� ����.
        var result = table.Compute(GetComponentInChildren<TextMeshProUGUI>().text,"");
        GetComponentInChildren<TextMeshProUGUI>().text = "";

        if (target.GetComponent<TextMeshProUGUI>())     // ȭ�� �� ��� ����� ��� ���
        {
            Debug.Log(result.ToString());
            target.GetComponent<TextMeshProUGUI>().text = result.ToString();
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
