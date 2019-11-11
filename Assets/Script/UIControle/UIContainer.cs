using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIContainer : MonoBehaviour
{
    private List<string> uiName = null;
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
        uiName = new List<string>();
        AddName();
    }

    public T FindUI<T>(string name)
    {
        if (name == null || name.Length == 0 || !uiName.Contains(name))
            return default;
        
        return uiObject[uiName.IndexOf(name)].GetComponent<T>();
    }

    public GameObject FindGameObject(string name)
    {
        if (name == null || name.Length == 0 || !uiName.Contains(name))
            return default;

        return uiObject[uiName.IndexOf(name)];
    }

    private void AddName()
    {
        if (uiObject == null || uiObject.Count == 0)
            return;
        
        foreach(GameObject ui in uiObject)
        {
            uiName.Add(ui.name);
        }
    }
}
