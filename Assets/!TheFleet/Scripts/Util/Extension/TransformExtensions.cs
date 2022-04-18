using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    public static void DestroyAllChildren(this Transform transform)
    {
        foreach (Transform t in transform)
            Object.Destroy(t.gameObject);
    }
    public static void DestroyAllChildrenExcept(this Transform transform, Transform except)
    {
        foreach (Transform t in transform)
            if (t != except)
                Object.Destroy(t.gameObject);
    }
    public static void DestroyAllChildren(this GameObject gameObject)
    {
        foreach (Transform t in gameObject.transform)
            Object.Destroy(t.gameObject);
    }

    public static RectTransform GetRectTransform(this Component c)
    {
        return (RectTransform)c.transform;
    }
    public static RectTransform GetRectTransform(this GameObject gameObject)
    {
        return (RectTransform)gameObject.transform;
    }

    public static T GetComponentAndAddIfNotExist<T>(this Component c) where T : Component
    {
        T component = c.GetComponent<T>();
        if (component)
            return component;
        return c.gameObject.AddComponent<T>();
    }
    public static T GetComponentAndAddIfNotExist<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        if (component)
            return component;
        return gameObject.AddComponent<T>();
    }

    public static void AddComponentIfNotExist<T>(this Component c) where T : Component
    {
        if (c.GetComponent<T>())
            return;
        c.gameObject.AddComponent<T>();
    }

    public static void AddComponentIfNotExist<T>(this GameObject gameObject) where T : Component
    {
        if (gameObject.GetComponent<T>())
            return;
        gameObject.AddComponent<T>();
    }
}
