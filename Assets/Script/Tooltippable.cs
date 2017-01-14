using UnityEngine;
using System.Collections;

public class Tooltippable : MonoBehaviour {
    Tooltip tooltip;
    // Use this for initialization
    public string tooltipName = "Tooltip";
    void Start () {
        tooltip = GameObject.Find(tooltipName).GetComponent<Tooltip>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            tooltip.hideOn(this.gameObject);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 touchPos = new Vector2(wp.x, wp.y);
            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
            {
                tooltip.showOn(this.gameObject);
            }
            else
            {
                tooltip.hideOn(this.gameObject);
            }
        }
    }
}
