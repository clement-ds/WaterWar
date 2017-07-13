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
            it.StartMyself();
        }
    }

    protected void createCrew()
    {
        List<CrewMember> members = (this.isPlayer ? PlayerManager.GetInstance().player.crew.crewMembers : PlayerManager.GetInstance().ai.crew.crewMembers);
        SimpleObjectPool crewPool = GameObject.Find("CrewPool").GetComponent<SimpleObjectPool>();

        foreach (CrewMember member in members)
        {
            GameObject crewMember = crewPool.GetObject();

            Battle_CrewMember battleCrewMember = crewMember.GetComponent<Battle_CrewMember>();
            battleCrewMember.initialize(member);
            crewMember.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(member.memberImage);

            ShipElement[] items;

            if (member.assignedRoom == Ship_Item.CANON)
            {
                items = this.GetComponentsInChildren<Canon>();
            }
            else if (member.assignedRoom == Ship_Item.CANTEEN)
            {
                items = this.GetComponentsInChildren<Canteen>();
            }
            else if (member.assignedRoom == Ship_Item.HELM)
            {
                items = this.GetComponentsInChildren<Helm>();
            }
            else
            {
                crewMember.transform.position = this.transform.position;
                crewMember.transform.SetParent(this.transform);
                break;
            }

            foreach (ShipElement it in items)
            {
                if (it.hasAvailableCrewMemberPosition())
                {
                    battleCrewMember.directAssignCrewMemberInElement(it);
                    break;
                }
            }
        }
    }

    /** DAMAGE **/
    public void receiveDamage(float damage) {
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

    /** ACTIONS **/
    public abstract void aboardingEnemy();

    public abstract void escape();

    public abstract void canEscape(bool value);

    public abstract void canAboarding(bool value);

    public abstract void die();

    /** GETTERS **/
    public float getCurrentLife() {
        return this.currentLife;
    }

    /** SETTERS **/
    public void setCurrentLife(float value) {
        this.currentLife = value;
        this.currentLife = (value < 0 ? 0 : value);
        this.currentLife = (this.currentLife > this.life ? this.life : this.currentLife);
        this.updateSliderValue();
    }

    public void applyCrewAttributes(Effect effect, float time, float value)
    {
    }
}
