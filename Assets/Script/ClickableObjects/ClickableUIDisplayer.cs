using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableUIDisplayer : MonoBehaviour {

	public UIController controller; 
	[SerializeField] private GameObject button;
	// Use this for initialization

	void OnMouseDown() {
		if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) {
			if (button) {
				button.SetActive(!button.activeSelf);
			}
			controller.TogglePanel();
        }
	}
}
