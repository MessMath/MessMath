using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int type {get; set;}
    public int num  {get; set;}
    public char mathematicalSymbol  {get; set;}
    public float speed  {get; set;}
    public Vector2 startPosition {get; set;}
    public Vector2 direction  {get; set;}

    void Update()
    {
        // TODO 플레이어와 콜라이더 충돌 시 파괴되는 기능 추가
    }
}
