using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SoundManager : MonoBehaviour 
{
    private AudioSource audioSource;
    public List<AudioClip> clips;

    void Start() {
      audioSource = GetComponent<AudioSource>();
    }
    public void PlaySingle(string soundName)
    {
      AudioClip clip = clips.Find((c) => c.name == soundName);
      if (clip) {
        audioSource.clip = clip;
        audioSource.Play();
      }
    }
}