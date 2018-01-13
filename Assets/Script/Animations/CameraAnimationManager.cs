using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraAnimationManager : MonoBehaviour {

    private Animator animationController;
    private void Start()
    {
        animationController = GetComponent<Animator>();
    }



    public void StateChange(string state, bool hasForcedState = false, bool forcedState = false) {
        animationController.SetBool(state, hasForcedState ? forcedState : !animationController.GetBool(state));
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
