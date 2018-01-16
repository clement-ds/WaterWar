using UnityEngine;
using System.Collections;

public class SoundOnClick : MonoBehaviour {
    [SerializeField] private string clipName;
    [SerializeField] private IntroSceneManager sceneManager;

    void OnMouseDown() {
        sceneManager.PlaySound(clipName);
    }
}