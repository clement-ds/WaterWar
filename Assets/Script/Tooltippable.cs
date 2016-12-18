using UnityEngine;
using System.Collections;

public class Tooltippable : MonoBehaviour {
    Tooltip tooltip;
    // Use this for initialization
    void Start () {
        tooltip = GameObject.Find("Tooltip").GetComponent<Tooltip>();
    }
	
	// Update is called once per frame
	void Update () {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 touchPos = new Vector2(wp.x, wp.y);
            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
            {
                tooltip.show(this.gameObject);
            }
    }
}
