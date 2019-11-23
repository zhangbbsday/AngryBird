using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : BaseSystem
{
    public bool IsZoom { get; private set; }

    private Renderer leftEdge;
    private Renderer rightEdge;

    private Camera mainCamera;
    private const float MaxMoveSpeed = 8.0f;
    private const float MaxZoomSpeed = 30.0f;
    private const float MinZoomSize = 5.0f;
    private float moveSpeed;
    private float zoomSpeed;
    private float zoomTime;                     //镜头聚焦计时器
    private float zoomTimeMax = 0.8f;           //镜头聚焦时长

    public CameraSystem()
    {
    }

    public override void Release()
    {
        IsRuning = false;
        IsZoom = false;
    }

    public override void Update()
    {
        if (!IsRuning)
            return;

        Move();
        Zoom();
    }

    public void SetLevelCamera(Transform edges)
    {
        mainCamera = Camera.main;
        leftEdge = edges.Find("Left").GetComponent<Renderer>();
        rightEdge = edges.Find("Right").GetComponent<Renderer>();
    }

    public void MoveCamera(float mouseAxis)
    {
        moveSpeed = -mouseAxis * MaxMoveSpeed;
    }

    public void ZoomCamera(float mouseScrollWheelAxis)
    {
        zoomSpeed = -mouseScrollWheelAxis * MaxZoomSpeed;
        zoomTime = Time.time;
        IsZoom = true;
    }

    private void Move()
    {
        if ((leftEdge.isVisible && moveSpeed < 0) || (rightEdge.isVisible && moveSpeed > 0))
            return;

        mainCamera.transform.position += moveSpeed * Time.deltaTime * Vector3.right;
    }

    private void Zoom()
    {
        if (((leftEdge.isVisible || rightEdge.isVisible) && zoomSpeed > 0) || (mainCamera.orthographicSize <= MinZoomSize && zoomSpeed < 0))
        {
            IsZoom = false;
            zoomSpeed = 0;
        }

        if (!IsZoom)
            return;

        mainCamera.orthographicSize += zoomSpeed * Time.deltaTime;
        if (Time.time - zoomTime > zoomTimeMax)
            IsZoom = false;
    }

    protected override void Initialize()
    {
        IsRuning = true;
        IsZoom = false;
        moveSpeed = 0;
        zoomSpeed = 0;
    }
}
