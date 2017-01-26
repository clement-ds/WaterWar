using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Battle_Ship : MonoBehaviour
{
    public Slider slider = null;
    protected int life = 100;
    protected int currentLife = 100;

    public void updateSliderValue() {
        slider.value = (currentLife * 100) / life;
    }

    public void receiveDamage(int damage) {
        currentLife -= damage;
    }

    /** GETTERS **/
    public int getCurrentLife() {
        return this.currentLife;
    }

    /** SETTERS **/
    public void setCurrentLife(int value) {
        this.currentLife = value;
        this.updateSliderValue();
    }
}
