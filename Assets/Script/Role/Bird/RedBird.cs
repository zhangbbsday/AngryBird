using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBird : Bird
{
    protected override void Initialize()
    {
        base.Initialize();
        DamageCoefficient = new float[3] { 1.5f, 1.0f, 0.8f };
        scoreColor = Color.red;
    }
}
