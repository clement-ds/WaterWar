using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableUIDisplayer : MonoBehaviour {

	public UIController controller; 
	// Use this for initialization

	void OnMouseDown() {
		controller.TogglePanel();
	}
}
