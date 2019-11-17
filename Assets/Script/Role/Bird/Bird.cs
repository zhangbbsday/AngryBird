using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public enum State
    {
        OnGround = 0,
        OnSlingShot = 1,
        Shot = 2,
        InSky = 3,
        Collision = 4,
        Scroll = 5,
        Dead = 6
    }

    public AudioClip NormalSong;
    public AudioClip ChoseSong;
    public AudioClip ShotSong;
    public AudioClip CollisionSong;
    public AudioClip SkillSong;
    public AudioClip DestorySong;

    protected int hp;
    protected int[] damage;
    protected int weight;
    protected AudioSource audioSource;
    protected Animator animator;
    protected float existTime;

    public int Hp { get; set; }
    public int[] Damage { get; protected set; }
    public int Weight { get; protected set; }
    public Rigidbody2D RigidbodySelf { get; protected set; }
    public State BirdState { get; set; }

    
    protected void Start()
    {
        Initialize();
    }

    protected void Update()
    {
        UpdataThings();
    }
    public void DestroyThis()
    {
        Destroy(gameObject);
    }

    protected virtual void Initialize()
    {
        RigidbodySelf = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        BirdState = State.OnGround;
    }
    protected virtual void UseSkill()
    {
        if (Input.GetMouseButtonDown(0))
            audioSource.PlayOneShot(SkillSong);

        //使用技能
    }
    protected virtual void DestroySelf()
    {
        audioSource.PlayOneShot(DestorySong);

        //播放销毁动画，交由动画动作销毁物体
    }

    protected virtual void UpdataThings()
    {

    }

    protected IEnumerable Dead()
    {
        yield return new WaitForSeconds(existTime);
        BirdState = State.Dead;
        DestroySelf();
    }
}
