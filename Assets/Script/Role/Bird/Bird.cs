using System.Collections;
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

    public enum AttackObstacleType
    {
        Glass = 0,
        Wood = 1,
        Rock = 2
    }

    public float Damage { get; set; }
    public BehaviorState State { get; protected set; }
    public TrailRenderer TrailRenderer { get; protected set; }

    public float damage;
    
    public Rigidbody2D RigidbodySelf { get; protected set; }
    protected HurtState hurtState;
    protected AudioSource audioSource;
    protected Animator animator;
    protected float[] DamageCoefficient { get; set; }
    protected readonly float exitTime = 5.0f;

    private IEnumerator yell;
    private IEnumerator wink;
    private IEnumerator pettyAction;
    private readonly float singTimeMax = 12.0f;
    private readonly float winkTimeMax = 8.0f;
    
    private readonly float pettyActionTimeMax = 6.0f;
    private readonly float pettyActionSpeed = 2.0f;
    private readonly float jumpTime = 0.5f;
    private readonly float jumpPrepareTime = 1.0f;
    
    protected bool canUseSkill;

    private void Start()
    {
        Initialize();
    }

    public void JumpToSling()
    {
        StopCoroutine(yell);
        StopCoroutine(pettyAction);
        StartCoroutine(Jump());
    }

    public void Launch(Vector2 velocity)
    {
        StopCoroutine(wink);
        GameManager.Instance.AudioSystemControl.Play(audioSource, tag + "Launch");
        GameManager.Instance.SlingSystemControl.IsLoadBird = false;
        GameManager.Instance.CameraSystemControl.IsFollow = true;

        animator.SetTrigger("Fly");
        RigidbodySelf.velocity = velocity;
        RigidbodySelf.isKinematic = false;
        State = BehaviorState.Fly;

        TrailRenderer.emitting = true;
        gameObject.layer = 9;
    }

    public virtual void Skill()
    {
        if (!canUseSkill)
            return;

        GameManager.Instance.AudioSystemControl.Play(audioSource, tag + "Skill");
        canUseSkill = false;
    }

    protected virtual void Initialize()
    {
        RigidbodySelf = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        TrailRenderer = transform.parent.GetChild(1).GetComponent<TrailRenderer>();

        Damage = damage;
        State = BehaviorState.AtGround;
        hurtState = HurtState.Normal;
        canUseSkill = true;

        yell = Yell();
        wink = Wink();
        pettyAction = PettyAction();
        TrailRenderer.emitting = false;
        TrailRenderer.transform.position = RigidbodySelf.position;

        StartCoroutine(yell);
        StartCoroutine(wink);
        StartCoroutine(pettyAction);
    }

    private IEnumerator Yell()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(singTimeMax / 2, singTimeMax));
            animator.SetTrigger("Yell");
            GameManager.Instance.AudioSystemControl.Play(audioSource, tag + "Yell");
        }
    }

    private IEnumerator Wink()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(winkTimeMax / 2, winkTimeMax));
            animator.SetTrigger("Wink");
        }
    }

    private IEnumerator Jump()
    {
        yield return new WaitForSeconds(jumpPrepareTime);
        State = BehaviorState.JumpToSling;
        RigidbodySelf.isKinematic = true;

        RigidbodySelf.velocity = new Vector2((GameManager.Instance.SlingSystemControl.Origin.x - RigidbodySelf.position.x) / jumpTime,
            (GameManager.Instance.SlingSystemControl.Origin.y - RigidbodySelf.position.y) / jumpTime);
        RigidbodySelf.angularVelocity = -360.0f / jumpTime;

        yield return new WaitForSeconds(jumpTime);
        RigidbodySelf.velocity = Vector2.zero;
        RigidbodySelf.rotation = 0;
        RigidbodySelf.angularVelocity = 0;
        RigidbodySelf.position = GameManager.Instance.SlingSystemControl.Origin;
        State = BehaviorState.WaitForLaunch;

        yield return new WaitForSeconds(0.01f);
        GameManager.Instance.SlingSystemControl.IsLoadBird = true;
    }

    private IEnumerator PettyAction()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(pettyActionTimeMax / 2, pettyActionTimeMax));
            RigidbodySelf.velocity = Vector2.up * pettyActionSpeed;
        }
    }

    private IEnumerator DeadBefore()
    {
        yield return new WaitForSeconds(exitTime);
        animator.SetTrigger("Dead");
        GameManager.Instance.AudioSystemControl.Play(audioSource, "BirdDestory");
        //粒子效果
    }

    protected void DestroyThis()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.layer != 9)
            return;
        if (collision.collider.GetComponent<Bird>() || collision.collider.GetComponent<BlueBirdClone>())
            return;

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
            TrailRenderer.emitting = false;

            animator.SetTrigger("Damaged");
            GameManager.Instance.AudioSystemControl.Play(audioSource, tag + "Hurt");
            GameManager.Instance.BirdControlSystemControl.ClearTrail();
            GameManager.Instance.CameraSystemControl.StopFollow();

            StartCoroutine(DeadBefore());
        }

        State = BehaviorState.FinalRoll;   
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Edge"))
        {
            Destroy(gameObject, exitTime);
            GameManager.Instance.BirdControlSystemControl.ClearTrail();
            GameManager.Instance.CameraSystemControl.StopFollow();
            canUseSkill = false;
        }
    }
}
