using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StartMenuManager : MonoBehaviour {
	public List<GameObject> canvases;
	PanelFader fader;
	public GameObject LoadingCanvas;

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

    public void ContinueGame()
    {
	fader.FadeIn(.01f);

	if (LoadingCanvas) {
		LoadingCanvas.SetActive(true);
	}
        Invoke("ContinueGameDelayed", 2);
    }

    public void NewGame()
    {
	fader.FadeIn(.01f);

	if (LoadingCanvas) {
		LoadingCanvas.SetActive(true);
	}
	Invoke("NewGameDelayed", 2);
    }

    private void ContinueGameDelayed() {
	GameManager.Instance.continueGame();    
    }

    private void NewGameDelayed() {
	GameManager.Instance.newGame();    
    }


}
