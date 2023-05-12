using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    [field: SerializeField]
    public TextMeshPro tmp { get; set ; }            // 화살의 Symbol이 표시될 TextMeshPro
    
}
