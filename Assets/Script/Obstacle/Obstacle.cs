using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IDamageObject
{
    enum HurtState
    {
        Normal = 0,
        Little = 1,
        Bad = 2,
        VeryBad = 3,
        Destroy = 4
    }

    public float Damage { get; set; }
    public float Hp { get; set; }

    public Sprite[] sprites;
    public float damage;
    public float hpMax;

    private HurtState hurtState;
    private SpriteRenderer sprite;
    private float criticalSpeed = 6f;
    private AudioSource audioSource;
    private float destoryTime = 0.1f;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        Hp = hpMax;
        Damage = damage;
        hurtState = HurtState.Normal;
        audioSource.volume = 0.6f;
    }

    private void ChangeHurtState()
    {
        float percent = Hp / hpMax;

        if (Mathf.Approximately(percent, 1.0f))
            return;

        if (percent > 0.8)
            hurtState = HurtState.Normal;
        else if (percent > 0.6)
            hurtState = HurtState.Little;
        else if (percent > 0.3)
            hurtState = HurtState.Bad;
        else if (percent > 0)
            hurtState = HurtState.VeryBad;
        else
            hurtState = HurtState.Destroy;
    }

    private void SetHurtEffect()
    {
        if (hurtState == HurtState.Destroy)
        {
            StartCoroutine(DestroyThis());
            GameManager.Instance.AudioSystemControl.Play(audioSource, tag + "Destroy");
            return;
        }

        sprite.sprite = sprites[(int)hurtState];

        GameManager.Instance.AudioSystemControl.Play(audioSource, tag + "Damage");
    }

    private IEnumerator DestroyThis()
    {
        yield return new WaitForSeconds(destoryTime);
        Destroy(gameObject);
    }

    public void ChangeHp(float changeNum)
    {
        Hp -= changeNum;
        ChangeHurtState();
        SetHurtEffect();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hurtState == HurtState.Destroy || collision.collider.CompareTag("Bird"))
            return;

        IDamageObject damageObject = collision.collider.GetComponent<IDamageObject>();
        float damageAdd = 1.0f;

        if (collision.relativeVelocity.magnitude > criticalSpeed)
        {
            if (damageObject != null)
                damageAdd = damageObject.Damage;
            ChangeHp((collision.relativeVelocity.magnitude - criticalSpeed) * damageAdd);
        }
    }
}
