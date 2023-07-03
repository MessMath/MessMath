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

    // 학습 상태
    public Define.CurrentStatus CurrentStatus { get; set; }
    

    public string Name { get; set; }

    public int _idxOfHeart;
    public int Coin { get; set; }
    public int Damage { get; set; }
    public bool IsCorrect { get; set; }
    public string SelectedGrace { get; set; } = null;
    public string SelectedGrace1 { get; set; }
    public string SelectedGrace2 { get; set; }
    public int SelectGarceInx { get; set; } = 0;

    public bool IsTutorialFinished = false;

}