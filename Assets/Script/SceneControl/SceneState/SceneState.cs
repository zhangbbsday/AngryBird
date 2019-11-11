using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneState
{
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
}
