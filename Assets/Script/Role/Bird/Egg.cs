using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    private Rigidbody2D rigidbodySelf;
    private AudioSource audioSource;
    private Animator animator;
    private float explosionRadius;
    private float explosionForce;
    private float eggDamage;

    public void Initialize(float radius, float force, float damage)
    {
        rigidbodySelf = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        explosionRadius = radius;
        explosionForce = force;
        eggDamage = damage;
    }

    private void Explosion()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(rigidbodySelf.position, explosionRadius);
        foreach (Collider2D obj in colliders)
        {
            Rigidbody2D rigidbody = obj.GetComponent<Rigidbody2D>();
            IPassiveDamageObject passiveDamageObject = obj.GetComponent<IPassiveDamageObject>();

            if (!rigidbody || passiveDamageObject == null)
                continue;
            AddForce(rigidbody);

            passiveDamageObject.Hp -= eggDamage;
        }

        rigidbodySelf.velocity = Vector2.zero;
        rigidbodySelf.isKinematic = true;

        GameManager.Instance.AudioSystemControl.Play(audioSource, "EggBoom");
        animator.SetTrigger("Exp");
    }

    private void DestoryThis()
    {
        Destroy(gameObject);
    }

    private void AddForce(Rigidbody2D rigidbody)
    {
        Vector2 direction = rigidbody.position - rigidbodySelf.position;
        float distance = direction.magnitude / explosionRadius;
        rigidbody.AddForce(Mathf.Lerp(0, explosionForce, (1 - distance)) * direction);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Bird>() || collision.collider.GetComponent<BlueBirdClone>())
            return;

        Explosion();
    }
}
