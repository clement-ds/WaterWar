using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Battle_CrewMember : GuiElement
{

    ShipElement room = null;
    CrewMember member = null;
    ShipElement targetFocus = null;
    ShipElement parent = null;
    Vector3 finalMovePos;
    Boolean haveToMove = false;

    // Use this for initialization
    public override void StartMyself()
    {
    }

    public void initialize(CrewMember crewMember)
    {
        this.member = crewMember;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.haveToMove)
        {
            float step = member.getValueByCrewSkill(SkillAttribute.WalkValue, member.walkSpeed) * Time.deltaTime;
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

    public void stopMove()
    {
        this.arriveAtFinalPos();
    }

    private void arriveAtFinalPos()
    {
        this.haveToMove = false;
        if (this.targetFocus != null)
        {
            Debug.Log("Arrive at pos : " + finalMovePos);
            this.directAssignCrewMemberInElement(this.targetFocus);
            Debug.Log("final pos: " + this.transform.localPosition);
            this.targetFocus.focus();
            this.targetFocus.updateActionMenu();
            this.targetFocus = null;
        }
    }

    public void directAssignCrewMemberInElement(ShipElement element)
    {
        this.transform.SetParent(element.transform);
        this.transform.localPosition = element.chooseAvailableCrewMemberPosition(this.GetInstanceID());
    }

    /** PARENT MANAGER **/
    public void assignCrewMemberToShipElement(ShipElement target, GameObject container)
    {
        this.targetFocus = target;
        this.parent = target;
        this.moveTo(target.transform.position);
    }

    public void freeCrewMemberFromShipElement(ShipElement target, GameObject container)
    {
        if (target != null)
        {
            target.unfocus();
            target.freeCrewMemberPosition(this.GetInstanceID());
            this.parent = null;
            this.transform.SetParent(container.transform);
        }
    }

    public void freeCrewMemberFromParent(GameObject container)
    {
        this.freeCrewMemberFromShipElement(this.transform.parent.GetComponent<ShipElement>(), container);
    }

    public CrewMember getMember()
    {
        return this.member;
    }

    /** GETTERS **/

    public List<ActionMenuItem> getParentActionList()
    {
        Debug.Log("Partent : " + (parent == null ? -1 : this.parent.getActionList().Count));
        if (this.parent == null)
        {
            return new List<ActionMenuItem>();
        }
        return this.parent.getActionList();
    }
}
