using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IDamageObject
{
    public float Damage { get; set; }
    public float damage;
    public float hpMax;

    private float hp;

    private void Start()
    {
        hp = hpMax;
        Damage = damage;
    }

    private void ChangeHurtState()
    {

    }

    private void Hurt()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
