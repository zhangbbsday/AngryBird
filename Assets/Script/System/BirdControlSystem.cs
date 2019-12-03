using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdControlSystem : BaseSystem
{
    public Bird FlyBird { get => previousBird; }

    private List<Bird> birdsList;
    private List<TrailRenderer> trailRenderers;
    private Bird previousBird;
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
        }

        selectBird = birdsList[birdIndex];
        JumpToSling();
    }

    private void ClearBird()
    {
        birdsList.Clear();
        trailRenderers.Clear();
        previousBird = null;
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

        SetTrail();
    }

    protected override void Initialize()
    {
        IsRuning = false;
        birdIndex = 0;
        birdsList = new List<Bird>();
        trailRenderers = new List<TrailRenderer>();
        previousBird = null;
        selectBird = null;
    }

    private void JumpToSling()
    {
        if (selectBird.State != Bird.BehaviorState.AtGround)
            return;

        selectBird.JumpToSling();
    }

    private void SetTrail()
    {
        if (previousBird == null)
            return;

        previousBird.TrailRenderer.transform.position = previousBird.RigidbodySelf.position;
    }

    public void SetPosition()
    {
        if (selectBird.State != Bird.BehaviorState.WaitForLaunch)
            return;

        selectBird.RigidbodySelf.position = GameManager.Instance.SlingSystemControl.HoldPosition;
        selectBird.TrailRenderer.transform.position = selectBird.RigidbodySelf.position;
    }

    public void Launch(Vector2 length)
    {
        Vector2 velocity = Mathf.Sqrt(GameManager.Instance.SlingSystemControl.SlingCoefficient / selectBird.RigidbodySelf.mass) * length;

        if (selectBird.State == Bird.BehaviorState.WaitForLaunch)
        {
            birdIndex++;
            selectBird.Launch(velocity);
        }

        previousBird = selectBird;
        trailRenderers.Add(previousBird.TrailRenderer);
        if (birdIndex < birdsList.Count)
        {
            selectBird = birdsList[birdIndex];
            selectBird.JumpToSling();
        }
    }

    public void UseSkill()
    {
        if (previousBird && previousBird.State == Bird.BehaviorState.Fly)
            previousBird.Skill();
    }

    public void ClearTrail()
    {
        if (trailRenderers.Count < 2)
            return;

        GameObject.Destroy(trailRenderers[0].gameObject);
        trailRenderers.RemoveAt(0);
    }
}
