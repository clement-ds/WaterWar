using UnityEngine;
using System.Collections;

public class CarpenterInteraction : MonoBehaviour {
    GameObject carpenterMenu;

    void Start () {
        carpenterMenu = GameObject.Find("CarpenterCanvas");
        carpenterMenu.SetActive(false);
    }

    public void TriggerButton()
    {
        carpenterMenu.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && carpenterMenu)
            carpenterMenu.SetActive(false);
    }
}
