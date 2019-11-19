using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSystem
{
    public bool IsRuning { get; set; }
    protected abstract void Initialize();
    public abstract void Update();
    public abstract void Release();
}
