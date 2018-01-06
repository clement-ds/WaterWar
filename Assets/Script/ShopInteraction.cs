using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopInteraction : MonoBehaviour {
    GameObject shopMenu;

    void Start () {
        // shopMenu = GameObject.Find("ShopCanvas");
        // shopMenu.SetActive(false);
    }

    public void TriggerButton()
    {
        // shopMenu.SetActive(!shopMenu.active);
    }

    void Update () {
    //   if (Input.GetKeyDown(KeyCode.Escape) && shopMenu)
    //     shopMenu.SetActive(false);
    }
}
