using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CooldownButton : MonoBehaviour {
    public float timeLeft = 15.0f;
    private float startTime = 0;

    // Use this for initialization
    void Start() {
        startTime = timeLeft;
    }

    // Update is called once per frame
    void Update() {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0) {
            this.gameObject.GetComponent<Button>().interactable = true;
        }
        else {
            this.gameObject.GetComponent<Button>().interactable = false;
        }
    }

    public void Reset() {
        timeLeft = startTime;
        this.gameObject.GetComponent<Button>().interactable = false;
    }
}
