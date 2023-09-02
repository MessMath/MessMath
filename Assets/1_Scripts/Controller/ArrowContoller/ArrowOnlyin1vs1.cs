using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class ArrowOnlyin1vs1 : MonoBehaviour
{
    [field: SerializeField]
    public int type { get; set; }
    [field: SerializeField]
    public float speed { get; set; }
    [field: SerializeField]
    public Vector2 startPosition { get; set; }
    [field: SerializeField]
    public Vector2 direction { get; set; }
    public string text;

    public TEXDraw tmp;            // 화살의 Symbol이 표시될 TextMeshPro

    private void Start()
    {
        Destroy(gameObject, 20f);
    }

    private void OnValidate()
    {
        if (tmp != null)
            tmp.text = text;
    }

}

