using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IPassiveDamageObject
{
    float Hp { get; set; }
    float Damage { get; set; }
    void ChangeHp(float damage, bool isBirdChange);
    void ShowScore(int score);
}
