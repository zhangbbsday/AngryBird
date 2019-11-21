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
        if (!IsRuning)
            return;

        if (Input.GetMouseButtonDown(0))
            UseSkill();
        else if (Input.GetMouseButton(0))
        {
            SetLinePosition();
            MoveCamera();
        }
        else if (Input.GetMouseButtonUp(0))
            Launch();
    }

    protected override void Initialize()
    {
        IsRuning = false;
    }

    private void SetLinePosition()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if (!GameManager.Instance.SlingSystemControl.IsDrag && Vector2.Distance(GameManager.Instance.SlingSystemControl.Origin, mousePosition) > GameManager.Instance.SlingSystemControl.MaxLength)
            return;

        GameManager.Instance.SlingSystemControl.SetLinePosition(mousePosition);
    }

    private void MoveCamera()
    {
        if (GameManager.Instance.SlingSystemControl.IsDrag)
            return;
    }

    private void Launch()
    {
        GameManager.Instance.SlingSystemControl.Launch();
    }

    private void UseSkill()
    {
        Debug.Log("UseSkill!");
    }
}
