using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;

/// <summary>
/// 모든 디버프를 관리하는 Manager
/// </summary>
public class DebuffManager
{
    PlayerControllerCCF player;
    Vector3 playerPos;
    WitchController witch;

    /// <summary>
    /// 모든 디버프는 호출될 때 Setup()을 맨처음에 호출해야 한다.
    /// </summary>
    public void Setup()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerCCF>();
        playerPos = player.transform.position;
        witch = GameObject.FindGameObjectWithTag("Witch").GetComponent<WitchController>();
    }

    /// <summary>
    /// 모든 디버프는 CallDebuff()를 통해서 한다.
    /// </summary>
    /// <param name="MathMTCName">수학자의 이름을 입력</param>
    public void CallDebuff(string MathMTCName)
    {
        Setup();
        switch (MathMTCName)
        {
            case "Gauss":
                DebuffOfGauss();
                break;
            case "Pythagoras":
                DebuffOfPythagoras();
                break;
            case "Newton":
                DebuffOfNewton();
                break;
        }
    }

    /// <summary>
    /// 가우스의 디버프 : 가우스 기호로 플레이어의 진로를 방해한다.
    /// </summary>
    private void DebuffOfGauss()
    {
        Debug.Log("<color=yellow>DebuffOfGauss</color>");

        GameObject bracket1 = Managers.Resource.Instantiate("Debuffs/GaussBrackets1", player.transform.parent);
        GameObject bracket2 = Managers.Resource.Instantiate("Debuffs/GaussBrackets2", player.transform.parent);

        CoroutineHandler.StartCoroutine(GaussBrackets(bracket1, bracket2));
    }

    /// <summary>
    /// 가우스 기호 소환
    /// </summary>
    /// <param name="bracket1">기호 오브젝트1</param>
    /// <param name="bracket2">기호 오브젝트2</param>
    /// <returns></returns>
    IEnumerator GaussBrackets(GameObject bracket1, GameObject bracket2)
    {
        Vector3 b1Pos = bracket1.transform.localPosition;
        Vector3 b1DestPos = new Vector3(-172.0f, b1Pos.y, b1Pos.z);

        Vector3 b2Pos = bracket2.transform.localPosition;
        Vector3 b2DestPos = new Vector3(172f, b2Pos.y, b2Pos.z);

        while (true)
        {
            if (Vector3.Distance(bracket1.transform.localPosition, b1DestPos) < 0.05f)
            {
                GameObject.Destroy(bracket1,2);
                GameObject.Destroy(bracket2,2);
                break;
            }
            else
            {
                bracket1.transform.localPosition = Vector3.MoveTowards(bracket1.transform.localPosition, b1DestPos, 10f);
                bracket1.transform.localScale = bracket1.transform.localScale * 0.99f;

                bracket2.transform.localPosition = Vector3.MoveTowards(bracket2.transform.localPosition, b2DestPos, 10f);
                bracket2.transform.localScale = bracket2.transform.localScale * 0.99f;
            }
            yield return null;
        }
    }
    
    /// <summary>
    /// 피타고라스의 디버프
    /// </summary>
    private void DebuffOfPythagoras()
    {
        Debug.Log("<color=red>DebuffOfPythagoras</color>");

    }

    /// <summary>
    /// 뉴턴의 디버프
    /// </summary>
    private void DebuffOfNewton()
    {
        Debug.Log("<color=blue>DebuffOfNewton</color>");

        // UptoSky에서 블랙홀
        //// 블랙홀
        //if (collision.gameObject.name == "BlackHole")
        //{
        //    Vector3 dir = collision.gameObject.transform.position - gameObject.transform.position;
        //    Vector3.Normalize(dir);
        //    _rb.AddForce(dir * 70, ForceMode2D.Force);
        //    return;
        //}

    }

}