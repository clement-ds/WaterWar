using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

public class Battle_Player : Battle_Ship
{
    public Text distanceText = null;
    public Button actionFuite = null;
    public Button actionAbordage = null;

    private int position = 2;

    public Battle_Player() : base(200)
    {
    }


    // CREATE
    protected override void createCrew()
    {
        // Doesn't work
        /*
        GameObject crew1 = (GameObject)Instantiate(new GameObject("crew"), new Vector3(-6f, -1f, 99f), Quaternion.identity);
        // Sprite
        SpriteRenderer renderer = crew1.AddComponent<SpriteRenderer>();
        Sprite sprite = Resources.Load("pirate1", typeof(Sprite)) as Sprite;
        renderer.sprite = sprite;

        //transform
        crew1.transform.localRotation = new Quaternion(0, 0, 0, 0);
        crew1.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        crew1.transform.parent = this.transform;
        print("create" + crew1);*/
    }

    // Use this for initialization
    void Start()
    {
        distanceText.text = position.ToString();
        actionFuite.gameObject.SetActive(false);
        actionAbordage.gameObject.SetActive(false);

        this.createCrew();
    }

    // Update is called once per frame
    void Update()
    {
        this.hasInputMouse();
        distanceText.text = position.ToString();
        if (position == 3)
        {
            actionFuite.gameObject.SetActive(true);
            actionAbordage.gameObject.SetActive(false);
        }
        else if (position == 1)
        {
            actionFuite.gameObject.SetActive(false);
            actionAbordage.gameObject.SetActive(true);
        }
        else
        {
            actionFuite.gameObject.SetActive(false);
            actionAbordage.gameObject.SetActive(false);
        }

    }

    public void approcheAction()
    {
        if (position > 1) position--;
    }

    public void eloignementAction()
    {
        if (position < 3) position++;
    }

    void hasInputMouse()
    {
        Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 touchPos = new Vector2(wp.x, wp.y);
        if (Input.GetMouseButtonDown(0))
        {
            if (!this.checkSelfShip(touchPos))
            {
                this.checkEnemyShip(touchPos);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {

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
               result = target.GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos);
                if (result)
                {
                    foreach (Transform child2 in player.transform)
                    {
                        ShipElement target2 = child2.GetComponent<ShipElement>();
                        
                        if (target2 != null && target.GetInstanceID() != target2.GetInstanceID())
                        {
                            target2.unfocus();
                        }
                    }
                }
                target.hasInputMouse(result);
            }
        }
        return result;
    }

    private bool checkEnemyShip(Vector2 touchPos)
    {
        GameObject player = GameObject.Find("Player");
        GameObject enemy = GameObject.Find("Enemy");
        bool result = false;

        print("check enemy");
        foreach (Transform child in enemy.transform)
        {
            ShipElement target = child.GetComponent<ShipElement>();

            if (target != null)
            {
                print("target: " + target);
                if (target.GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
                {
                    print("target clicked");
                    foreach (Transform child2 in player.transform)
                    {
                        Canon target2 = child2.GetComponent<Canon>();

                        if (target2)
                        print("canon : " + target2.isFocused());
                        if (target2 != null && target2.isFocused())
                        {
                            print("change target to : " + target);
                            //transform.GetComponentInParent<FiringCanons>().fireOn(target);
                            target2.setTarget(target);
                            result = true;
                        }
                    }
                }
            }
        }
        return result;
    }

    /** CREW MANAGER **/
    private Battle_CrewMember getSelectedCrewMember()
    {
        GameObject player = GameObject.Find("Player");
        foreach (Transform child in player.transform)
        {
            Battle_CrewMember target = child.GetComponent<Battle_CrewMember>();

            if (target != null && target.isSelected())
            {
                return target;
            }
        }
        return null;
    }
}
