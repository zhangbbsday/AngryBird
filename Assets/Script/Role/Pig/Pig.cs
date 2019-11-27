using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour, IDamageObject
{
    enum HurtState
    {
        Normal,
        Little,
        Bad,
        Dead
    }

    enum PigAudio
    {
        PigDamage1,
        PigDamage2,
        PigDamage3,
        PigSing1,
        PigSing2,
        PigSmile,
        PigDestroy
    }

    public float maxHp;
    public float Hp { get; set; }
    public Rigidbody2D RigidbodySelf { get; private set; }
    public float Damage { get; set; }

    private float criticalSpeed = 2;  //临界速度，大于此值会受伤
    private float winkTimeMax = 3;
    private float singTimeMax = 6;
    private Animator animator;
    private AudioSource audioSource;
    private IEnumerator winkMethod;
    private IEnumerator singMethod;
    private HurtState hurtState;

    void Start()
    {
        Initialize();

        StartCoroutine(winkMethod);
        StartCoroutine(singMethod);
    }

    protected virtual void Initialize()
    {
        RigidbodySelf = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        winkMethod = Wink();
        singMethod = Sing();

        Hp = maxHp;
        Damage = 1.0f;
        hurtState = HurtState.Normal;
    }

    private void ChangeHurtState()
    {
        float percent = Hp / maxHp;

        if (Mathf.Approximately(percent, 1.0f))
            return;

        if (percent > 0.8)
            hurtState = HurtState.Normal;
        else if (percent > 0.5)
            hurtState = HurtState.Little;
        else if (percent > 0)
            hurtState = HurtState.Bad;
        else
            hurtState = HurtState.Dead;

    }

    private void Smile()
    {
        animator.SetTrigger("Smile");
        GameManager.Instance.AudioSystemControl.Play(audioSource, PigAudio.PigSmile.ToString());
    }

    private void DestroyPrepare()
    {
        StopAllCoroutines();
        animator.SetTrigger("Destroy");
        GameManager.Instance.AudioSystemControl.Play(audioSource, PigAudio.PigDestroy.ToString());
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }

    private void Hurt(float changeNum)
    {
        Hp -= changeNum;
        ChangeHurtState();
        switch (hurtState)
        {
            case HurtState.Normal:
                GameManager.Instance.AudioSystemControl.Play(audioSource, PigAudio.PigDamage1.ToString());
                animator.SetFloat("Damage", 0);
                break;
            case HurtState.Little:
                GameManager.Instance.AudioSystemControl.Play(audioSource, PigAudio.PigDamage2.ToString());
                animator.SetFloat("Damage", 1);
                break;
            case HurtState.Bad:
                GameManager.Instance.AudioSystemControl.Play(audioSource, PigAudio.PigDamage3.ToString());
                animator.SetFloat("Damage", 2);
                break;
            case HurtState.Dead:
                GameManager.Instance.AudioSystemControl.Play(audioSource, PigAudio.PigDestroy.ToString());
                DestroyPrepare();
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hurtState == HurtState.Dead)
            return;

        IDamageObject damageObject = collision.collider.GetComponent<IDamageObject>();
        float damageAdd = 1.0f;

        if (collision.relativeVelocity.magnitude > criticalSpeed)
        {     
            if (damageObject != null)
                damageAdd = damageObject.Damage;
            Hurt((collision.relativeVelocity.magnitude - criticalSpeed) * damageAdd);
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

    private IEnumerator Sing()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0, singTimeMax));
            GameManager.Instance.AudioSystemControl.Play(audioSource, PigAudio.PigSing1.ToString());
            yield return new WaitForSeconds(Random.Range(0, singTimeMax));
            GameManager.Instance.AudioSystemControl.Play(audioSource, PigAudio.PigSing2.ToString());
        }
    }
}
