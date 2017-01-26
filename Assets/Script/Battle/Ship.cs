using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Ship : MonoBehaviour
{
    public Slider slider = null;
    protected int life = 100;

    public void receiveDamage(int damage) {
        life -= damage;
    }

    /** GETTERS **/
    public int getCurrentLife() {
        return this.life;
    }

    /** SETTERS **/
    public void setCurrentLife(int value) {
        this.life = value;
    }
}
