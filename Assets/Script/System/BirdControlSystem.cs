using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdControlSystem : BaseSystem
{
    private List<Bird> birdsList;
    private List<TrailRenderer> trailRenderers;
    private Bird selectBird;
    private int birdIndex;

    public BirdControlSystem()
    {
        Initialize();
    }

    public void GetBird(Transform birds)
    {
        ClearBird();

        if (!birds)
            throw new System.ArgumentException("Birds没有设定");

        for (int i = 0; i < birds.childCount; i++)
        {
            birdsList.Add(birds.GetChild(i).GetChild(0).GetComponent<Bird>());
            trailRenderers.Add(birds.GetChild(i).GetChild(1).GetComponent<TrailRenderer>());
        }

        selectBird = birdsList[birdIndex];
        JumpToSling();
    }

    private void ClearBird()
    {
        birdsList.Clear();
        selectBird = null;
        birdIndex = 0;
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

    protected override void Initialize()
    {
        IsRuning = false;
        birdIndex = 0;
        birdsList = new List<Bird>();
        trailRenderers = new List<TrailRenderer>();
        selectBird = null;
    }

    private void JumpToSling()
    {
        if (selectBird.State != Bird.BehaviorState.AtGround)
            return;

        selectBird.JumpToSling();
    }

    public void SetPosition()
    {
        if (selectBird.State != Bird.BehaviorState.WaitForLaunch)
            return;

        selectBird.RigidbodySelf.position = GameManager.Instance.SlingSystemControl.HoldPosition;
    }

    public void Launch(Vector2 length)
    {
        Vector2 velocity = Mathf.Sqrt(GameManager.Instance.SlingSystemControl.SlingCoefficient / selectBird.RigidbodySelf.mass) * length;

        if (selectBird.State == Bird.BehaviorState.WaitForLaunch)
        {
            birdIndex++;
            selectBird.Launch(velocity);
        }

        if (birdIndex < birdsList.Count)
        {
            selectBird = birdsList[birdIndex];
            selectBird.JumpToSling();
        }
    }

    public void ClearTrail()
    {
        if (birdIndex - 1 == 0)
            return;

        GameManager.Destroy(trailRenderers[0].gameObject);
        trailRenderers.RemoveAt(0);
    }
}
