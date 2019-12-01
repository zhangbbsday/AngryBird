using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingSystem : BaseSystem
{
    public GameObject Sling { get; set; }
    public Vector2 Origin { get; set; }
    public Vector2 HoldPosition { get => hold.position; }
    public bool IsDrag { get; set; }
    public bool IsLoadBird { get; set; }
    public float MaxLength { get; } = 2.0f;  //弹弓拉伸最大距离
    public float MinLength { get; } = 1.0f;   //弹弓拉伸最小距离

    private LineRenderer slingLeftLine;
    private LineRenderer slingRightLine;
    private Transform hold;
    
    public SlingSystem()
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
    }

    public void GetSling(GameObject sling)
    {
        Sling = sling;
        slingLeftLine = Sling.transform.Find("Left").GetComponent<LineRenderer>();
        slingRightLine = Sling.transform.Find("Right").GetComponent<LineRenderer>();
        hold = Sling.transform.Find("Hold").transform;
        Origin = hold.position;
    }

    public void SetLinePosition(Vector2 mousePosition)
    {
        IsDrag = true;
        if (Vector2.Distance(Origin, mousePosition) <= MaxLength)
            hold.position = mousePosition;
        else
            hold.position = (mousePosition - Origin).normalized * ((Vector2)hold.position - Origin).magnitude + Origin;

        hold.eulerAngles = Vector3.forward * Mathf.Rad2Deg * Mathf.Atan2(mousePosition.y - Origin.y, mousePosition.x - Origin.x);
        slingLeftLine.SetPosition(1, hold.localPosition - slingLeftLine.transform.localPosition);
        slingRightLine.SetPosition(1, hold.localPosition - slingRightLine.transform.localPosition);

        GameManager.Instance.BirdControlSystemControl.SetPosition();
    }

    public void Launch()
    {
        IsDrag = false;

        GameManager.Instance.BirdControlSystemControl.Launch(-(HoldPosition - Origin) * 10);

        hold.position = Origin;
        hold.eulerAngles = Vector3.zero;
        slingLeftLine.SetPosition(1, hold.localPosition - slingLeftLine.transform.localPosition);
        slingRightLine.SetPosition(1, hold.localPosition - slingRightLine.transform.localPosition);
    }

    protected override void Initialize()
    {
        IsRuning = true;
        IsDrag = false;
    }
}
