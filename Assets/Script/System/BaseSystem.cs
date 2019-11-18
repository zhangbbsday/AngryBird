using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSystem
{
    protected abstract void Initialize();
    public abstract void Update();
    public abstract void Release();
}
