using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WjChallenge;

public class WJ_DisplayText : MonoBehaviour
{
    [SerializeField] Text textCurrentState;

    string state        = "-";
    string myAnswer     = "-";
    string isCorrect    = "-";
    string svTime       = "-";

    void Start()
    {
        
    }

    /// <summary>
    /// 현재 문제풀이 상태를 UI에 표시
    /// </summary>
    /// <param name="state">현재 상태</param>
    /// <param name="myAnswer">내가 마지막으로 고른 정답</param>
    /// <param name="isCorrect">마지막으로 고른 것이 정답인지</param>
    /// <param name="svTime">풀이시간</param>
    public void SetState(string state, string myAnswer, string isCorrect, string svTime)
    {
        this.state      = state       != ""   ? state     : this.state ;
        this.myAnswer   = myAnswer    != ""   ? myAnswer  : this.myAnswer ;
        this.isCorrect  = isCorrect   != ""   ? isCorrect : this.isCorrect ;
        this.svTime     = svTime      != ""   ? svTime    : this.svTime ;

        textCurrentState.text =
            $"현재 상태 : {this.state}\n" +
            $"최근 선택한 답 : {this.myAnswer}\n" +
            $"최근 정답 여부 : {this.isCorrect}\n" +
            $"최근 풀이 시간 : {this.svTime}\n";
    }
}
