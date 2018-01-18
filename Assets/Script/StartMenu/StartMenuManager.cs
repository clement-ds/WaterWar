using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuManager : MonoBehaviour {

	PanelFader fader;
	// Use this for initialization
	void Start () {
		fader = GetComponent<PanelFader>();
		fader.FadeOut();
	}
	
}
