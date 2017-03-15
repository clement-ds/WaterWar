using UnityEngine;
using System.Collections;

public class Battle_CrewMember : MonoBehaviour
{

    ShipElement room = null;
    CrewMember member = null;
    bool selected = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.hasInputMouse();
    }

    /** INPUT **/
    void hasInputMouse()
    {
        Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 touchPos = new Vector2(wp.x, wp.y);
        if (Input.GetMouseButtonDown(0))
        {
            if (this.GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
            {
                this.select();
            }
            else
            {
                this.unselect();
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            GameObject player = GameObject.Find("Player");
            foreach (Transform child in player.transform)
            {
                ShipElement target = child.GetComponent<ShipElement>();
                if (target != null && target.GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
                {
                    this.transform.SetParent(target.transform);
                    this.transform.localPosition = new Vector3(0, 0, 0);
                }
            }
        }
    }

    /** INTERACTION **/
    public void select()
    {
        this.selected = true;
        this.GetComponent<SpriteOutline>().enabled = true;
    }

    public void unselect()
    {
        this.selected = false;
        this.GetComponent<SpriteOutline>().enabled = false;
    }

    /** GETTERS **/
    public bool isSelected()
    {
        return this.selected;
    }

    public string getId()
    {
        return this.member.getId();
    }
}
