using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Battle_Player : Ship {
    public Text distanceText = null;
    public Button actionFuite = null;
    public Button actionAbordage = null;
    
    private int position = 2;


    // Use this for initialization
    void Start () {
        slider.value = life;
        distanceText.text = position.ToString();
        actionFuite.gameObject.SetActive(false);
        actionAbordage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        this.hasInputMouse();
        slider.value = life;
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
            GameObject player = GameObject.Find("Enemy");

            foreach (Transform child in player.transform)
            {
                ShipElement target = child.GetComponent<ShipElement>();

                if (target != null)
                {
                    if (target.GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
                    {
                        transform.GetComponentInParent<FiringCanons>().fireOn(target);
                    }
                }
            }
        }
    }

}
