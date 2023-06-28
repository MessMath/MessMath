using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 모든 가호를 관리하는 Manager
/// </summary>
public class GraceManager
{
    PlayerControllerCCF player;
    WitchController witch;
    Vector3 playerPos;

    public bool gaussOn = false;

    public void Setup()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerCCF>();
        playerPos = player.transform.position;
        witch = GameObject.FindGameObjectWithTag("Witch").GetComponent<WitchController>();

    }

    /// <summary>
    /// 가호 호출은 모두 CallGrace()를 통해서 한다. Managers.Grace.CallGrace("GraceOfGauss")
    /// </summary>
    /// <param name="graceName"> 가호의 이름을 입력 예. CallGrace </param>
    public void CallGrace(string graceName)
    {
        switch (graceName)
        {
            case "GraceOfGauss":
                GraceOfGauss();
                break;
        }
    }

    /// <summary>
    /// 가우스의 가호 : 세레스 공전, 이 다음 공격 데미지를 두배로.
    /// </summary>
    public void GraceOfGauss()
    {
        if (gaussOn) return;
        gaussOn = true;
        Setup();
        float radius = 200f;
        float angle = 0f;
        float speed = 2f;
        int prevWitchHp = witch.Hp;

        // 공격 데미지 2배
        Managers.Game.Damage *= 2;

        GameObject Ceres = Managers.Resource.Instantiate("Grace/Ceres", player.transform);
        Ceres.transform.position = new Vector3(playerPos.x - radius, playerPos.y, 0);

        // 세레스 돌리기
        CoroutineHelper.StartCoroutine(RotateCeres(Ceres, player.transform, radius, angle, speed, prevWitchHp));
    }

    /// <summary>
    /// 세레스를 계속 돌린다. 마녀가 데미지를 받으면 멈춤.
    /// </summary>
    /// <param name="Ceres">세레스 오브젝트</param>
    /// <param name="center">중심으로 잡을 오브젝트</param>
    /// <param name="radius">반지름</param>
    /// <param name="angle">시작각도</param>
    /// <param name="speed">속도</param>
    /// <param name="prevWitchHp">마녀의 이전 체력</param>
    IEnumerator RotateCeres(GameObject Ceres, Transform center, float radius, float angle, float speed, int prevWitchHp)
    {
        while(true)
        {
            angle += speed * Time.deltaTime;
            Ceres.transform.position = center.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

            if (prevWitchHp != witch.Hp)
            {
                Debug.Log("witch hp : " + witch.Hp);
                Managers.Game.Damage = Managers.Game.Damage / 2;
                Object.Destroy(Ceres.gameObject);
                gaussOn = false;
                yield break;
            }

            yield return null;
        }
    }

}