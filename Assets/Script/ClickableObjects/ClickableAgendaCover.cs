using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableAgendaCover : MonoBehaviour {
    private Animator anim;
    private bool opened = false;
    private CameraAnimationManager cameraAnim;

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
            anim.Play(opened ? "OpenAgenda" : "CloseAgenda");
            cameraAnim.PlayAnimation(opened ? "CameraToQuest" : "CameraFromQuestToDefault", PlayType.CROSSFADE);
        }
    }
}