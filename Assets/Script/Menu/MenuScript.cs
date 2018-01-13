using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {
    public Canvas inventory;
    public Canvas crew;
    
    GameObject menu;

    // Use this for initialization
    void Start() {
        menu = GameObject.Find("UI_Interaction_Canvas");
//        menu.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OpenMenu() {
        menu.SetActive(!menu.activeSelf);
        inventory.enabled = false;
        crew.enabled = false;
    }

    public void CloseMenu() {
        menu.SetActive(false);
    }

    public void OpenInventory() {
        inventory.enabled = true;
    }

    public void OpenCrew() {
        crew.enabled = true;
    }
}
