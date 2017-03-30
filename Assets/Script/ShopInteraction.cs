using UnityEngine;
using System.Collections;

public class ShopInteraction : MonoBehaviour {
    GameObject shopMenu;
    Inventory inventory;
    public int ressourceNumber;

    void Start () {
        shopMenu = GameObject.Find("ShopCanvas");
        shopMenu.SetActive(false);
        inventory = GameObject.Find("Inventory System").GetComponent<Inventory>();
    }

    void FillSellShop() {
        /*
         * 
         *
         */
    }

    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 touchPos = new Vector2(wp.x, wp.y);
            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos)) {
                shopMenu.SetActive(!shopMenu.activeSelf);
                 
                //                inventory.AddItem(ressourceNumber);
            }
        }
    }
}
