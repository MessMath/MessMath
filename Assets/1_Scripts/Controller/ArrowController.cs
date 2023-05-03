using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public string Symbol;       // Symbol�� Inspector���� �����ϰ� ������ �� �ֵ���,
    TextMeshPro tmp;            // ȭ���� Symbol�� ǥ�õ� TextMeshPro

    Vector2 dirVec;
    public GameObject player;

    float curShotDelay = 0f;
    float maxShotDelay = 0f;

    private void Start()
    {
        tmp = GetComponentInChildren<TextMeshPro>();
        tmp.text = Symbol;

        if (Symbol.Length > 1)  // Symbol�� ���̰� 1�� �Ѿ�ٸ� ������ 1���ڸ� �ڿ����� ��ü.
        {
            Debug.Log("Symbole Length should be 1!!");
            Symbol = Random.Range(0, 10).ToString();
            tmp.text = Symbol;
        }
        //
        maxShotDelay = Random.Range(1f, 2f);
        //
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
      
        GameObject testBullet = Managers.Resource.Instantiate("TestBullet");
        testBullet.transform.position = transform.position;
        Rigidbody2D testBulletRigid = testBullet.GetComponent<Rigidbody2D>();
        testBulletRigid.AddForce(dirVec.normalized * 10, ForceMode2D.Impulse);

        Debug.Log(" + bullet shot!");

        Destroy(testBullet, 2f);

        curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }


}
