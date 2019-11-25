using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    public float winkTimeMax;
    public float singTimeMax;
    public float Hp { get => hp; set { hp = value; } }
    public Rigidbody2D RigidbodySelf { get; private set; }

    [SerializeField]
    private float hp;
    private Animator animator;
    private AudioSource audioSource;
    private IEnumerator winkMethod;
    private IEnumerator singMethod;
    void Start()
    {
        Initialize();

        StartCoroutine(winkMethod);
        StartCoroutine(singMethod);
    }

    void Update()
    {
        
    }

    protected virtual void Initialize()
    {
        RigidbodySelf = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        winkMethod = Wink();
        singMethod = Sing();
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
            //GameManager.Instance.AudioSystemControl.Play(audioSource, AudioSystem.SoundsName.);
        }
    }
}
