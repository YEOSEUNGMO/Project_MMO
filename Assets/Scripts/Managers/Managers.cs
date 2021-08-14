using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance = null;
    public static Managers Instantance { get{ Init(); return s_instance; } }

    InputManager _input = new InputManager();
    public static InputManager Input { get { return Instantance._input; } }

    ResourceManager _resource = new ResourceManager();
    public static ResourceManager Resource { get { return Instantance._resource; } }

    private void Awake()
    {
        Init();
    }
    public static void Init()
    {
        if(s_instance == null)
        {
            GameObject obj = GameObject.Find("@Managers");
            if (obj == null)
            {
                obj = new GameObject { name = "@Managers" };
                obj.AddComponent<Managers>();
            }
            DontDestroyOnLoad(obj);
            s_instance = obj.GetComponent<Managers>();
        }
    }

    void Update()
    {
        _input.OnUpdate();
    }
}
