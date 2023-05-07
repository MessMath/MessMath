using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawnerController : MonoBehaviour
{
    float curShotDelay = 0f;
    float maxShotDelay = 0f;

    void Start()
    {
        maxShotDelay = Random.Range(1f, 2f);

        Vector2[] points = GetComponent<EdgeCollider2D>().points;
        float xStep = Mathf.Abs(points[0].x - points[points.Length - 1].x)/points.Length;



        for (int i = 0; i < points.Length;i++)
        {
            if (i == 0) continue;
            points[i].x = points[i - 1].x + xStep;
            //GetComponent<EdgeCollider2D>().points[i].y = (float)(points[i].x * points[i].x * (-0.05));

            GetComponent<EdgeCollider2D>().points[i] = new Vector2(points[i].x, points[i].y);
        }
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
        testBullet.transform.position = transform.position;

        Debug.Log(" + bullet shot!");

        Destroy(testBullet, 2f);

        curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }
}
