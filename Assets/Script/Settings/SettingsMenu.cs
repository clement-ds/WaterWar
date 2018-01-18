using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour {

	public AudioMixer mixer;
	public void SetMusicVolume(float volume) {
		mixer.SetFloat("MusicVolume", volume);
	}

	public void SetFXVolume(float volume) {
		mixer.SetFloat("FXVolume", volume);
	}

	public void SetGraphicQuality(int quality) {
		QualitySettings.SetQualityLevel(quality);
	}
}
