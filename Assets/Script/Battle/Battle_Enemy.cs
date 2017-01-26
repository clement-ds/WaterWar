using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Battle_Enemy : Battle_Ship
{
    // Use this for initialization
    void Start() {
        slider.value = currentLife;
    }

    // Update is called once per frame
    void Update() {
        slider.value = currentLife;
    }
}
