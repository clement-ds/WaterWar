using UnityEngine;
using System.Collections;

public class SellRessource : MonoBehaviour {
    public Canvas inventory;
    public GameObject slot;
    public GameObject item;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 touchPos = new Vector2(wp.x, wp.y);
            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
            {
                GameObject panel = GameObject.Find("SlotsPanel");
                GameObject a = (GameObject)Instantiate(slot);
                a.transform.SetParent(panel.transform, false);
                GameObject b = (GameObject)Instantiate(item);
                b.transform.SetParent(a.transform, false);
            }
        }
    }
}
