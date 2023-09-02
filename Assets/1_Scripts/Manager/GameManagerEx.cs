using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Windows;
using static UI_Diagnosis;

public class GameManagerEx
{
    #region Input

    public Vector3 _input = Vector3.zero;

    public float Horizontal { get { return _input.x; } set { _input.x = value; } }
    public float Vertical { get { return _input.y; } set { _input.y = value; } }

    #endregion

    // �н� ����
    public Define.CurrentStatus CurrentStatus { get; set; }
    public Define.Mode CurrentMode { get; set; } = Define.Mode.None;

    public bool IsExisted { get; set; }

    public string Name { get; set; }

    public int _idxOfHeart;
    public int Damage { get; set; }
    public bool IsCorrect { get; set; }
    public int SelectGraceInx { get; set; } = 0;
}