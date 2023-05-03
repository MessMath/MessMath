using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public string Symbol;       // Symbol을 Inspector에서 간편하게 설정할 수 있도록,
    TextMeshPro tmp;            // 화살의 Symbol이 표시될 TextMeshPro

    private void Start()
    {
        tmp = GetComponentInChildren<TextMeshPro>();
        tmp.text = Symbol;

        if (Symbol.Length > 1)  // Symbol의 길이가 1이 넘어간다면 임의의 1의자리 자연수로 대체.
        {
            Debug.Log("Symbole Length should be 1!!");
            Symbol = Random.Range(0, 10).ToString();
            tmp.text = Symbol;
        }

    }
}
