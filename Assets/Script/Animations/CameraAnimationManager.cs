using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraAnimationManager : MonoBehaviour {

    private Animator animationController;
    public float ySpeed;
    private void Start()
    {
        animationController = GetComponent<Animator>();
    }


    public void PlayAnimation(string anim) {
        Debug.Log("PlayAnimation: Requesting " + anim);
        string mode = "crossfade";

        switch (mode)
        {
            case "play":
                {
                    animationController.Play(anim);
                    break;
                }
            case "stop":
                {
                    //animationController.Stop(animName);
                    break;
                }
            case "crossfade":
                {
                    animationController.CrossFade(anim, .2f);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
