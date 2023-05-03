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

        DataTable table = new DataTable();      // 수식을 스트링으로 받아서 계산. 결과를 result에 저장.
        var result = table.Compute(GetComponentInChildren<TextMeshProUGUI>().text,"");
        GetComponentInChildren<TextMeshProUGUI>().text = "";

        if (target.GetComponent<TextMeshProUGUI>())     // 화면 정 가운에 결과를 잠깐 출력
        {
            Debug.Log(result.ToString());
            target.GetComponent<TextMeshProUGUI>().text = result.ToString();
            StartCoroutine(Waitfor2Sec());
            
        }
    }

    IEnumerator Waitfor2Sec()
    {
        yield return new WaitForSeconds(2.0f);  // 2초 지연 후에 글자 삭제
        Debug.Log("Wait2Sec");
        target.GetComponent<TextMeshProUGUI>().text = "";
    }
}
