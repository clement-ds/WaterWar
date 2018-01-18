using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Settings : MonoBehaviour {

    public AudioMixer audioMixer;
    public float musicVolume = 0;
    public float fxVolume = 0;
    public int graphicSetting = 2;

    public void ApplySettings() {
        audioMixer.SetFloat("MusicVolume", musicVolume);
		audioMixer.SetFloat("FXVolume", fxVolume);
		QualitySettings.SetQualityLevel(graphicSetting);
    }

	public void SetMusicVolume(float volume) {
        audioMixer.SetFloat("MusicVolume", volume);
	}
	public void SetFXVolume(float volume) {
        audioMixer.SetFloat("FXVolume", volume);
	}
	public void SetGraghicSettings(int setting) {
		QualitySettings.SetQualityLevel(setting);
	}
}
