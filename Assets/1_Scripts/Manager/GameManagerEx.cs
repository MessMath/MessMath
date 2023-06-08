using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class GameManagerEx
{
    #region Input

    public Vector3 _input = Vector3.zero;

    public float Horizontal { get { return _input.x; } set { _input.x = value; } }
    public float Vertical { get { return _input.y; } set { _input.y = value; } }

    #endregion

    public int _idxOfHeart;
}
