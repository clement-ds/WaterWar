using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IntroSceneManager : MonoBehaviour {
    public GameObject map, menu;
    public GameManager gameManager;

	// Use this for initialization
	void Start () {

    }

    void Update() {
        if (gameManager.IsInGame() == 45) {
            if (!map.activeSelf)
            {
                map.SetActive(true);
            }
            if (menu.activeSelf)
            {
                menu.SetActive(false);
            }
        }
    }
}
