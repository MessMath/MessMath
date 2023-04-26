using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;
using static Define;

public class Utils
{
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            Transform transform = go.transform.Find(name);
            if (transform != null)
                return transform.GetComponent<T>();
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform != null)
            return transform.gameObject;
        return null;
    }

    public static GameObject FindNearestObject(string tag, Vector3 pos)  // Scene에서 가장 가까운 게임오브젝트를 반환한다
    {
        GameObject nearestGo = null;
        float distance = float.MaxValue;
        float minDistance = 0.001f;
        GameObject[] Gos = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject Go in Gos)
        {
            float distance2 = Mathf.Abs((Go.transform.position - pos).magnitude);
            if (Go != null && distance2 < distance && minDistance < distance2)
            {
                nearestGo = Go;
                distance = distance2;
            }
        }

        return nearestGo;
    }

}
