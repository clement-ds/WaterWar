using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanonPanelGuiManager : MonoBehaviour
{

    private List<Canon> canons = new List<Canon>();

    public List<Button> buttons;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void initCanons(Battle_Ship p, Battle_Ship target)
    {
        foreach (var room in p.getRooms())
        {
            if (room.getEquipment() != null && room.getEquipment().getType() == Ship_Item.CANON && ((Canon)room.getEquipment()).isInGoodPositionToShoot(target))
            {
                this.addCanon((Canon)room.getEquipment());
            }
        }
    }

    /** ACTIONS **/
    public void attack()
    {
        bool check = false;
        foreach (var canon in this.canons)
        {
            if (canon.doDamage())
            {
                check = true;
            }
        }
        //Debug.Log("check : " + check);
        if (check)
        {
            this.buttons[0].gameObject.SetActive(false);
            this.buttons[1].gameObject.SetActive(true);
        }
    }

    public void stopAttack()
    {
        this.buttons[0].gameObject.SetActive(true);
        this.buttons[1].gameObject.SetActive(false);
        foreach (var canon in this.canons)
        {
            canon.actionStopRunning();
        }
    }

    public void selectTarget()
    {
        foreach (var canon in this.canons)
        {
            canon.selectTarget();
        }
    }

    public void callCrew()
    {
        foreach (var canon in this.canons)
        {
            canon.callBestCrewMember();
        }
    }

    /** MODIFIER **/
    public void addCanon(Canon canon)
    {
        this.canons.Add(canon);
    }
}
