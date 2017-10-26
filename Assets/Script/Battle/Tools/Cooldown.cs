using UnityEngine;
using System.Collections;

public class Cooldown {

    public float timeLeft = 30.0f;
    private float startTime = 0;
    private bool possibility = false;

    // Use this for initialization
    public Cooldown() {
        startTime = timeLeft;
    }

    // Update is called once per 
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0) {
            possibility = true;
        } else {
            possibility = false;
        }
    }

    public bool getPossibility() {
        return possibility;
    }

    public void resetPossibility() {
        timeLeft = startTime;
        possibility = false;
    }
}
