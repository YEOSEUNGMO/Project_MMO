using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');      // '/' 가있는 마지막 인덱스받아오기. - 경로의 마지막 이름을 받아오기위해
            if (index >= 0)
                name = name.Substring(index + 1);   // '/' 뒤부터 마지막까지 문자받아오기. -> 파일이름.

            GameObject go = Managers.Pool.GetOriginal(name);
            if (go != null)
                return go as T;
        }
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.LogError($"Failedd to load prefab : {path}");
            return null;
        }

        if(original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original,parent).gameObject;

        GameObject go = Object.Instantiate(original, parent);
        int index = go.name.IndexOf("(Clone)");
        if (index > 0)
            go.name = go.name.Substring(0, index);
        return go;
    }

    public void Destroy(GameObject obj)
    {
        if (obj == null)
            return;

        Poolable poolable = obj.GetComponent<Poolable>();
        if (poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(obj);
    }
}
