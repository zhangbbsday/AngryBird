using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIContainer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> uiObject = null;

    private static UIContainer instance;
    public static UIContainer Instacne
    {
        get
        {
            if (instance != null)
                return instance;

            Debug.LogWarning("不存在UIContainer!");
            return null;
        }
    }

    private void Start()
    {
        instance = GetComponent<UIContainer>();
    }

    public T FindUI<T>(string name)
    {
        if (name == null || name.Length == 0)
            return default;
        T find = default;

        foreach (GameObject ui in uiObject)
        {
            if (ui.name == name)
            {
                find = ui.GetComponent<T>();
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

        foreach (GameObject ui in uiObject)
        {
            if (ui.name == name)
            {
                find = ui;
                break;
            }
        }

        return find;
    }
}
