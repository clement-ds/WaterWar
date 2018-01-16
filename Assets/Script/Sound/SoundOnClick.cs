using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlaySoundOnClick : MonoBehaviour {
    [SerializeField] private string clipName;
    public IntroSceneManager sceneManager;

   void Start() {
    }
    public void OnMouseDown() {
        sceneManager.PlaySound(clipName);
    }
}