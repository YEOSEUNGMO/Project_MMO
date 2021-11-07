using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public abstract class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> objects = new Dictionary<Type, UnityEngine.Object[]>();
    protected enum Buttons
    {
        PointButton,
    }
    protected enum Texts
    {
        PointText,
        ScoreText,
    }
    protected enum GameObjects
    {
        TestObject,
    }
    protected enum Images
    {
        ItemIcon,
    }

    public abstract void Init();
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objs = new UnityEngine.Object[names.Length];
        objects.Add(typeof(T), objs);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objs[i] = Util.FindChild(gameObject, names[i], true);
            else
                objs[i] = Util.FindChild<T>(gameObject, names[i], true);
        }
    }

    protected T Get<T>(int index) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objs = null;
        if (objects.TryGetValue(typeof(T), out objs) == false)
        {
            return null;
        }

        return objs[index] as T;
    }
    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
    protected Text GetText(int index) { return Get<Text>(index); }

    protected Button GetButton(int index) { return Get<Button>(index); }

    protected Image GetImage(int index) { return Get<Image>(index); }

    public static void BindEvent(GameObject go, Action<PointerEventData> action,Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);
        switch(type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHandler += action;
                break;
        }
    }
}
