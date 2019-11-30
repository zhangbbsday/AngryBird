﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public enum BehaviorState
    {
        AtGround,
        JumpToSling,
        WaitForLaunch,
        Fly,
        FinalRoll
    }

    protected enum HurtState
    {
        Normal,
        Little,
        Dead
    }

    protected enum AttackObstacleType
    {
        Glass = 0,
        Wood = 1,
        Rock = 2
    }

    public float Damage { get; set; }
    public BehaviorState State { get; set; }

    public float damage;
    
    protected Rigidbody2D rigidbodySelf;
    protected HurtState hurtState;
    protected AudioSource audioSource;
    protected Animator animator;
    protected float[] DamageCoefficient { get; set; }

    private IEnumerator yell;
    private IEnumerator wink;
    private IEnumerator pettyAction;
    private readonly float singTimeMax = 6.0f;
    private readonly float winkTimeMax = 6.0f;
    
    private readonly float pettyActionTimeMax = 6.0f;
    private readonly float pettyActionSpeed = 2.0f;
    private readonly float jumpTime = 0.5f;
    private readonly float jumpPrepareTime = 2.0f;
    private readonly float exitTime = 5.0f;

    private void Start()
    {
        Initialize();

        StartCoroutine(yell);
        StartCoroutine(wink);
        StartCoroutine(pettyAction);

        JumpToSling();
    }

    public void JumpToSling()
    {
        StopCoroutine(yell);
        StopCoroutine(pettyAction);
        StartCoroutine(Jump());
        
    }

    public void Launch()
    {
        StopCoroutine(wink);
        GameManager.Instance.AudioSystemControl.Play(audioSource, tag + "Launch");
    }

    public virtual void Skill()
    {
        //发动特技
    }

    protected virtual void Initialize()
    {
        rigidbodySelf = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        Damage = damage;
        State = BehaviorState.AtGround;
        hurtState = HurtState.Normal;

        yell = Yell();
        wink = Wink();
        pettyAction = PettyAction();
    }

    private IEnumerator Yell()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0, singTimeMax));
            animator.SetTrigger("Yell");
            GameManager.Instance.AudioSystemControl.Play(audioSource, tag + "Yell");
        }
    }

    private IEnumerator Wink()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0, winkTimeMax));
            animator.SetTrigger("Wink");
        }
    }

    private IEnumerator Jump()
    {
        yield return new WaitForSeconds(jumpPrepareTime);

        State = BehaviorState.JumpToSling;
        rigidbodySelf.isKinematic = true;

        rigidbodySelf.velocity = new Vector2((GameManager.Instance.SlingSystemControl.Origin.x - rigidbodySelf.position.x) / jumpTime,
            (GameManager.Instance.SlingSystemControl.Origin.y - rigidbodySelf.position.y) / jumpTime);
        rigidbodySelf.angularVelocity = -360.0f / jumpTime;
        yield return new WaitForSeconds(jumpTime);

        rigidbodySelf.velocity = Vector2.zero;
        rigidbodySelf.angularVelocity = 0;
        rigidbodySelf.position = GameManager.Instance.SlingSystemControl.Origin;
        State = BehaviorState.WaitForLaunch;
    }

    private IEnumerator PettyAction()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(pettyActionTimeMax / 2, pettyActionTimeMax));
            rigidbodySelf.velocity = Vector2.up * pettyActionSpeed;
        }
    }

    private IEnumerator DeadBefore()
    {
        yield return new WaitForSeconds(exitTime);
        animator.SetTrigger("Dead");
        //粒子效果
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IPassiveDamageObject passiveDamageObject = collision.collider.GetComponent<IPassiveDamageObject>();
        if (passiveDamageObject != null)
        {
            if (passiveDamageObject is Obstacle obstacle)
                passiveDamageObject.ChangeHp(Damage * DamageCoefficient[(int)System.Enum.Parse(typeof(AttackObstacleType), obstacle.tag)]);
            else
                passiveDamageObject.ChangeHp(Damage);
        }

        if (State == BehaviorState.Fly)
        {
            animator.SetTrigger("Damaged");
            GameManager.Instance.AudioSystemControl.Play(audioSource, tag + "Hurt");
            StartCoroutine(DeadBefore());
        }
        State = BehaviorState.FinalRoll;
    }
}
