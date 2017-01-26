using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Battle_Enemy : Ship
{
    // Use this for initialization
    void Start() {
        slider.value = life;
    }

    // Update is called once per frame
    void Update() {
        slider.value = life;
    }
}
