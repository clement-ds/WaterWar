using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Battle_CrewMember : GuiElement
{

    CrewMember profile = null;
    string teamId;

    RoomElement room = null;
    ShipElement equipment = null;
    MonoBehaviour targetFocus = null;

    List<Vector3> finalMovePos;
    int indexMove;
    bool moving = false;
    bool alive = true;

    // Use this for initialization
    public override void StartMyself()
    {
    }

    public void initialize(CrewMember crewMember, string teamId)
    {
        this.profile = crewMember;
        this.profile.purgeEffects();
        this.teamId = teamId;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.moving)
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

    /** STATUS **/

    public void die()
    {
        this.alive = false;
        if (this.room)
        {
            this.room.freeCrewMemberPosition(this.GetInstanceID());
        }
        if (this.equipment)
        {
            this.equipment.freeCrewMemberPosition(this.GetInstanceID());
        }
        GameRulesManager.GetInstance().getShip(this.getTeamId()).crewMemberDied(this.GetInstanceID());
        this.stopPendingActions();
        Destroy(gameObject);
    }

    /** ACTIONS **/
    protected void checkAttackInRoom()
    {
        if (this.room)
        {
            List<Battle_CrewMember> enemies = new List<Battle_CrewMember>();

            foreach (Battle_CrewMember member in this.room.getMembers())
            {
                if (member.getTeamId() != this.getTeamId())
                {
                    enemies.Add(member);
                }
            }

            if (enemies.Count != 0)
            {
                int target = Random.Range(0, enemies.Count - 1);

                this.attackOtherCrewMember(enemies[target]);
            }
            this.launchAttackInRoom();
        }
    }

    public void launchAttackInRoom()
    {
        if (this.profile != null && this.profile.isAvailable() && this.room)
            Invoke("checkAttackInRoom", this.profile.getValueByCrewSkill(SkillAttribute.AttackTime, 2f));
    }

    protected void attackOtherCrewMember(Battle_CrewMember target)
    {
        Debug.Log("Attack another crew : ", target);
        this.profile.doDamage(this, target);

    }
    public void repairEquipment()
    {
        if (this.equipment != null)
        {
            this.equipment.repair(this.profile.getValueByCrewSkill(SkillAttribute.RepairValue, 40));
            this.launchRepairEquipment();
        }
    }

    public void launchRepairEquipment()
    {
        if (this.equipment && this.equipment.getParentShip().getId() == this.teamId)
            Invoke("repairEquipment", this.profile.getValueByCrewSkill(SkillAttribute.RepairTime, 1));
    }

    public void stopPendingActions()
    {
        CancelInvoke();
    }

    /** MOVING **/
    public void moveTo(MonoBehaviour target, List<Vector3> pos)
    {
        this.targetFocus = target;
        this.finalMovePos = pos;
        this.indexMove = 0;
        this.moving = true;
    }

    private void crewMemberArrivedAtFinalPos()
    {
        this.moving = false;
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
        this.transform.parent = this.targetFocus.transform;
        if (this.targetFocus.GetType() == typeof(RoomElement))
        {
            //Debug.Log("its a room");
            this.changeParents((RoomElement)this.targetFocus, ((RoomElement)this.targetFocus).getEquipment());
            this.room.directAddMember(this);
            this.launchRepairEquipment();
        }
        else if (this.targetFocus.GetType() == typeof(ShipElement) || this.targetFocus.GetType().IsSubclassOf(typeof(ShipElement)))
        {
            //Debug.Log("its a equipment");
            this.changeParents(((ShipElement)this.targetFocus).getParentRoom(), (ShipElement)this.targetFocus);
            this.room.directAddMember(this);
            if (this.equipment.getType() == Ship_Item.INFIRMARY)
            {
                ((Infirmary)this.equipment).launchHealCrew();
            }
        }
        this.launchAttackInRoom();
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
        this.room.directAddMember(this);
        this.transform.SetParent(element.transform);
        this.transform.localPosition = pos;
        return true;
    }

    /** PARENT MANAGER **/
    public bool assignCrewMemberToRoom(RoomElement target)
    {
        if (!RoomUtils.hasRoute(this.room, target))
        {
            return false;
        }
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
        return true;
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

    public bool isAlive()
    {
        return this.alive;
    }

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

    public bool isMoving()
    {
        return this.moving;
    }

    public string getTeamId()
    {
        return this.teamId;
    }

    /** SETTERS **/
    public void changeParents(RoomElement room, ShipElement equipment)
    {
        this.room = room;
        this.equipment = equipment;
    }
}
