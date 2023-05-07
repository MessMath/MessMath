using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public string Symbol;       // Symbol을 Inspector에서 간편하게 설정할 수 있도록,
    TextMeshPro tmp;            // 화살의 Symbol이 표시될 TextMeshPro
    string[] Operator = { "+", "-", "×", "÷" };
    Vector2 dirVec;
    public GameObject player;

    private void Start()
    {
        tmp = GetComponentInChildren<TextMeshPro>();
        tmp.text = Symbol;

        if (Symbol.Length > 1)  // Symbol의 길이가 1이 넘어간다면 임의의 1의자리 자연수로 대체.
        {
            Debug.Log("Symbole Length should be 1!!");

            if (Random.Range(0, 2) == 1)
                Symbol = Random.Range(0, 10).ToString();            // 50%의 확률로 Symbol이 0~9의 숫자고
            else
                Symbol = Operator[Random.Range(0, 4)].ToString();   // 50%의 확률로 Symbol이 사칙연산 중 하나의 기호에 해당한다.

            tmp.text = Symbol;
        }

        if (player == null)     
            player = GameObject.FindGameObjectWithTag("Player"); // Player를 찾지못하는 오류를 고치기 위한 코드

        LookAt(player);     // Player를 바라보고 날라가게끔

        dirVec = player.transform.position - transform.position;
        GetComponent<Rigidbody2D>().AddForce(dirVec.normalized * 10, ForceMode2D.Impulse);

    }

    void LookAt(GameObject target)
    {
        if(target != null)
        {
            Vector2 direction = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion angleAxis = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
            Quaternion rotation = Quaternion.Slerp(transform.rotation, angleAxis, 1);

            transform.rotation = rotation;
        }
    }
}
