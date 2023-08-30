using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;

public class UI_DamageText : MonoBehaviour
{
    [SerializeField] float moveSpeed = 7f; // 위로 움직이는 속도 값
    [SerializeField] float destroyTime = 2f; // 몇 초 후 삭제할 건지
    [SerializeField] float alphaSpeed = 2f;

    public int damage;

    TMP_Text text;
    Color alpha;

    void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
        alpha = text.color;
        text.text = damage.ToString();
        Invoke("DestroyObject", destroyTime);
    }

    void Update()
    {
        GetComponentInChildren<TMP_Text>().transform.position += Vector3.up;
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        text.color = alpha;
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
