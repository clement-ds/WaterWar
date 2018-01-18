using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelFader : MonoBehaviour {

	[SerializeField] private CanvasGroup canvas;
	[SerializeField] private float time = 2;

	public void FadeOut() {
		if (!canvas.gameObject.activeSelf) {
			canvas.gameObject.SetActive(true);
		}
		canvas.alpha = 1;
		StartCoroutine(FadeCoroutine(false));
	}

	public void FadeIn() {
		if (!canvas.gameObject.activeSelf) {
			canvas.gameObject.SetActive(true);
		}
		canvas.alpha = 0;
		StartCoroutine(FadeCoroutine(true));
	}

	IEnumerator FadeCoroutine(bool fadeIn) {
		while (fadeIn ? canvas.alpha < 1 : canvas.alpha > 0) {
			float amount = Time.deltaTime / time;
			canvas.alpha += fadeIn ? amount : -amount;
			yield return null;
		}
	}
}
