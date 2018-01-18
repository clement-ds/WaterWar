using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StartMenuManager : MonoBehaviour {
	public List<GameObject> canvases;
	PanelFader fader;
	// Use this for initialization
	void Start () {
		fader = GetComponent<PanelFader>();
		fader.FadeOut();
	}

	public void CanvasSelector(int index) {
		for (int i = 0; i < canvases.Count; i++) {
			if (index == i) {
				canvases[i].SetActive(!canvases[i].activeSelf);
			} else {
				canvases[i].SetActive(false);
			}
		}

	}
	
}
