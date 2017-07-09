using UnityEngine;
using System.Collections;
using System;

public class Battle_CrewMember : GuiElement
{

    ShipElement room = null;
    CrewMember member = null;
    ShipElement targetFocus = null;
    Vector3 finalMovePos;
    Boolean haveToMove = false;

    // Use this for initialization
    protected override void StartMySelf()
    {
        member = new CrewMember_Captain("cquoica");
    }

    // Update is called once per frame
    void Update()
    {
        if (this.haveToMove)
        {
            float step = member.walkSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, this.finalMovePos, step);
            if (transform.position == this.finalMovePos)
            {
                this.arriveAtFinalPos();
            }
        }
    }


    protected override void createActionList()
    {
    }

    /** ACTIONS **/

    public void moveTo(Vector3 touchPos)
    {
        this.finalMovePos = touchPos;
        this.haveToMove = true;
    }

    private void arriveAtFinalPos()
    {
        this.haveToMove = false;
        if (this.targetFocus != null)
        {
            this.transform.SetParent(this.targetFocus.transform);
            this.transform.localPosition = this.targetFocus.chooseAvailableCrewMemberPosition(this.GetInstanceID());
            this.targetFocus.focus();
            this.targetFocus.updateActionMenu();
            this.targetFocus = null;
        }
    }

    /** PARENT MANAGER **/
    public void assignCrewMemberToShipElement(ShipElement target, GameObject container)
    {
        this.targetFocus = target;
        this.moveTo(target.transform.position);
    }

    public void freeCrewMemberFromShipElement(ShipElement target, GameObject container)
    {
        if (target != null)
        {
            target.unfocus();
            target.freeCrewMemberPosition(this.GetInstanceID());
            this.transform.SetParent(container.transform);
        }
    }

    public void freeCrewMemberFromParent(GameObject container)
    {
        this.freeCrewMemberFromShipElement(this.transform.parent.GetComponent<ShipElement>(), container);
    }

}
