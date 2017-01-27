using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Battle_Player : Battle_Ship {
    public Text distanceText = null;
    public Button actionFuite = null;
    public Button actionAbordage = null;
    
    private int position = 2;

    public Battle_Player() : base(200)
    {
    }

    // Use this for initialization
    void Start () {
        distanceText.text = position.ToString();
        actionFuite.gameObject.SetActive(false);
        actionAbordage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        this.hasInputMouse();
        distanceText.text = position.ToString();
        if (position == 3) {
            actionFuite.gameObject.SetActive(true); 
            actionAbordage.gameObject.SetActive(false);
        } else if (position == 1) {
            actionFuite.gameObject.SetActive(false);
            actionAbordage.gameObject.SetActive(true);
        } else {
            actionFuite.gameObject.SetActive(false);
            actionAbordage.gameObject.SetActive(false);
        }

    }

    public void approcheAction() {
        if (position > 1) position--;
    }

    public void eloignementAction() {
        if (position < 3) position++;
    }

    void hasInputMouse() {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 touchPos = new Vector2(wp.x, wp.y);

            if (!this.checkSelfShip(touchPos))
            {
                this.checkEnemyShip(touchPos);
            }
        }
    }

    private bool checkSelfShip(Vector2 touchPos)
    {
        GameObject player = GameObject.Find("Player");
        bool result = false;

        foreach (Transform child in player.transform)
        {
            ShipElement target = child.GetComponent<ShipElement>();

            if (target != null)
            {
                if (target.GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
                {
                    // todo find a way to use a preselected sailor here (maybe assign sailor first to some place)
                    target.repair();
                    result = true;
                }
            }
        }
        return result;
    }

    private bool checkEnemyShip(Vector2 touchPos)
    {
        GameObject player = GameObject.Find("Enemy");
        bool result = false;

        foreach (Transform child in player.transform)
        {
            ShipElement target = child.GetComponent<ShipElement>();

            if (target != null)
            {
                if (target.GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
                {
                    transform.GetComponentInParent<FiringCanons>().fireOn(target);
                    result = true;
                }
            }
        }
        return result;
    }

}
