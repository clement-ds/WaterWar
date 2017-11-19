using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayType
{
    PLAY,
    STOP,
    CROSSFADE
}

public class CameraAnimationManager : MonoBehaviour {

    public AnimationClip[] animations;
    private Animation animationController;

    private void Start()
    {
        animationController = gameObject.GetComponent(typeof(Animation)) as Animation;

        foreach (AnimationClip clip in animations)
        {
            Debug.Log("Added Clip: " + clip);
            animationController.AddClip(clip, clip.name);
        }
    }

    public void PlayAnimation(string animName, PlayType type) {
        Debug.Log("PlayAnimation: Requesting " + animName);
        AnimationClip clip = GetAnimFromName(animName);
        if (!clip)
        {
            Debug.LogWarning("Couldn't find animation clip with name '" + animName + "'");
            return;
        }

        switch (type)
        {
            case PlayType.PLAY:
                {
                    animationController.Play(animName);
                    break;
                }
            case PlayType.STOP:
                {
                    animationController.Stop(animName);
                    break;
                }
            case PlayType.CROSSFADE:
                {
                    animationController.CrossFade(animName);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    private AnimationClip GetAnimFromName(string name)
    {
        foreach (AnimationClip clip in animations) {
            if (clip.name.Equals(name))
            {
                return clip;
            }
        }
        return null;
    }
}
