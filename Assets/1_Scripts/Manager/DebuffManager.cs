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

    private void DebuffOfGauss()
    {
        Debug.Log("<color=yellow>DebuffOfGauss</color>");

        GameObject bracket1 = Managers.Resource.Instantiate("Debuffs/GaussBrackets1", player.transform.parent);
        GameObject bracket2 = Managers.Resource.Instantiate("Debuffs/GaussBrackets2", player.transform.parent);

        CoroutineHandler.StartCoroutine(GaussBrackets(bracket1, bracket2));
    }

    IEnumerator GaussBrackets(GameObject bracket1, GameObject bracket2)
    {
        Vector3 b1Pos = bracket1.transform.position;
        Vector3 b1DestPos = new Vector3(-172f, b1Pos.y);

        Vector3 b2Pos = bracket2.transform.position;
        Vector3 b2DestPos = new Vector3(172f, b2Pos.y);

        while (true)
        {
            if (Mathf.Approximately(b1Pos.x, b1DestPos.x))
            {
                UnityEngine.Object.Destroy(bracket1);
                UnityEngine.Object.Destroy(bracket2);
                break;
            }
            bracket1.transform.position = Vector3.Lerp(bracket1.transform.position, b1DestPos, 0.1f);
            bracket1.transform.localScale = bracket1.transform.localScale * 0.1f;

            bracket2.transform.position = Vector3.Lerp(bracket1.transform.position, b2DestPos, 0.1f);
            bracket2.transform.localScale = bracket1.transform.localScale * 0.1f;

            yield return null;
        }
    }

    private void DebuffOfPythagoras()
    {
        Debug.Log("<color=red>DebuffOfPythagoras</color>");

    }

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