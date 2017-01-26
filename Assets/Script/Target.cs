using UnityEngine;
using System.Collections;

/** DEPRECATED **/
public class Target : MonoBehaviour {
    FiringCanons player;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("PlayerController").GetComponent<FiringCanons>();
    }

    // Update is called once per frame
    void Update()
    {/*
        if (player != null && Input.GetMouseButtonDown(0))
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 touchPos = new Vector2(wp.x, wp.y);
            if (player.getMainCanon() != null && player.getMainCanon().GetComponent<Cooldown>().getPossibility() == true &&
                GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
            {
                print("You attack!");
                player.fireOn(this.gameObject);
            }
        }*/
    }
}
