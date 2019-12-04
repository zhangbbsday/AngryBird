﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBird : Bird
{
    public BlueBirdClone birdContainer;
    private readonly int divideNumber = 2;
    private readonly float offsetY = 2.0f;
    
    public override void Skill()
    {
        if (!canUseSkill)
            return;
        
        for (int i = 0, j = -1; i < divideNumber; i++, j += 2)
        {
            BlueBirdClone createBird = Instantiate(birdContainer, RigidbodySelf.position, Quaternion.identity,
                transform.parent).GetComponent<BlueBirdClone>();

            createBird.SetClone(this, j * offsetY, DamageCoefficient, exitTime);
        }

        base.Skill();
    }

    protected override void Initialize()
    {
        DamageCoefficient = new float[3] { 2.0f, 0.8f, 0.4f };
        base.Initialize();
    }
}
