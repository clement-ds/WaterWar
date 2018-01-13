using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableCameraTransitioner : MonoBehaviour {
    private Animator anim;
    private bool opened = false;
    public IntroSceneManager sceneManager;
    public List<string> objectAnimations;
    public string stateChange;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnMouseOver()
    {
        //TODO: Glow
        if (Input.GetMouseButtonDown(0)) {
            LaunchAnimations();
        }
    }

    void LaunchAnimations() {
        opened = !opened;
        if (objectAnimations.Count == 2) {
            anim.Play(opened ? objectAnimations[0] : objectAnimations[1]);
        }
        sceneManager.CameraStateChange(stateChange);
    }

    public void SimulateClick() {
        LaunchAnimations();
    }
}