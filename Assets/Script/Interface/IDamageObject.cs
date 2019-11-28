using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IDamageObject
{
    float Hp { get; set; }
    float Damage { get; set; }
    void ChangeHp(float damage);
}
