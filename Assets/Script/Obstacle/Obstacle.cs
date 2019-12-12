using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Obstacle : MonoBehaviour, IPassiveDamageObject
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
    public ParticleSystem particle;
    public Sprite[] particleSprites;

    private Text text;
    private Transform canvas;
    private HurtState hurtState;
    private SpriteRenderer sprite;
    private AudioSource audioSource;
    private readonly float criticalSpeed = 6.0f;
    private readonly float destoryTime = 0.1f;
    private readonly float effectExitTime = 1.5f;
    private readonly int maxScore = 100;
    private readonly int[] scoreRandom = { 10, 20, 30, 50 };

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        text = GameObjectContainer.Instacne.FindGameObjectComponent<Text>("DamageScore");
        canvas = GameObjectContainer.Instacne.FindGameObjectComponent<Transform>("ScoreUI");


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

    public void ChangeHp(float changeNum, bool isBirdChange = false)
    {
        Hp -= changeNum;
        ChangeHurtState();
        SetHurtEffect();

        if (isBirdChange || hurtState == HurtState.Destroy)
            ShowScore(maxScore);
        
    }

    public void ShowScore(int score)
    {
        int scoreText = score;
        if (hurtState != HurtState.Destroy)
            scoreText = scoreRandom[Random.Range(0, scoreRandom.Length)];

        Text t = GameObject.Instantiate(text, transform.position + Vector3.up, Quaternion.identity, canvas);
        t.text = scoreText.ToString();
        Destroy(t.gameObject, 0.5f);

        GameManager.Instance.ScoreSystemControl.GetScore(scoreText);
    }

    private void SetParticle(Vector2 position)
    {
        ParticleSystem obj = GameObject.Instantiate(particle, position, Quaternion.identity);
        foreach (Sprite sprite in particleSprites)
        {
            obj.textureSheetAnimation.AddSprite(sprite);
        }
        obj.Play();
        Destroy(obj.gameObject, effectExitTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude > criticalSpeed)
            SetParticle(collision.GetContact(0).point);

        if (hurtState == HurtState.Destroy || collision.collider.GetComponent<Bird>() != null)
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
}
