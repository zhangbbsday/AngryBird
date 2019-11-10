using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    public int Hp { get; set; }
    public Rigidbody2D RigidbodySelf { get; private set; }

    private Animator animator;
    private AudioSource audioSource;
    void Start()
    {
        Initialize();
    }

    void Update()
    {
        
    }

    protected virtual void Initialize()
    {
        RigidbodySelf = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }
}
