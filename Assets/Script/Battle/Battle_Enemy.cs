using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Battle_Enemy : Battle_Ship
{
    public Battle_Enemy() : base(200)
    {
    }

    // CREATE
    protected override void createCrew()
    {
    }

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        //this.attackMode();
    }

    /** ATTACK **/
    public void attackMode()
    {
        ShipElement target = this.findTargetElement();

        if (target)
        {
            bool find = false;
            foreach (Transform child in this.transform)
            {
                Canon canon = child.GetComponent<Canon>();

                if (canon != null && canon.isAvailable() && canon.GetComponent<Cooldown>().getPossibility())
                {
                    transform.GetComponentInParent<FiringCanons>().setMainCanon(canon.gameObject);
                    transform.GetComponentInParent<FiringCanons>().fireOn(target);
                    find = true;
                    break;
                }
            }
            if (!find)
            {
                this.repearCanon();
            }
        }
    }

    /** RESEARCH **/
    ShipElement findTargetElement()
    {
        GameObject player = GameObject.Find("Player");

        foreach (Transform child in player.transform)
        {
            ShipElement target = child.GetComponent<ShipElement>();

            if (target != null && target.isAvailable())
            {
                return target;
            }
        }
        return null;
    }

    /** REPAIR **/
    void repearCanon()
    {
        foreach (Transform child in this.transform)
        {
            Canon canon = child.GetComponent<Canon>();

            if (canon != null && !canon.isAvailable())
            {
                canon.doRepair();
            }
        }
    }
}
