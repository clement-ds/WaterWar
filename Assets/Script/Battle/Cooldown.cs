using UnityEngine;
using System.Collections;

public class Cooldown : MonoBehaviour {

    public float timeLeft = 30.0f;
    private float startTime = 0;
    private bool possibility = false;

    // Use this for initialization
    void Start() {
        startTime = timeLeft;
    }

    // Update is called once per frame

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0) {
            possibility = true;
        } else {
            possibility = false;
        }
    }

    bool getPossibility() {
        if (possibility == true)
        {
            timeLeft = startTime;
            possibility = false;
        }
        return possibility;    
    }
}
