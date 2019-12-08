using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackBird : Bird
{
    private float explosionRadius = 5f;
    private float explosionForce = 500f;

    public override void Skill()
    {
        if (!canUseSkill)
            return;

        Explosion();
        canUseSkill = false;
    }

    private void Explosion()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(RigidbodySelf.position, explosionRadius);
        foreach (Collider2D obj in colliders)
        {
            Rigidbody2D rigidbody = obj.GetComponent<Rigidbody2D>();
            IPassiveDamageObject passiveDamageObject = obj.GetComponent<IPassiveDamageObject>();

            if (!rigidbody || passiveDamageObject == null)
                continue;
            AddForce(rigidbody);
            passiveDamageObject.Hp -= Damage;
        }

        RigidbodySelf.velocity = Vector2.zero;
        RigidbodySelf.isKinematic = true;

        GameManager.Instance.AudioSystemControl.Play(audioSource, tag + "Skill");
        GameManager.Instance.CameraSystemControl.StopFollow();
        animator.SetTrigger("Dead");
    }

    private void AddForce(Rigidbody2D rigidbody)
    {
        Vector2 direction = rigidbody.position - RigidbodySelf.position;
        float distance = direction.magnitude / explosionRadius;
        rigidbody.AddForce(Mathf.Lerp(0, explosionForce, (1 - distance)) * direction);
    }

    private void EnablePhysics()
    {
        gameObject.layer = 12;
    }

    protected override void Initialize()
    {
        base.Initialize();
        DamageCoefficient = new float[] { 1.0f, 1.0f, 1.0f };
        scoreColor = Color.black;
    }
}
