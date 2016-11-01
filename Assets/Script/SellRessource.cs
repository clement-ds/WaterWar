using UnityEngine;
using System.Collections;

public class SellRessource : MonoBehaviour {
    Inventory inventory;
    public int ressourceNumber;
    // Use this for initialization
    void Start () {
	    inventory = GameObject.Find("Inventory System").GetComponent<Inventory>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 touchPos = new Vector2(wp.x, wp.y);
            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
            {
                inventory.AddItem(ressourceNumber);
            }
        }
    }
}
