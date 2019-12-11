using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : BaseSystem
{
    public bool IsZoom { get; private set; }
    public bool IsFollow { get; set; }

    private Renderer leftEdge;
    private Renderer rightEdge;

    private Camera mainCamera;
    private Vector3 startPosition = new Vector3(-8.4f, 0, -10);
    private float startSize = 5.2f;
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

        if (!IsFollow)
        {
            Move();
            Zoom();
        }
        else
            Follow();
    }

    public void SetLevelCamera(Transform edges)
    {
        mainCamera = Camera.main;
        leftEdge = edges.Find("Left").GetComponent<Renderer>();
        rightEdge = edges.Find("Right").GetComponent<Renderer>();

        SetStartCamera();
    }

    public void SetStartCamera()
    {
        mainCamera.transform.position = startPosition;
        mainCamera.orthographicSize = startSize;

        moveSpeed = 0;
        IsFollow = false;
        IsZoom = false;
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

    public void StopFollow()
    {
        IsFollow = false;
        MoveCamera(1);
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

    private void Follow()
    {
        Bird bird = GameManager.Instance.BirdControlSystemControl.FlyBird;
        Vector2 offset;
        if (!bird)
            return;

        offset = bird.transform.position - mainCamera.transform.position;
        offset *= Vector2.right;

        if ((leftEdge.isVisible && offset.x < 0) || (rightEdge.isVisible && offset.x > 0))
            return;

        //mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, mainCamera.transform.position + (Vector3)offset, Time.deltaTime * followingSpeed);
        mainCamera.transform.position += (Vector3)offset;
    }

    protected override void Initialize()
    {
        IsRuning = true;
        IsZoom = false;
        IsFollow = false;

        moveSpeed = 0;
        zoomSpeed = 0;
    }
}
