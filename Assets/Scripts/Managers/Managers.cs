using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    #region  Contents
    GameManager _game = new GameManager();
    public static GameManager Game { get { return Instantance._game; } }
    #endregion
    #region  Core
    static Managers s_instance = null;
    public static Managers Instantance { get { Init(); return s_instance; } }

    InputManager _input = new InputManager();
    public static InputManager Input { get { return Instantance._input; } }

    ResourceManager _resource = new ResourceManager();
    public static ResourceManager Resource { get { return Instantance._resource; } }

    UIManager _ui = new UIManager();
    public static UIManager UI { get { return Instantance._ui; } }

    SceneManagerEX _scene = new SceneManagerEX();
    public static SceneManagerEX Scene { get { return Instantance._scene; } }

    SoundManager _sound = new SoundManager();
    public static SoundManager Sound { get { return Instantance._sound; } }

    DataManager _data = new DataManager();
    public static DataManager Data { get { return Instantance._data; } }

    PoolManager _pool = new PoolManager();
    public static PoolManager Pool { get { return Instantance._pool; } }
    #endregion

    private void Awake()
    {
        Init();
    }
    public static void Init()
    {
        if (s_instance == null)
        {
            GameObject obj = GameObject.Find("@Managers");
            if (obj == null)
            {
                obj = new GameObject { name = "@Managers" };
                obj.AddComponent<Managers>();
            }
            DontDestroyOnLoad(obj);
            s_instance = obj.GetComponent<Managers>();
            s_instance._pool.Init();
            s_instance._sound.Init();
            s_instance._data.Init();
        }
    }
    public static void Clear()
    { 
        Input.Clear();
        Sound.Clear();
        Scene.Clear();
        UI.Clear();

        Pool.Init();
    }

    void Update()
    {
        _input.OnUpdate();
    }
}
