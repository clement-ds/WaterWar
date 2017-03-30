using UnityEngine;
using System.Collections;

//DEPRECATED
public class SetAsCanonOnClick : MonoBehaviour {
    FiringCanons player;
    public string bouletname { get; set; }

    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player").GetComponent<FiringCanons>();
        print("Player canons: " + player.name);
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
                if (player.getMainCanon()) {
                    player.getMainCanon().GetComponent<SpriteOutline>().enabled = false;
                }
                player.setMainCanon(this.gameObject);
                player.getMainCanon().GetComponent<SpriteOutline>().enabled = true;
            }
        }
    }

    public void setBoulet(GameObject newBoulet)
    {
        /*Get boulet effect*/
        bouletname = newBoulet.GetComponent<SelectAsBoulet>().bouletname;
    }
}
