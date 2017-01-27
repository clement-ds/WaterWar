using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Battle_Ship : MonoBehaviour
{
    public Slider slider = null;
    protected readonly int life;
    protected int currentLife;

    protected Battle_Ship(int lifeValue)
    {
        life = lifeValue;
        this.setCurrentLife(life);
    }

    public void updateSliderValue() {
        if (slider)
        {
            slider.value = (currentLife * 100) / life;
        }
    }

    public void receiveDamage(int damage) {
        this.setCurrentLife(this.currentLife - damage);
    }

    /** GETTERS **/
    public int getCurrentLife() {
        return this.currentLife;
    }

    /** SETTERS **/
    public void setCurrentLife(int value) {
        this.currentLife = value;
        this.currentLife = (value < 0 ? 0 : value);
        this.currentLife = (this.currentLife > this.life ? this.life : this.currentLife);
        this.updateSliderValue();
    }
}
