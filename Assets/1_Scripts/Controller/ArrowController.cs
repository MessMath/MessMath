using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public string Symbol;       // Symbol�� Inspector���� �����ϰ� ������ �� �ֵ���,
    TextMeshPro tmp;            // ȭ���� Symbol�� ǥ�õ� TextMeshPro

    private void Start()
    {
        tmp = GetComponentInChildren<TextMeshPro>();
        tmp.text = Symbol;

        if (Symbol.Length > 1)  // Symbol�� ���̰� 1�� �Ѿ�ٸ� ������ 1���ڸ� �ڿ����� ��ü.
        {
            Debug.Log("Symbole Length should be 1!!");
            Symbol = Random.Range(0, 10).ToString();
            tmp.text = Symbol;
        }

    }
}
