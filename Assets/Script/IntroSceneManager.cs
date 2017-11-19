using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IntroSceneManager : MonoBehaviour {
    public GameObject map, menu;
    private CameraAnimationManager cameraManager;

	// Use this for initialization
	void Start () {
        GameObject o = GameObject.FindGameObjectWithTag("MainCamera");
        cameraManager = o.GetComponent<CameraAnimationManager>();
        Debug.Log("ISM: IsInGame: " + GameManager.Instance.IsInGame());
        if (GameManager.Instance.IsInGame() != 45)
        {
            cameraManager.PlayAnimation("CameraStart", PlayType.PLAY);
        }
    }

    void Update() {
        if (GameManager.Instance.IsInGame() == 45) {
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
