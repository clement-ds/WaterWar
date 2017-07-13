using UnityEngine;
using System.Collections;

public abstract class Ammunition
{
    protected float damage;
    protected int weight;

    protected Ammunition()
    {
    }

    public float getDamage()
    {
        return this.damage;
    }

    public int getWeight()
    {
        return this.weight;
    }
}
