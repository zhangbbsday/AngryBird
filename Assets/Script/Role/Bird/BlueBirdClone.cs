using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBirdClone : MonoBehaviour
{
    private Bird.BehaviorState state;
    private Rigidbody2D rigidbodySelf;
    private Animator animator;
    private AudioSource audioSource;
    private TrailRenderer trailRenderer;

    private float damage;
    private float[] damageCoefficient;
    private float exitTime;

    public void SetClone(Bird blueBird, float offset, float[] coefficient, float exit)
    {
        rigidbodySelf = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        trailRenderer = GetComponent<TrailRenderer>();

        damage = blueBird.Damage;
        damageCoefficient = coefficient;
        exitTime = exit;
        state = Bird.BehaviorState.Fly;

        rigidbodySelf.position = blueBird.RigidbodySelf.position + (offset * Vector2.up).normalized * 0.1f;
        rigidbodySelf.velocity = blueBird.RigidbodySelf.velocity + offset * Vector2.up;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Bird>() || collision.collider.GetComponent<BlueBirdClone>())
            return;

        IPassiveDamageObject passiveDamageObject = collision.collider.GetComponent<IPassiveDamageObject>();
        if (passiveDamageObject != null)
        {
            if (passiveDamageObject is Obstacle obstacle)
                passiveDamageObject.ChangeHp(damage * damageCoefficient[(int)System.Enum.Parse(typeof(Bird.AttackObstacleType), obstacle.tag)]);
            else
                passiveDamageObject.ChangeHp(damage);
        }

        if (state == Bird.BehaviorState.Fly)
        {
            trailRenderer.emitting = false;

            animator.SetTrigger("Damaged");
            GameManager.Instance.AudioSystemControl.Play(audioSource, tag + "Hurt");

            StartCoroutine(DeadBefore());
        }

        state = Bird.BehaviorState.FinalRoll;
    }

    private void DestroyThis()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    private IEnumerator DeadBefore()
    {
        yield return new WaitForSeconds(exitTime);
        animator.SetTrigger("Dead");
        //粒子效果
    }
}
