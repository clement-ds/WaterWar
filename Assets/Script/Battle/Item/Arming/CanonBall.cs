using UnityEngine;
using System.Collections;

public class CanonBall : Ammunition {

    public CanonBall(float ratioCanon, float ratioCrew)
    {
        this.damage = 20 * ratioCanon * ratioCrew;
        this.weight = 2;
    }
}
