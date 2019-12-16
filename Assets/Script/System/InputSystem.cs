using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : BaseSystem
{
    private bool isMovingCamera;

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
        if (!IsRuning || GameManager.Instance.IsPasue)
            return;
        if (GameManager.Instance.JudgeSystemControl.IsJudged)
        {
            ReleaseMouse();
            return;
        }

        if (Input.GetMouseButtonDown(0))
            UseSkill();
        else if (Input.GetMouseButton(0))
        {
            SetLinePosition();
            MoveCamera();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ReleaseMouse();
        }

        ZoomCamera();
    }

    protected override void Initialize()
    {
        IsRuning = false;
        isMovingCamera = false;
    }

    private void SetLinePosition()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (isMovingCamera)
            return;
        if (!GameManager.Instance.SlingSystemControl.IsLoadBird)
            return;
        if (!GameManager.Instance.SlingSystemControl.IsDrag && Vector2.Distance(GameManager.Instance.SlingSystemControl.Origin, mousePosition) > GameManager.Instance.SlingSystemControl.MaxLength)
            return;

        GameManager.Instance.CameraSystemControl.MoveCamera(0);
        GameManager.Instance.SlingSystemControl.SetLinePosition(mousePosition);
    }

    private void MoveCamera()
    {
        if (GameManager.Instance.SlingSystemControl.IsDrag)
            return;

        GameManager.Instance.CameraSystemControl.MoveCamera(Input.GetAxis("Mouse X"));
        isMovingCamera = true;
    }

    private void ZoomCamera()
    {
        if (Input.GetAxis("Mouse ScrollWheel") == 0)
            return;

        GameManager.Instance.CameraSystemControl.ZoomCamera(Input.GetAxis("Mouse ScrollWheel"));
    }

    private void ReleaseMouse()
    {
        isMovingCamera = false;
        if (GameManager.Instance.SlingSystemControl.IsDrag && GameManager.Instance.SlingSystemControl.IsLoadBird)
            GameManager.Instance.SlingSystemControl.Launch();
    }

    private void UseSkill()
    {
        GameManager.Instance.BirdControlSystemControl.UseSkill();
    }
}
