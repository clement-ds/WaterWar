using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Battle_CrewMember : GuiElement
{

    CrewMember profile = null;

    RoomElement room = null;
    ShipElement equipment = null;
    MonoBehaviour targetFocus = null;

    List<Vector3> finalMovePos;
    int indexMove;
    Boolean haveToMove = false;

    // Use this for initialization
    public override void StartMyself()
    {
    }

    public void initialize(CrewMember crewMember)
    {
        this.profile = crewMember;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.haveToMove)
        {
            float step = profile.getValueByCrewSkill(SkillAttribute.WalkValue, profile.walkSpeed) * Time.deltaTime;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, this.finalMovePos[indexMove], step);
            if (transform.localPosition == this.finalMovePos[indexMove])
            {
                if (indexMove == this.finalMovePos.Count - 2)
                {
                    this.crewMemberArrivedAtContainer();
                }
                else if (indexMove == this.finalMovePos.Count - 1)
                {
                    this.crewMemberArrivedAtFinalPos();
                }
                ++indexMove;
            }
        }
    }

    protected override void createActionList()
    {
    }

    /** ACTIONS **/

    public void moveTo(MonoBehaviour target, List<Vector3> pos)
    {
        this.targetFocus = target;
        this.finalMovePos = pos;
        this.indexMove = 0;
        this.haveToMove = true;
    }

    private void crewMemberArrivedAtFinalPos()
    {
        this.haveToMove = false;
        Debug.Log("arrived at final pos : " + this.targetFocus);
        if (this.targetFocus != null)
        {
            Debug.Log("Arrive at pos : " + finalMovePos);
            this.transform.localPosition = finalMovePos[finalMovePos.Count - 1];
            Debug.Log("final pos: " + this.transform.localPosition);
            this.targetFocus = null;
        }
    }

    protected void crewMemberArrivedAtContainer()
    {
        if (this.targetFocus.GetType() == typeof(RoomElement))
        {
            //Debug.Log("its a room");
            this.changeParents((RoomElement)this.targetFocus, ((RoomElement)this.targetFocus).getEquipment());
            this.room.directAddMember(this);
        }
        else if (this.targetFocus.GetType() == typeof(ShipElement) || this.targetFocus.GetType().IsSubclassOf(typeof(ShipElement)))
        {
            //Debug.Log("its a equipment");
            this.changeParents(((ShipElement)this.targetFocus).getParentRoom(), (ShipElement)this.targetFocus);
            this.room.directAddMember(this);
        }
        this.transform.SetParent(this.targetFocus.transform);
    }

    public bool directAssignCrewMemberInRoom(RoomElement element, Vector3 pos)
    {
        this.changeParents(element, element.getEquipment());
        this.room.directAddMember(this);
        this.transform.SetParent(element.transform);
        this.transform.localPosition = pos;
        //Debug.Log("create member with " + this.room + ", " + this.equipment);
        return true;
    }

    public bool directAssignCrewMemberInElement(ShipElement element, Vector3 pos)
    {
        this.changeParents(element.getParentRoom(), element);
        this.transform.SetParent(element.transform);
        this.transform.localPosition = pos;
        //Debug.Log("create member with " + this.room + ", " + this.equipment);
        return true;
    }

    public void repair()
    {
        if (this.equipment != null)
        {
            this.equipment.repair(this.profile.getValueByCrewSkill(SkillAttribute.RepairValue, 40));
            //Invoke("repair", this.profile.getValueByCrewSkill(SkillAttribute.RepairTime, 1));
        }
    }

    /** PARENT MANAGER **/
    public void assignCrewMemberToRoom(RoomElement target)
    {
        this.transform.parent = target.transform.parent;

        target.moveMemberToRoom(this);

        if (this.equipment != null)
        {
            this.equipment.freeCrewMemberPosition(this.GetInstanceID());
            this.equipment = null;
        }
        if (this.room != null)
        {
            this.room.freeCrewMemberPosition(this.GetInstanceID());
            this.room = null;
        }
    }

    public bool freeCrewMemberFromShipElement()
    {
        if (this.equipment == null)
            return true;
        if (this.equipment.getParentRoom().hasAvailableCrewMemberPosition())
        {
            this.equipment.freeCrewMemberPosition(this.GetInstanceID());
            this.equipment = null;

            this.transform.parent = this.room.transform;

            this.room.moveFromEquipmentToRoom(this);
            this.targetFocus = this.room;
            return true;
        }
        return false;
    }

    public void freeCrewMemberFromParent()
    {
        this.freeCrewMemberFromShipElement();
    }

    public CrewMember getProfile()
    {
        return this.profile;
    }

    /** GETTERS **/

    public List<ActionMenuItem> getParentActionList()
    {
        if (this.room == null)
        {
            return new List<ActionMenuItem>();
        }
        return this.room.getActionList();
    }

    public RoomElement getRoom()
    {
        return this.room;
    }

    public ShipElement getEquipment()
    {
        return this.equipment;
    }

    /** SETTERS **/
    public void changeParents(RoomElement room, ShipElement equipment)
    {
        this.room = room;
        this.equipment = equipment;
    }
}
