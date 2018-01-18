using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour {

	private GameManager gameManager;

	void Start() {
		gameManager = GameManager.Instance;
	}
	public void SetMusicVolume(float volume) {
		gameManager.settings.SetMusicVolume(volume);
	}

	public void SetFXVolume(float volume) {
		gameManager.settings.SetFXVolume(volume);
	}

	public void SetGraphicQuality(int quality) {
		gameManager.settings.SetGraghicSettings(quality);
	}
}
