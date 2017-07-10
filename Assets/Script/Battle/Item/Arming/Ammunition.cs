using UnityEngine;
using System.Collections;

public abstract class Ammunition
{
    protected int damage;
    protected int weight;

    protected Ammunition()
    {
    }

    public int getDamage()
    {
        return this.damage;
    }

    public int getWeight()
    {
        return this.weight;
    }
}
