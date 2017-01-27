using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class ShipElement : MonoBehaviour
{
    protected readonly int life;
    protected int currentLife;
    protected bool available = true;
    public Slider slider = null;

    protected ShipElement(int lifeValue)
    {
        life = lifeValue;
        this.setCurrentLife(life);
    }

    /** SLIDER HP **/
    public void updateSliderValue() {
        if (slider) {
            slider.value = (currentLife * 100) / life;
        }
    }

    void OnMouseEnter()
    {
        if (slider)
        {
            slider.enabled = true;
        }
    }

    void OnMouseExit()
    {
        if (slider)
        {
            slider.enabled = false;
        }
    }

    /** ON HIT EFFECT **/
    protected abstract void dealDamageAsRepercution(int damage);

    protected abstract void dealDamageOnDestroy();

    protected abstract void applyMalusOnHit();

    protected abstract void applyMalusOnDestroy();

    /** REPAIR **/
    public bool repair() {
        // todo, sailor in parameter
        this.doRepairAction();
        return true;
    }

    protected abstract void doRepairAction();

    /** DO DAMAGE **/
    public bool doDamage() {
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

    /** RECEIVE DAMAGE **/
    public bool receiveDamage(int damage)
    {
        if (this.available)
        {
            this.receiveDamageAction(damage);
            this.receiveDamageAnimation();
            this.dealDamageAsRepercution(damage);
            this.applyMalusOnHit();
            return true;
         }
        return false;
    }

    protected abstract void receiveDamageAction(int damage);

    protected abstract void receiveDamageAnimation();


    /** GETTERS **/
    public int getLife() {
        return this.life;
    }

    public int getCurrentLife() {
        return this.currentLife;
    }

    public bool isAvailable() {
        return this.available;
    }

    /** SETTERS **/
    public void setCurrentLife(int value) {
        this.currentLife = (value < 0 ? 0 : value);
        this.currentLife = (this.currentLife > this.life ? this.life : this.currentLife);
        this.updateSliderValue();
        this.available = (this.currentLife > 0);
        if (!available)
        {
            this.dealDamageOnDestroy();
            this.applyMalusOnDestroy();
        }
    }

    public void setAvailable(bool value) {
        this.available = value;
    }
}