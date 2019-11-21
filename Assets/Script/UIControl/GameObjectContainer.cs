using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameObjectContainer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> uiObject = null;

    private static GameObjectContainer instance;
    public static GameObjectContainer Instacne
    {
        get
        {
            if (instance != null)
                return instance;

            Debug.LogWarning("不存在GameObjectContainer!");
            return null;
        }
    }

    private void Start()
    {
        instance = GetComponent<GameObjectContainer>();
    }

    public T FindGameObjectComponent<T>(string name)
    {
        if (name == null || name.Length == 0)
            return default;
        T find = default;

        foreach (GameObject g in uiObject)
        {
            if (g.name == name)
            {
                find = g.GetComponent<T>();
                break;
            }
        }

        return find;
    }

    public GameObject FindGameObject(string name)
    {
        if (name == null || name.Length == 0)
            return default;

        GameObject find = default;

        foreach (GameObject g in uiObject)
        {
            if (g.name == name)
            {
                find = g;
                break;
            }
        }

        return find;
    }
}
