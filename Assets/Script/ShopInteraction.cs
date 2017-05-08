using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopInteraction : MonoBehaviour {
    GameObject shopMenu;
    Inventory inventory;
    public int ressourceNumber;
    public GameObject shopList;

    void Start () {
        shopMenu = GameObject.Find("ShopCanvas");
        shopMenu.SetActive(false);
        inventory = GameObject.Find("Inventory System").GetComponent<Inventory>();
    }

    void FillSellShop() {
        IslandManager im = IslandManager.GetInstance();
        IslandInventory inv = im.island.inventory;
        GameObject item = GameObject.Find("item");

        for (int i = 0; i < inv.food.Count; ++i)
        {
            //if (i == 0)
            //{
                InventoryObject invObj = inv.food[i];
                Text text = item.GetComponent<Text>();
                text.text = "";
                text.text = invObj.name + " " + invObj.number;
            //} else
            //{

            //}

        }
    }

    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 touchPos = new Vector2(wp.x, wp.y);
            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos)) {
                shopMenu.SetActive(!shopMenu.activeSelf);
                FillSellShop();
                //                inventory.AddItem(ressourceNumber);
            }
        }
    }
}
