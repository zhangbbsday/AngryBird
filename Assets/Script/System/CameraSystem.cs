using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : BaseSystem
{
    private Camera mainCamera;
    public CameraSystem()
    {

    }

    public override void Release()
    {
        IsRuning = false;
    }

    public override void Update()
    {
        if (!IsRuning)
            return;
    }

    public void SetLevelCamera()
    {
        mainCamera = Camera.main;
    }

    protected override void Initialize()
    {
        IsRuning = true;
    }
}
