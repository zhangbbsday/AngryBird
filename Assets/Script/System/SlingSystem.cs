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
    public float MaxLength { get; } = 1.5f;  //弹弓拉伸最大距离
    public float MinLength { get; } = 1.0f;   //弹弓拉伸最小距离
    public float SlingCoefficient { get; } = 120;

    private float[] slingAngleLimit = { -60, -120 };
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

        IsLoadBird = false;
        IsDrag = false;
    }

    public void SetLinePosition(Vector2 mousePosition)
    {
        IsDrag = true;
        float length = Mathf.Min(Vector2.Distance(Origin, mousePosition), MaxLength);
        float angle = Mathf.Rad2Deg * Mathf.Atan2(hold.position.y - Origin.y, hold.position.x - Origin.x);
        float mid = (slingAngleLimit[0] + slingAngleLimit[1]) / 2;

        if (angle > slingAngleLimit[1] && angle < slingAngleLimit[0])
            length = Mathf.Lerp(0, length, Mathf.Abs((mid - angle) / (slingAngleLimit[0] - mid)));
      
        hold.position = (mousePosition - Origin).normalized * length + Origin;
        hold.eulerAngles = Vector3.forward * Mathf.Rad2Deg * Mathf.Atan2(mousePosition.y - Origin.y, mousePosition.x - Origin.x);
        slingLeftLine.SetPosition(1, hold.localPosition - slingLeftLine.transform.localPosition);
        slingRightLine.SetPosition(1, hold.localPosition - slingRightLine.transform.localPosition);
        GameManager.Instance.BirdControlSystemControl.SetPosition();
    }

    public void Launch()
    {
        Vector2 length = Origin - HoldPosition;
        IsDrag = false;

        hold.position = Origin;
        hold.eulerAngles = Vector3.zero;
        slingLeftLine.SetPosition(1, hold.localPosition - slingLeftLine.transform.localPosition);
        slingRightLine.SetPosition(1, hold.localPosition - slingRightLine.transform.localPosition);

        if (length.magnitude > MinLength && !GameManager.Instance.JudgeSystemControl.IsJudged)
            GameManager.Instance.BirdControlSystemControl.Launch(length);
        else
            GameManager.Instance.BirdControlSystemControl.SetPosition();
    }

    protected override void Initialize()
    {
        IsRuning = false;
        IsDrag = false;
        IsLoadBird = false;
    }
}
