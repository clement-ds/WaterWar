using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaloDisplayer : MonoBehaviour {

	Behaviour halo;
	void Start() {
 		halo = (Behaviour)GetComponent("Halo");
		halo.enabled = false;
	}

	void OnMouseEnter() {
		if (halo && !halo.enabled) {
			halo.enabled = true;
		}
	}

	void OnMouseExit() {
		if (halo && halo.enabled) {
			halo.enabled = false;
		}
	}
}
