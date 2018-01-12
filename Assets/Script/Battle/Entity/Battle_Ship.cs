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

    protected bool canAboardingAction;
    protected bool canEscapeAction;

    protected Vector3 movement;
    protected Ship_Direction direction;
    protected Ship_Direction saveCollisionDirection;
    protected float moveRotation;
    protected Rigidbody2D body;

    protected bool isPlayer;

    protected List<RoomElement> rooms;
    protected List<Battle_CrewMember> crewMembers;

    protected Battle_Ship(float lifeValue, bool isPlayer)
    {
        this.direction = Ship_Direction.FRONT;
        this.saveCollisionDirection = Ship_Direction.NONE;
        this.moveRotation = 0;

        this.canAboardingAction = false;
        this.canEscapeAction = false;
        this.isPlayer = isPlayer;

        this.speed = 50;
        life = lifeValue;
        this.setCurrentLife(life);

        this.rooms = new List<RoomElement>();
        this.crewMembers = new List<Battle_CrewMember>();
    }

    // Use this for initialization
    void Start()
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
        List<CrewMember> members = (this.isPlayer ? PlayerManager.GetInstance().player.crew.crewMembers : PlayerManager.GetInstance().enemies[0].crew.crewMembers);
        SimpleObjectPool crewPool = GameObject.Find("CrewPool").GetComponent<SimpleObjectPool>();

        foreach (CrewMember member in members)
        {
            GameObject crewMember = crewPool.GetObject();

            Battle_CrewMember battleCrewMember = crewMember.GetComponent<Battle_CrewMember>();
            battleCrewMember.initialize(member);
            crewMember.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(member.memberImage);

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
            this.die();
        }
    }

    public void updateSliderValue()
    {
        if (slider)
        {
            slider.value = (currentLife * 100) / life;
        }
    }

    /** COLLISION **/
    void OnTriggerEnter2D(Collider2D col)
    {
        Battle_Ship enemy = col.gameObject.GetComponent<Battle_Ship>();
        if (enemy)
        {
            this.movement = new Vector3(0, 0, 0);
            enemy.moveRotation = this.moveRotation;
            this.saveCollisionDirection = this.direction;
            this.canAboarding(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        Battle_Ship enemy = col.gameObject.GetComponent<Battle_Ship>();
        if (enemy)
        {
            this.saveCollisionDirection = Ship_Direction.NONE;
            this.canAboarding(false);
        }
    }

    /** MOVEMENT **/
    public void changeDirection(Ship_Direction direction)
    {
        this.direction = direction;
        if (this.direction == this.saveCollisionDirection)
            return;
        if (direction == Ship_Direction.FRONT)
        {
            this.movement = new Vector3(0, 0, 0);
            this.moveRotation = 90;
        }
        else if (direction == Ship_Direction.RIGHT)
        {
            this.movement = new Vector3(this.speed / 100, 0, 0);
            this.moveRotation = 80;
        }
        else if (direction == Ship_Direction.LEFT)
        {
            this.movement = new Vector3(-1 * (this.speed / 100), 0, 0);
            this.moveRotation = 100;
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

    public abstract void canAboarding(bool value);

    public abstract void die();

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

    /** SETTERS **/
    protected void setCurrentLife(float value)
    {
        this.currentLife = value;
        this.currentLife = (value < 0 ? 0 : value);
        this.currentLife = (this.currentLife > this.life ? this.life : this.currentLife);
        this.updateSliderValue();
    }

    public void applyCrewAttributes(Effect effect, float time, float value)
    {
    }
}
