using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    // 플레이어 위치를 찾고 벡터 뽑기
    // 벡터 방향으로 무언가(숫자, 기호) 던지기
    // playerController에서 기호, 숫자 불렛을 먹으면 수식 완성되는 코드 추가해야함.

    Vector2 dirVec;
    public GameObject player;

    float curShotDelay = 0f;
    float maxShotDelay = 0f;
    void Start()
    {
        maxShotDelay = Random.Range(1f, 2f);

    }

    void Update()
    {
        dirVec = player.transform.position - transform.position;

        Shot();
        Reload();
    }

    void Shot()
    {
        if (curShotDelay < maxShotDelay) return;
        int randValue = Random.Range(0, 3);
        switch (randValue)
        {
            case 0:
                GameObject testBullet = Managers.Resource.Instantiate("TestBullet");
                testBullet.transform.position = transform.position;
                Rigidbody2D testBulletRigid = testBullet.GetComponent<Rigidbody2D>();
                testBulletRigid.AddForce(dirVec.normalized * 10, ForceMode2D.Impulse);

                Debug.Log(" + bullet shot!");

                Destroy(testBullet, 2f);
                break;
            case 1:
                GameObject testBullet2 = Managers.Resource.Instantiate("TestBullet2");
                testBullet2.transform.position = transform.position;
                Rigidbody2D testBullet2Rigid = testBullet2.GetComponent<Rigidbody2D>();
                testBullet2Rigid.AddForce(dirVec.normalized * 10, ForceMode2D.Impulse);

                Debug.Log(" 10 bullet shot!");

                Destroy(testBullet2, 2f);
                break;
            case 2:
                GameObject testBullet3 = Managers.Resource.Instantiate("TestBullet3");
                testBullet3.transform.position = transform.position;
                Rigidbody2D testBullet3Rigid = testBullet3.GetComponent<Rigidbody2D>();
                testBullet3Rigid.AddForce(dirVec.normalized * 10, ForceMode2D.Impulse);

                Debug.Log(" - bullet shot!");

                Destroy(testBullet3, 2f);
                break;
        }
        

        curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }
}
