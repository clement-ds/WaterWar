using UnityEngine;
using System.Collections;

public class DetectClick : MonoBehaviour {

    public bool inside = false;
    void Start () {

    }
	
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 touchPos = new Vector2(wp.x, wp.y);
            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
            {
                Debug.Log("Click"); 
                inside = true;
            }
            else
                inside = false;
        }
    }

    public bool getInside()
    {
        return inside;
    }

}
