using UnityEngine;
using System.Collections;

public abstract class ShipElement : MonoBehaviour
{
    protected int life = 0;
    protected bool available = true;

    public bool repair()
    {
        // todo, sailor in parameter
        this.doRepairAction();
        return true;
    }

    protected abstract void doRepairAction();

    public bool doDamage()
    {
        if (this.available)
        {
            this.doDamageAction();
            this.doDamageAnimation();
            return true;
        }
        return false;
    }

    protected abstract void doDamageAction();

    protected abstract void doDamageAnimation();

    public int receiveDamage(int damage)
    {
        if (this.available)
        {
            int resultDamage = this.receiveDamageAction(damage);
            this.receiveDamageAnimation();
            return resultDamage;
         }
        return -1;
    }

    protected abstract int receiveDamageAction(int damage);

    protected abstract void receiveDamageAnimation();


    /** GETTERS **/

    public int getLife()
    {
        return this.life;
    }

    public bool isAvailable()
    {
        return this.available;
    }

    /** SETTERS **/
    public void setLife(int value)
    {
        this.life = value;
        this.available = (this.life > 0);
    }

    public void setAvailable(bool value)
    {
        this.available = value;
    }
}