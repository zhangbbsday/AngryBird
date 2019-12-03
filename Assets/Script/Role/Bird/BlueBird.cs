using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBird : Bird
{
    private GameObject birdContainer;
    private readonly int divideNumber = 2;
    private bool isCreated = false;
    
    public override void Skill()
    {
        if (!canUseSkill)
            return;
        
        for (int i = 0, j = -1; i < divideNumber; i++, j += 2)
        {
            BlueBird createBird = Instantiate(birdContainer, RigidbodySelf.position, Quaternion.identity, 
                TrailRenderer.transform).transform.GetChild(0).GetComponent<BlueBird>();
        }

        base.Skill();
    }

    protected override void Initialize()
    {
        if (isCreated)
            return;

        base.Initialize();
        DamageCoefficient = new float[3] { 2.0f, 0.8f, 0.4f };
        birdContainer = transform.parent.gameObject;
    }

    //public void CreateSet()
    //{
    //    Initialize();

    //    TrailRenderer.emitting = true;
    //    RigidbodySelf.isKinematic = false;
    //    State = BehaviorState.Fly;
    //    canUseSkill = false;
    //    isCreated = true;
    //    gameObject.layer = 9;

    //    StopAllCoroutines();
    //}
}
