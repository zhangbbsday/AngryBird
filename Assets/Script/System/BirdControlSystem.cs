using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdControlSystem : BaseSystem
{
    private List<Bird> birdsList;
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
            birdsList.Add(birds.GetChild(i).GetComponent<Bird>());
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
        if (selectBird.State != Bird.BehaviorState.WaitForLaunch || !GameManager.Instance.SlingSystemControl.IsDrag)
            return;

        selectBird.RigidbodySelf.position = GameManager.Instance.SlingSystemControl.HoldPosition;
    }

    public void Launch(Vector2 velocity)
    {
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
}
