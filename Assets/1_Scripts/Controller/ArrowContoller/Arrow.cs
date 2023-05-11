using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int type { get; set; }
    //public int num  {get; set;}
    //public char mathematicalSymbol  {get; set;}
    public float speed { get; set; }
    public Vector2 startPosition { get; set; }
    public Vector2 direction { get; set; }
    public TextMeshPro tmp { get; set; }            // 화살의 Symbol이 표시될 TextMeshPro

    void Update()
    {

    }
}
