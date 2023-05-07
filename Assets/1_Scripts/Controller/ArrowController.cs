using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public string Symbol;       // Symbol�� Inspector���� �����ϰ� ������ �� �ֵ���,
    TextMeshPro tmp;            // ȭ���� Symbol�� ǥ�õ� TextMeshPro
    string[] Operator = { "+", "-", "��", "��" };
    Vector2 dirVec;
    public GameObject player;

    private void Start()
    {
        tmp = GetComponentInChildren<TextMeshPro>();
        tmp.text = Symbol;

        if (Symbol.Length > 1)  // Symbol�� ���̰� 1�� �Ѿ�ٸ� ������ 1���ڸ� �ڿ����� ��ü.
        {
            Debug.Log("Symbole Length should be 1!!");

            if (Random.Range(0, 2) == 1)
                Symbol = Random.Range(0, 10).ToString();            // 50%�� Ȯ���� Symbol�� 0~9�� ���ڰ�
            else
                Symbol = Operator[Random.Range(0, 4)].ToString();   // 50%�� Ȯ���� Symbol�� ��Ģ���� �� �ϳ��� ��ȣ�� �ش��Ѵ�.

            tmp.text = Symbol;
        }

        if (player == null)     
            player = GameObject.FindGameObjectWithTag("Player"); // Player�� ã�����ϴ� ������ ��ġ�� ���� �ڵ�

        LookAt(player);     // Player�� �ٶ󺸰� ���󰡰Բ�

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
