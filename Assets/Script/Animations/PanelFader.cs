using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelFader : MonoBehaviour {

	[SerializeField] private CanvasGroup canvas;
	[SerializeField] private float time = 2;

	public void FadeOut(float addedSpeed = 0) {
		if (!canvas.gameObject.activeSelf) {
			canvas.gameObject.SetActive(true);
		}
		canvas.alpha = 1;
		StartCoroutine(FadeCoroutine(false, addedSpeed));
	}

	public void FadeIn(float addedSpeed = 0) {
		if (!canvas.gameObject.activeSelf) {
			canvas.gameObject.SetActive(true);
		}
		canvas.alpha = 0;
		StartCoroutine(FadeCoroutine(true, addedSpeed));
	}

	IEnumerator FadeCoroutine(bool fadeIn, float addedSpeed) {
		while (fadeIn ? canvas.alpha < 1 : canvas.alpha > 0) {
			float amount = (Time.deltaTime / time) + addedSpeed;
			canvas.alpha += fadeIn ? amount : -amount;
			yield return null;
		}
	}
}
