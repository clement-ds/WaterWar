using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public abstract class Battle_Ship : MonoBehaviour
{
    public Slider slider = null;
    protected readonly float life;
    protected float currentLife;
    protected float speed;
    protected string id;

    protected bool aboarding;
    protected bool canEscapeAction;

    protected Vector3 movement;
    protected Ship_Direction direction;
    protected Ship_Direction saveCollisionDirection;
    protected float moveRotation;
    protected Rigidbody2D body;

    protected bool isPlayer;

    protected List<RoomElement> rooms;
    protected List<Battle_CrewMember> crewMembers;

    protected int weaponForCrew;
    protected int countDiedMember;

    protected Battle_Ship(float lifeValue, bool isPlayer)
    {
        this.direction = Ship_Direction.FRONT;
        this.saveCollisionDirection = Ship_Direction.NONE;
        this.moveRotation = 0;

        this.aboarding = false;
        this.canEscapeAction = false;
        this.isPlayer = isPlayer;

        this.speed = 50;
        this.life = lifeValue;

        this.setCurrentLife(this.life);
        this.weaponForCrew = 0;
        this.countDiedMember = 0;

        this.rooms = new List<RoomElement>();
        this.crewMembers = new List<Battle_CrewMember>();
    }

    // Use this for initialization
    void Start()
    {
    }

    public void init()
    {
        this.createRoom();
        this.createCrew();
    }

    /** CREATOR **/
    protected void createRoom()
    {
        ShipElement[] items = this.GetComponentsInChildren<ShipElement>();

        foreach (ShipElement it in items)
        {
            it.init();
        }
    }

    protected void createCrew()
    {
        List<CrewMember> members = PlayerManager.GetInstance().getCharacter(this.id).crew.crewMembers;
        SimpleObjectPool crewPool = GameObject.Find("CrewPool").GetComponent<SimpleObjectPool>();

        float powerWeaponValue = this.weaponForCrew / members.Count;

        foreach (CrewMember member in members)
        {
            GameObject crewMember = crewPool.GetObject();

            Battle_CrewMember battleCrewMember = crewMember.GetComponent<Battle_CrewMember>();
            battleCrewMember.initialize(member, this.id);
            battleCrewMember.getProfile().changePower(powerWeaponValue);
            crewMember.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(member.memberImage);

            if (!member.morale)
            {
                battleCrewMember.getProfile().addEffect(Effect.MORAL, -1, 50);
                battleCrewMember.getProfile().addEffect(Effect.SPEED, -1, 50);
                battleCrewMember.getProfile().addEffect(Effect.ENERGY, -1, 50);
            }
            List<RoomElement> items = this.parseShipElement(member.assignedRoom);

            if (this.assignateCrewMemberToRoom(battleCrewMember, items, true))
            {
                this.crewMembers.Add(battleCrewMember);
            }
        }
    }

    protected bool assignateCrewMemberToRoom(Battle_CrewMember crewMember, List<RoomElement> items, bool priority)
    {
        bool result = false;
        foreach (RoomElement room in items)
        {
            try
            {
                if (priority)
                {
                    if (room.getEquipment() && room.getEquipment().hasAvailableCrewMemberPosition())
                    {
                        Debug.Log("assign " + crewMember.getProfile().memberName);
                        if (crewMember.directAssignCrewMemberInElement(room.getEquipment(), room.getEquipment().chooseAvailableCrewMemberPosition(crewMember.GetInstanceID())))
                        {
                            result = true;
                            break;
                        }
                    }
                }
                else
                {
                    if (room.hasAvailableCrewMemberPosition())
                    {
                        if (crewMember.directAssignCrewMemberInRoom(room, room.chooseAvailableCrewMemberPosition(crewMember.GetInstanceID())))
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }
            catch (System.Exception m)
            {
                Debug.LogError(m.Message);
            }
        }
        if (result)
        {
            return true;
        }
        else if (priority && !result)
        {
            priority = false;
            return this.assignateCrewMemberToRoom(crewMember, items, priority);
        }
        else if (!result && items.Count != this.rooms.Count)
        {
            items = this.rooms;
            return this.assignateCrewMemberToRoom(crewMember, items, priority);
        }
        return false;
    }

    /** DAMAGE **/
    public void receiveDamage(float damage)
    {
        this.setCurrentLife(this.currentLife - damage);
        if (this.currentLife <= 0)
        {
            this.die(DestroyedStatus.DESTROY_SHIP);
        }
    }

    public void updateSliderValue()
    {
        if (slider)
        {
            slider.value = (currentLife * 100) / life;
        }
    }

    public void crewMemberDied(int id)
    {
        for (var i = 0; i < this.crewMembers.Count; ++i)
        {
            if (this.crewMembers[i].GetInstanceID() == id)
            {
                this.crewMembers.RemoveAt(i);
                ++this.countDiedMember;
                --i;
            }
        }
        if (this.countDiedMember >= 1)
        {
            foreach (var member in this.crewMembers)
            {
                member.getProfile().addEffect(Effect.MORAL, 60, 80);
            }
            this.countDiedMember = 0;
        }
        if (this.crewMembers.Count == 0)
        {
            this.die(DestroyedStatus.KILL_MEMBERS);
        }
    }
    
    /** COLLISION **/

   public void distanceWith(Battle_Ship target, float distance)
    {
        if (distance < 2.5 && !this.aboarding)
        {
            this.onAboardTarget(target);
        }
        else if (distance >= 2.5 && this.aboarding)
        {
            this.leaveAboardTarget(target);
        }
        if (distance <= 1.5)
        {
            this.movement = new Vector3(0, 0, 0);
        }
    }

    void onAboardTarget(Battle_Ship enemy)
    {
        if (enemy)
        {
            this.saveCollisionDirection = this.direction;
            this.doAboarding(enemy, true);
        }
    }

    void leaveAboardTarget(Battle_Ship enemy)
    {
        if (enemy)
        {
            this.moveRotation = Direction_Value.values[this.saveCollisionDirection];
            this.saveCollisionDirection = Ship_Direction.NONE;
            this.doAboarding(enemy, false);
        }
    }

    /** MOVEMENT **/
    public void changeDirection(Ship_Direction direction)
    {
        this.direction = direction;
        if (this.direction == this.saveCollisionDirection)
            return;
        this.saveCollisionDirection = Ship_Direction.NONE;
        if (direction == Ship_Direction.FRONT)
        {
            this.movement = new Vector3(0, 0, 0);
            this.moveRotation = Direction_Value.values[direction];
        }
        else if (direction == Ship_Direction.RIGHT)
        {
            this.movement = new Vector3(this.speed / 100, 0, 0);
            this.moveRotation = Direction_Value.values[direction];
        }
        else if (direction == Ship_Direction.LEFT)
        {
            this.movement = new Vector3(-1 * (this.speed / 100), 0, 0);
            this.moveRotation = Direction_Value.values[direction];
        }
    }

    /** UPDATE **/
    public void FixedUpdate()
    {
        if (!this.body)
        {
            this.body = this.GetComponent<Rigidbody2D>();
        }
        this.body.velocity = this.movement;
        if (this.moveRotation != 0)
        {
            if (this.body.rotation < this.moveRotation)
            {
                float angle = this.body.rotation + 3 * Time.fixedDeltaTime;
                this.body.MoveRotation((angle > this.moveRotation ? this.moveRotation : angle));
            }
            else if (this.body.rotation > this.moveRotation)
            {
                float angle = this.body.rotation - 3 * Time.fixedDeltaTime;
                this.body.MoveRotation((angle < this.moveRotation ? this.moveRotation : angle));
            }
        }
    }

    public void updateActionMenu()
    {
        foreach (RoomElement room in this.rooms)
        {
            room.updateActionMenu();
        }
    }

    /** ACTIONS **/
    public abstract void aboardingEnemy();

    public abstract void escape();

    public abstract void canEscape(bool value);

    public void doAboarding(Battle_Ship target, bool aboarding)
    {
        Debug.LogError(this + " can[" + aboarding + "] aboard " + target);
        this.aboarding = aboarding;
        if (aboarding)
        {
            var i2 = 0;
            for (var i = 0; i < this.rooms.Count; ++i)
            {
                if (this.rooms[i].getEquipment() != null && this.rooms[i].getEquipment().getType() == Ship_Item.CANON && ((Canon)this.rooms[i].getEquipment()).isInGoodPositionToShoot(target))
                {
                    while (i2 < target.getRooms().Count)
                    {
                        if (target.getRooms()[i2].getEquipment() != null && target.getRooms()[i2].getEquipment().getType() == Ship_Item.CANON && ((Canon)target.getRooms()[i2].getEquipment()).isInGoodPositionToShoot(this))
                        {
                            Debug.LogError("LINK : " + this.rooms[i].getId() + " + " + target.getRooms()[i2].getId());
                            this.rooms[i].addLink(target.getRooms()[i2].getId());
                            ++i2;
                            break;
                        }
                        ++i2;
                    }
                }
            }
        }
        else
        {
            Debug.LogError("UNLINK");
            foreach (var room in this.rooms)
            {
                room.purgeExternLinks(this.getId());
            }
        }
        GameRulesManager.GetInstance().guiAccess.boardingButton.gameObject.SetActive(aboarding);
    }

    public abstract void die(DestroyedStatus status);

    public void addShipElement(RoomElement shipElement)
    {
        this.rooms.Add(shipElement);
    }

    /** GETTERS **/
    public bool isDied()
    {
        return this.getCurrentLife() <= 0;
    }

    public List<RoomElement> parseShipElement(Ship_Item type)
    {
        List<RoomElement> result = new List<RoomElement>();

        foreach (RoomElement item in this.rooms)
        {
            try
            {
                if (item.getEquipment().getType() == type)
                {
                    result.Add(item);
                }
            }
            catch
            {
            }
        }
        return result;
    }
    public float getCurrentLife()
    {
        return this.currentLife;
    }

    public List<RoomElement> getRooms()
    {
        return this.rooms;
    }

    public RoomElement getFreeElement(Ship_Item type)
    {
        RoomElement result = null;
        foreach (RoomElement item in this.rooms)
        {
            if (item.getEquipment().getType() == type && item.getEquipment().getMember() == null)
            {
                return item;
            }
        }
        return result;
    }

    public string getId()
    {
        return this.id;
    }

    /** SETTERS **/
    protected void setCurrentLife(float value)
    {
        this.currentLife = value;
        this.currentLife = (value < 0 ? 0 : value);
        this.currentLife = (this.currentLife > this.life ? this.life : this.currentLife);
        this.updateSliderValue();
    }

    public void setId(string id)
    {
        this.id = id;
    }

    public void applyCrewAttributes(Effect effect, float time, float value)
    {
    }

    public void setWeaponForCrew(int value)
    {
        this.weaponForCrew = value;
    }
}
