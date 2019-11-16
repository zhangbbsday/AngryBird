using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public abstract class SceneState
{
    protected string[] stringMethod;
    protected SceneControl sceneControl;
    public SceneState(SceneControl control)
    {
        sceneControl = control;
    }
    /// <summary>
    /// 进入场景时执行
    /// </summary>
    public abstract void IntoScene();
    /// <summary>
    /// 场景更新
    /// </summary>
    public abstract void UpdateScene();
    /// <summary>
    /// 退出场景时执行
    /// </summary>
    public abstract void OutScene();


    protected virtual void AddStringMethod()
    {

    }
    protected virtual void LinkButton()
    {
        foreach (string name in stringMethod)
        {
            MethodInfo method = GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic);
            if (method.GetParameters().Length == 0)
                UIContainer.Instacne.FindUI<Button>(name).onClick.AddListener(() => { method.Invoke(this, null); });
            else
            {
                Button b = UIContainer.Instacne.FindUI<Button>(name);
                b.onClick.AddListener(() => { method.Invoke(this, new Button[] { b }); });
            }          
        }
    }

    protected virtual void LinkOtherUI()
    {

    }

}
