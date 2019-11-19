using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : BaseSystem
{
    public InputSystem()
    {
        Initialize();
    }

    public override void Release()
    {
        IsRuning = false;
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }

    protected override void Initialize()
    {
        IsRuning = true;
    }
}
