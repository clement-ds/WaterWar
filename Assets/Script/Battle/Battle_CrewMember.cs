using UnityEngine;
using System.Collections;
using System;

public class Battle_CrewMember : GuiElement
{

    ShipElement room = null;
    CrewMember member = null;

    // Use this for initialization
    protected override void StartMySelf()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }


    protected override void createActionList()
    {
    }

    /** PARENT MANAGER **/
    public void assignCrewMemberToShipElement(ShipElement target, GameObject container)
    {
        this.transform.SetParent(target.transform);
        this.transform.localPosition = target.chooseAvailableCrewMemberPosition(this.GetInstanceID());
        target.focus();
        target.updateActionMenu();
        foreach (Transform child in container.transform)
        {
            ShipElement target2 = child.GetComponent<ShipElement>();
            if (target2 != null && target.GetInstanceID() != target2.GetInstanceID())
            {
                target2.unfocus();
            }
        }
    }

    public void freeCrewMemberFromShipElement(ShipElement target, GameObject container)
    {
        target.freeCrewMemberPosition(this.GetInstanceID());
        this.transform.SetParent(container.transform);
        this.transform.localPosition = new Vector3(0, 0, 0);
    }

    /** GETTERS **/

    public string getId()
    {
        return this.member.getId();
    }
}
