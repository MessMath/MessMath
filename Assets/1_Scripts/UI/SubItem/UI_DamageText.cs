using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;

public class UI_DamageText : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f; // 위로 움직이는 속도 값
    [SerializeField] float destroyTime = 3f; // 몇 초 후 삭제할 건지

    Vector2 vector;

    void Update()
    {
        vector.Set(gameObject.transform.position.x, gameObject.transform.position.y + (moveSpeed + Time.deltaTime));
        gameObject.transform.position = vector;

        destroyTime -= Time.deltaTime;

        if (destroyTime <= 0) Destroy(this.gameObject);
    }
}
