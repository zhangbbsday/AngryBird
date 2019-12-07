using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pig : MonoBehaviour, IPassiveDamageObject
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
    public float Damage { get; set; }

    private readonly float criticalSpeed = 2.0f;  //临界速度，大于此值会受伤
    private readonly float winkTimeMax = 3.0f;
    private readonly float singTimeMax = 6.0f;
    private Text text;
    private Transform canvas;
    private Animator animator;
    private AudioSource audioSource;
    private IEnumerator winkMethod;
    private IEnumerator singMethod;
    private HurtState hurtState;
    private readonly int maxScore = 5000;

    void Start()
    {
        Initialize();

        StartCoroutine(winkMethod);
        StartCoroutine(singMethod);
    }

    private void Initialize()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        text = GameObjectContainer.Instacne.FindGameObjectComponent<Text>("DamageScore");
        canvas = GameObjectContainer.Instacne.FindGameObjectComponent<Transform>("Canvas");

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

    private void SetHurtEffect()
    {
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

    public void ChangeHp(float changeNum, bool isBirdChange = false)
    {
        Hp -= changeNum;
        ChangeHurtState();
        SetHurtEffect();

        if (hurtState == HurtState.Dead)
            ShowScore(maxScore);
    }

    public void ShowScore(int score)
    {
        Text t = GameObject.Instantiate(text, transform.position + Vector3.up, Quaternion.identity, canvas);
        t.text = score.ToString();
        t.color = Color.green;
        Destroy(t.gameObject, 0.5f);

        
        GameManager.Instance.ScoreSystemControl.GetScore(score);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hurtState == HurtState.Dead || collision.collider.GetComponent<Bird>() != null)
            return;

        IPassiveDamageObject damageObject = collision.collider.GetComponent<IPassiveDamageObject>();
        float damageAdd = 1.0f;

        if (collision.relativeVelocity.magnitude > criticalSpeed)
        {
            if (damageObject != null)
                damageAdd = damageObject.Damage;
            ChangeHp((collision.relativeVelocity.magnitude - criticalSpeed) * damageAdd);
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
