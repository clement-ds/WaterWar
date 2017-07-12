using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class Battle_Ship : MonoBehaviour
{
    protected GuiAccess guiAccess;

    public Slider slider = null;
    protected readonly int life;
    protected int currentLife;
    protected float speed;

    protected bool canAboardingAction;
    protected bool canEscapeAction;

    protected Vector3 movement;
    protected Ship_Direction direction;
    protected Ship_Direction saveCollisionDirection;
    protected float moveRotation;
    protected Rigidbody2D body;

    protected Battle_Ship(int lifeValue)
    {
        this.direction = Ship_Direction.FRONT;
        this.saveCollisionDirection = Ship_Direction.NONE;
        this.moveRotation = 0;

        this.canAboardingAction = false;
        this.canEscapeAction = false;
        
        this.speed = 50;
        life = lifeValue;
        this.setCurrentLife(life);
    }

    // Use this for initialization
    void Start()
    {
        this.guiAccess = GameObject.Find("Battle_UI").GetComponent<GuiAccess>();
        this.createCrew();
    }

    public void updateSliderValue() {
        if (slider)
        {
            slider.value = (currentLife * 100) / life;
        }
    }

    public void receiveDamage(int damage) {
        this.setCurrentLife(this.currentLife - damage);
        if (this.currentLife <= 0)
        {
            this.die();
        }
    }

    protected abstract void createCrew();

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
    public int getCurrentLife() {
        return this.currentLife;
    }

    /** SETTERS **/
    public void setCurrentLife(int value) {
        this.currentLife = value;
        this.currentLife = (value < 0 ? 0 : value);
        this.currentLife = (this.currentLife > this.life ? this.life : this.currentLife);
        this.updateSliderValue();
    }
}
