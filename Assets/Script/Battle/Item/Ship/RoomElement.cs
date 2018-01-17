using UnityEngine;
using System.Collections.Generic;

public class RoomElement : GuiElement
{
    private ShipElement equipment;
    private List<Battle_CrewMember> members;

    private string id;
    private List<string> links;
    private List<AvailablePosition> availablePositions;

    public RoomElement()
    {
        this.availablePositions = new List<AvailablePosition>();
        this.members = new List<Battle_CrewMember>();
    }

    public void init(string shipId, string roomId, List<string> links)
    {
        this.id = shipId + roomId;
        this.links = links;

        for (var i = 0; i < this.links.Count; ++i)
        {
            this.links[i] = shipId + this.links[i];
        }
    }

    /** INIT **/
    public override void StartMyself()
    {
        this.equipment = this.transform.GetComponentInChildren<ShipElement>();
        createAvailableCrewMemberPosition();
    }

    protected override void createActionList()
    {
        if (this.equipment != null)
            this.actionList = this.equipment.createActionList();
    }

    /** UPDATE **/
    void Update()
    {
        mouseIsHover();
        updateHover();
    }

    /** COLLISION **/
    void OnTriggerEnter(Collider col)
    {
        Battle_CanonBall canonBall = col.gameObject.GetComponent<Battle_CanonBall>();
        if (canonBall)
        {
            float value = UnityEngine.Random.value;
            print(this + " (" + this.GetInstanceID() + ")  :   " + canonBall.getTarget().GetInstanceID() + "  --> " + canonBall.getHitStatus());
            if ((canonBall.getHitStatus() == HitStatus.HIT && canonBall.getTarget().GetInstanceID() == this.GetInstanceID())
                || (canonBall.getHitStatus() == HitStatus.FAIL && canonBall.getTarget().GetInstanceID() != this.GetInstanceID()))
            {
                Destroy(col.gameObject);
            }
        }
    }

    /** AVAILABLE POSITION **/
    public bool hasAvailableCrewMemberPosition()
    {
        for (int i = 0; i < this.availablePositions.Count; ++i)
        {
            if (this.availablePositions[i].available)
            {
                return true;
            }
        }
        return false;
    }

    public Vector3 chooseAvailableCrewMemberPosition(int id)
    {
        for (int i = 0; i < this.availablePositions.Count; ++i)
        {
            if (this.availablePositions[i].crewId == id)
            {
                return this.availablePositions[i].position;
            }
        }
        for (int i = 0; i < this.availablePositions.Count; ++i)
        {
            if (this.availablePositions[i].available)
            {
                this.availablePositions[i].available = false;
                this.availablePositions[i].crewId = id;
                return this.availablePositions[i].position;
            }
        }
        throw new System.Exception("no position available");
    }

    public void freeCrewMemberPosition(int id)
    {
        for (int i = 0; i < this.members.Count; ++i)
        {
            if (this.members[i].GetInstanceID() == id)
            {
                this.members.RemoveAt(i);
                break;
            }
        }
        for (int i = 0; i < this.availablePositions.Count; ++i)
        {
            if (this.availablePositions[i].crewId == id)
            {
                this.availablePositions[i].available = true;
                break;
            }
        }
    }

    protected void createAvailableCrewMemberPosition()
    {
        if (this.equipment != null)
        {
            this.equipment.createAvailableCrewMemberPosition();

            if (!(this.equipment.transform.localPosition.x < 0))
            {
                this.availablePositions.Add(new AvailablePosition(new Vector3(-0.5f, 0, -1)));
            }
            if (!(this.equipment.transform.localPosition.x > 0))
            {
                this.availablePositions.Add(new AvailablePosition(new Vector3(0.5f, 0, -1)));
            }
            if (!(this.equipment.transform.localPosition.y < 0))
            {
                this.availablePositions.Add(new AvailablePosition(new Vector3(0, -0.5f, -1)));
            }
            if (!(this.equipment.transform.localPosition.y > 0))
            {
                this.availablePositions.Add(new AvailablePosition(new Vector3(0, 0.5f, -1)));
            }
        }
        else
        {
            this.availablePositions.Add(new AvailablePosition(new Vector3(-0.5f, 0, 0)));
            this.availablePositions.Add(new AvailablePosition(new Vector3(0.5f, 0, 0)));
            this.availablePositions.Add(new AvailablePosition(new Vector3(0, -0.5f, 0)));
            this.availablePositions.Add(new AvailablePosition(new Vector3(0, 0.5f, 0)));
        }
    }

    /** ACTION **/
    public bool actionIsRunning()
    {
        if (this.equipment != null)
            return this.equipment.actionIsRunning();
        return false;
    }

    public bool actionStopRunning()
    {
        if (this.equipment != null)
            return this.equipment.actionStopRunning();
        return false;
    }

    /** HOVER **/
    private bool mouseIsHover()
    {
        if (this.equipment != null)
            return this.equipment.mouseIsHover();
        return false;
    }

    private void updateHover()
    {
        if (this.equipment != null)
            this.equipment.updateHover();
    }

    void OnMouseEnter()
    {
        if (this.equipment != null)
            this.equipment.showSlider();
    }

    void OnMouseExit()
    {
        if (this.equipment != null)
            this.equipment.hideSlider();
    }

    /** MEMBERSHIP **/
    public bool moveFromEquipmentToRoom(Battle_CrewMember member)
    {
        try
        {
            Vector3 pos = this.chooseAvailableCrewMemberPosition(member.GetInstanceID());
            this.directAddMember(member);

            member.moveTo(this, new List<Vector3>() { pos });
            return true;
        }
        catch
        {
            return false;
        }
    }

    public void directAddMember(Battle_CrewMember member)
    {
        if (this.getMember(member.GetInstanceID()) == null)
        {
            this.members.Add(member);
        }
    }

    public void moveMemberToRoom(Battle_CrewMember member)
    {
        Debug.Log("move " + member.GetType() + " [" + member.getRoom() + ", " + member.getEquipment() + "]" + " to " + this.id);
        if (this.equipment && this.equipment.hasAvailableCrewMemberPosition())
        {
            Vector3 pos = this.equipment.chooseAvailableCrewMemberPosition(member.GetInstanceID());

            List<Vector3> path = RoomUtils.getRoute(member.getRoom(), this);
            path.Add(pos);
            member.moveTo(this.equipment, path);
        }
        else
        {
            foreach (AvailablePosition pos in availablePositions)
            {
                if (pos.available)
                {
                    pos.available = false;
                    pos.crewId = member.GetInstanceID();

                    List<Vector3> path = RoomUtils.getRoute(member.getRoom(), this);
                    path.Add(pos.position);
                    member.moveTo(this, path);
                    break;
                }
            }
        }
    }

    /** GETTERS **/
    public Battle_CrewMember getMember(int id)
    {
        foreach (Battle_CrewMember member in this.members)
        {
            if (member.GetInstanceID() == id)
            {
                return member;
            }
        }
        return null;
    }

    public List<Battle_CrewMember> getMembers()
    {
        return this.members;
    }

    public ShipElement getEquipment()
    {
        return this.equipment;
    }

    public List<string> getLinks()
    {
        return this.links;
    }

    public string getId()
    {
        return this.id;
    }

    /** MODIFIER **/
    public void addLink(string id)
    {
        this.links.Add(id);
    }

    public void purgeExternLinks(string id)
    {
        for (var i = 0; i < this.links.Count; ++i)
        {
            if (!this.links[i].Contains(id))
            {
                this.links.RemoveAt(i);
                --i;
            }
        }
    }

    public override string getIdentifier()
    {
        return this.getId() + "";
    }
}