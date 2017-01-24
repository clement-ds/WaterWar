using UnityEngine;
using System.Collections;

public class SetAsCanonOnClick : MonoBehaviour {
    FiringCanons player;
    public string bouletname { get; set; }

    // Use this for initialization
    void Start () {
        player = GameObject.Find("PlayerController").GetComponent<FiringCanons>();
        bouletname = "Default";
    }
    
    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 touchPos = new Vector2(wp.x, wp.y);
            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
            {
                if (player.MainCanon) {
                    player.MainCanon.GetComponent<SpriteOutline>().enabled = false;
                }
                player.MainCanon = this.gameObject;
                player.MainCanon.GetComponent<SpriteOutline>().enabled = true;
            }
        }
    }

    public void setBoulet(GameObject newBoulet)
    {
        /*Get boulet effect*/
        bouletname = newBoulet.GetComponent<SelectAsBoulet>().bouletname;
    }
}
