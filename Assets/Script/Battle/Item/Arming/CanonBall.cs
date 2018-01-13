using UnityEngine;
using System.Collections;

public class CanonBall : Ammunition {

    public CanonBall(float baseCanonDamage, float ratioCrew)
    {
        this.damage = baseCanonDamage * ratioCrew;
        this.weight = 2;
    }
}
