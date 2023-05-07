using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawnerController : MonoBehaviour
{
    float curShotDelay = 0f;
    float maxShotDelay = 0f;
    EdgeCollider2D edgeCollider;
    Vector2[] points;

    void Awake()
    {
        maxShotDelay = Random.Range(1f, 2f);
        edgeCollider = GetComponent<EdgeCollider2D>();

        #region EdgeCollider를 포물선으로 정하는 코드
        points = edgeCollider.points;
        float xStep = Mathf.Abs(points[0].x - points[points.Length - 1].x)/(points.Length - 1);

        for (int i = 0; i < points.Length; i++)
        {
            if (i == 0)
            {
                points[i].y = (float)(points[i].x * points[i].x * (-0.025));
                continue;
            }
            points[i].x = points[i - 1].x + xStep;
            points[i].y = (float)(points[i].x * points[i].x * (-0.025));
        }
        edgeCollider.points = points;
        #endregion
    }

    void Update()
    {
        Shot();
        Reload();
    }

    void Shot()
    {
        if (curShotDelay < maxShotDelay) return;
        int randValue = Random.Range(0, 3);
        GameObject testBullet = Managers.Resource.Instantiate("TestBullet");
        testBullet.transform.position = edgeCollider.points[Random.Range(0, edgeCollider.points.Length)];

        Debug.Log(" + bullet shot!");

        Destroy(testBullet, 2f);

        curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }
}
