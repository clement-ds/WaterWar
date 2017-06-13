using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopInteraction : MonoBehaviour {
    GameObject shopMenu;

    void Start () {
        shopMenu = GameObject.Find("ShopCanvas");
        shopMenu.SetActive(false);
    }

  void Update () {
      if (Input.GetMouseButtonDown(0)) {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 touchPos = new Vector2(wp.x, wp.y);
            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos) && shopMenu) {
                shopMenu.SetActive(true);
            }
        }
      if (Input.GetKeyDown(KeyCode.Escape) && shopMenu)
        shopMenu.SetActive(false);
    }
}
