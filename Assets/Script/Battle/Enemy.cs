using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/** DEPRECATED **/
public class Enemy : MonoBehaviour {
    public Slider slider = null;

    private int life = 100;

    // Use this for initialization
    void Start () {
        slider.value = life;
     }

    // Update is called once per frame
    void Update () {
        slider.value = life;
    }
}
