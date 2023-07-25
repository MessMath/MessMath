using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class Arrow : MonoBehaviour
{
    [field: SerializeField]
    public int type { get; set; }
    //public int num  {get; set;}
    //public char mathematicalSymbol  {get; set;}
    [field: SerializeField]
    public float speed { get; set; }
    [field: SerializeField]
    public Vector2 startPosition { get; set; }
    [field: SerializeField]
    public Vector2 direction { get; set; }
    //[field: SerializeField]
    //public TextMeshProUGUI tmp { get; set; }
    public string text;

    //public int type;
    //public float speed;
    //public Vector2 startPosition;
    //public Vector2 direction;
    public TextMeshProUGUI tmp;            // 화살의 Symbol이 표시될 TextMeshPro

    private void Awake()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        text = tmp.text;
    }

    private void OnValidate()
    {
        if (tmp != null)
            tmp.text = text;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DeadLine")
        {
            Destroy(gameObject);
        }

    }
}

