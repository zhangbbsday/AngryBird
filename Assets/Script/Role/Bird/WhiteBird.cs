using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBird : Bird
{
    public GameObject egg;

    private float explosionRadius = 3f;
    private float explosionForce = 500f;
    private float eggForce = 5.0f;

    public override void Skill()
    {
        if (!canUseSkill)
            return;

        GameObject.Instantiate(egg, RigidbodySelf.position, Quaternion.identity, transform.parent).GetComponent<Egg>()
            .Initialize(explosionRadius, explosionForce, Damage * 2f);
        animator.SetTrigger("Skill");

        RigidbodySelf.gravityScale = 0;
        RigidbodySelf.velocity += Vector2.up * eggForce;
        base.Skill();
    }

    protected override void Initialize()
    {
        DamageCoefficient = new float[3] { 1.2f, 1.0f, 0.5f };
        base.Initialize();
        scoreColor = Color.white;
    }
}
