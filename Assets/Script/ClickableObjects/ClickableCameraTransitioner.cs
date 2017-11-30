using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableCameraTransitioner : MonoBehaviour {
    private Animator anim;
    private bool opened = false;
    private CameraAnimationManager cameraAnim;
    public List<string> objectAnimations;
    public string cameraAnimations;

    void Start()
    {
        anim = GetComponent<Animator>();
        cameraAnim = GameObject.Find("MainCamera").GetComponent<CameraAnimationManager>();
    }

    void OnMouseOver()
    {
        //TODO: Glow
        if (Input.GetMouseButtonDown(0)) {
            opened = !opened;
            anim.Play(opened ? objectAnimations[0] : objectAnimations[1]);
            cameraAnim.PlayAnimation(opened? cameraAnimations : cameraAnimations + "ToDefault");
        }
    }
}