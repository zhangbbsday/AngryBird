using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBird : Bird
{
    private float addSpeed = 2.0f;
    private float addDamage = 1.5f;

    public override void Skill()
    {
        if (!canUseSkill)
            return;

        animator.SetTrigger("Skill");
        RigidbodySelf.velocity *= addSpeed;
        Damage *= addDamage;
        base.Skill();
    }

    protected override void Initialize()
    {
        base.Initialize();
        DamageCoefficient = new float[3] { 1.2f, 1.5f, 0.6f };
        scoreColor = Color.yellow;
    }
}
