using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCameraUtils : MonoBehaviour {
	public GameObject camera;
	private RenderCameraPadding cameraPaddingUtils;

	void Start() {
		cameraPaddingUtils = camera.GetComponent<RenderCameraPadding>();
	}
	void OnMouseOver() {
		if (cameraPaddingUtils && Input.GetMouseButton(1)) {
			cameraPaddingUtils.Padding();
		}
	}
}
