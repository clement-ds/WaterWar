using UnityEngine;
using System.Collections;

public class SoundOnCollide : MonoBehaviour {
    [SerializeField] private string clipName;
    private IntroSceneManager sceneManager;

    void Start() {
        sceneManager = GameObject.Find("SceneManager").GetComponent<IntroSceneManager>();
    }
    
    void OnCollisionEnter(Collision collision) {
        if (collision.relativeVelocity.magnitude > 2)
            sceneManager.PlaySound(clipName);
    }
}