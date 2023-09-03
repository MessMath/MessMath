using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// 모든 디버프를 관리하는 Manager
/// </summary>
public class DebuffManager
{
    PlayerController player;
    Vector3 playerPos;
    WitchController witch;

    // 각 디버프들의 실행 중 유무 bool
    public bool gaussOn = false;
    public bool newtonOn = false;
    List<string> obtainedCollections = new List<string>();

    /// <summary>
    /// 모든 디버프는 호출될 때 Setup()을 맨처음에 호출해야 한다.
    /// </summary>
    public void Setup()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerPos = player.transform.localPosition;
        witch = GameObject.FindGameObjectWithTag("Witch").GetComponent<WitchController>();

        InitObtainedClothes();
    }

    async void InitObtainedClothes()
    {
        obtainedCollections = Managers.DBManager.ParseObtanined(await Managers.DBManager.GetObtainedCollections(Managers.GoogleSignIn.GetUID()));
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
        if (gaussOn) return; gaussOn = true;
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
        gaussOn = false;
    }
    
    /// <summary>
    /// 피타고라스의 디버프
    /// </summary>
    private void DebuffOfPythagoras()
    {
        Debug.Log("<color=red>DebuffOfPythagoras</color>");
        GameObject SoPyth = Managers.Resource.Instantiate("Debuffs/ShadowOfPythagoras",player.transform.parent);
        GameObject.Destroy(SoPyth,3f);
    }

    /// <summary>
    /// 뉴턴의 디버프
    /// </summary>
    private void DebuffOfNewton()
    {
        if (newtonOn) return; newtonOn = true;
        Debug.Log("<color=blue>DebuffOfNewton</color>");

        GameObject Apple = Managers.Resource.Instantiate("Debuffs/AppleOfNewton", player.transform.parent);

        if (CheckHaveNewtonApple())
            Apple.gameObject.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Sprites/Collections/newton_apple");

        CoroutineHandler.StartCoroutine(NewtonApple(Apple));
    }

    bool CheckHaveNewtonApple()
    {
        if (obtainedCollections == null) return false;

        for (int i = 0; i < obtainedCollections.Count; i++)
        {
            if (obtainedCollections[i] == "newton_apple") return true;
        }

        return false;
    }

    /// <summary>
    /// 뉴턴 사과 던지기 & 중력효과
    /// </summary>
    /// <param name="apple">사과 오브젝트</param>
    /// <returns></returns>
    IEnumerator NewtonApple(GameObject apple)
    {
        float rotateDegreePerSec = 90f;
        RectTransform appleRect = apple.GetComponent<RectTransform>();

        apple.GetComponent<Rigidbody2D>().AddForce(Vector3.Normalize(playerPos - apple.transform.localPosition) * 1000f, ForceMode2D.Impulse);
        while(apple != null)
        {
            if (apple.transform.position.x < 0 - appleRect.sizeDelta.x * 0.5f ||
               apple.transform.position.y < 0 - appleRect.sizeDelta.y * 0.5f ||
               apple.transform.position.y > Camera.main.pixelHeight + appleRect.sizeDelta.y * 0.5f)
            {
                Debug.Log("end NewtonApple!");
                GameObject.Destroy(apple);
                break;
            }
            Vector3 dir = apple.transform.localPosition - player.transform.localPosition;
            Vector3.Normalize(dir);
            player.GetComponent<Rigidbody2D>().AddForce(dir * 30, ForceMode2D.Force);
            apple.transform.Rotate(Vector3.forward, Time.deltaTime * rotateDegreePerSec);

            yield return null;
        }

        if(player != null)
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        newtonOn = false;
    }

}