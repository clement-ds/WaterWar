using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Canon : ShipElement
{
    private RoomElement target = null;
    private bool ready = false;
    private bool reloading = false;
    private int power = 150;       // Random number
    private float damage = 5;      // Random number
    private int viewFinder = 0;  // Random number
    private ShotCutscene shotCutscene;
    private SimpleObjectPool canonBallPool;
    private bool selectingTarget;

    protected float baseDamage = 20;
    protected float baseCooldown = 3;
    protected float basePowerShootSpeed = 8;


    public Canon() : base(50, Ship_Item.CANON)
    {
    }

    public override void init()
    {
        Vector3 parentPos = this.transform.parent.transform.localPosition;
        this.transform.localRotation = Quaternion.Euler(0, 0, (parentPos.y < 0 ? -90 : 90));
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + (parentPos.y > 0 ? 0.6f : -0.6f), this.transform.localPosition.z);

        shotCutscene = GameObject.Find("CutsceneManager").GetComponent<ShotCutscene>();
        canonBallPool = GameObject.Find("CanonBallPool").GetComponent<SimpleObjectPool>();
    }

    public override void reInitValues()
    {
        this.reloading = false;
        this.attacking = false;
    }

    public void destroyCanon()
    {
        // Destroy all Components
        if (GetComponent<SpriteRenderer>())
            Destroy(GetComponent<SpriteRenderer>());
    }

    /** EFFECT **/


    /** GUI CREATOR **/
    public override List<ActionMenuItem> createActionList()
    {
        List<ActionMenuItem> actions = new List<ActionMenuItem>();
        if (this.isWorking())
        {
            if (this.isAvailable() && this.getMember() && this.getTarget() && !this.attacking && !this.reloading)
                actions.Add(new ActionMenuItem("Attack", doDamage));
            if (this.attacking && this.canAttack)
                actions.Add(new ActionMenuItem("Stop Attack", stopAttack));
            if (this.reloading)
                actions.Add(new ActionMenuItem("Reloading..", none));
            if (this.getMember() && !ready && !this.reloading)
                actions.Add(new ActionMenuItem("Load canon", doReload));
            actions.Add(new ActionMenuItem("Select Target", selectTarget));
        }
        this.addGeneralActionsTo(actions);
        return actions;
    }

    /** AVAILABLE POSITION CREATOR **/
    public override void createAvailableCrewMemberPosition()
    {
        this.availablePosition = new AvailablePosition(new Vector3(-0.55f, 0f, -1f));
    }

    /** ON HIT EFFECT **/
    protected override void dealDamageAsRepercution(Battle_CanonBall canonBall)
    {
    }

    protected override void dealDamageOnDestroy()
    {
        this.GetComponentInParent<Battle_Ship>().receiveDamage(20);
    }

    protected override void applyMalusOnHit(Battle_CanonBall canonBall)
    {

    }

    protected override void applyMalusOnDestroy()
    {

    }

    /** ACTIONS **/
    public bool none()
    {
        return false;
    }

    public override bool actionIsRunning()
    {
        if (this.reloading)
        {
            return true;
        }
        return false;
    }

    public override bool actionStopRunning()
    {
        if (this.isAttacking())
        {
            return this.stopAttack();
        }
        return false;
    }

    protected bool stopAttack()
    {
        this.canAttack = false;
        this.cancelEveryTask();
        this.updateParentActionMenu();
        return true;
    }

    protected bool selectTarget()
    {
        MouseManager.getInstance().setCursor(ECursor.SEARCH_TARGET);
        this.selectingTarget = true;
        return true;
    }

    /** REALOAD **/
    protected bool isPossibleToReload()
    {
        Gunpowder powder = this.transform.root.GetComponentInChildren<Gunpowder>();
        return (powder != null && powder.isAvailable() && powder.isWorking());
    }

    protected bool doReload()
    {
        if (!this.isPossibleToReload())
            return false;
        this.reloading = true;
        this.updateParentActionMenu();
        Invoke("setCanonReady", this.getMember().getProfile().getValueByCrewSkill(SkillAttribute.RCanonTime, this.baseCooldown));
        return true;
    }

    protected void reloadCanon()
    {
        if (!this.isPossibleToReload())
            return;
        this.reloading = true;
        Invoke("reloadEnd", this.getMember().getProfile().getValueByCrewSkill(SkillAttribute.RCanonTime, this.baseCooldown));
    }

    protected void reloadEnd()
    {
        this.setCanonReady();
        if (this.isWorking())
        {
            this.doDamage();
        }
    }

    /** DO DAMAGE **/
    protected override bool doDamageAction()
    {
        bool result = false;

        if (target != null)
        {
            if (ready)
            {
                MonoBehaviour finalTarget;
                Battle_Ship enemy = target.GetComponentInParent<Battle_Ship>();

                if (target.getEquipment() != null && target.getEquipment().isAvailable())
                {
                    finalTarget = target.getEquipment();
                }
                else
                {
                    finalTarget = enemy;
                }
                if (enemy.isDied())
                {
                    this.stopAttack();
                    this.target = null;
                    result = false;
                }
                /*if (UnityEngine.Random.value > 0.80)
                {
                    shotCutscene.StartCutscene();
                    WaitForX(shotCutscene.duration);
                }*/

                GameObject canonBall = canonBallPool.GetObject();

                Battle_CanonBall battleCanonBall = canonBall.GetComponent<Battle_CanonBall>();
                battleCanonBall.initialize(new CanonBall(this.baseDamage, this.GetComponentInChildren<Battle_CrewMember>().getProfile().getValueByCrewSkill(SkillAttribute.ShootCanonValue, 1)), finalTarget, this.getBulletAccuracy(this.GetComponentInChildren<Battle_CrewMember>()));
                canonBall.transform.position = this.transform.position;
                canonBall.transform.SetParent(this.transform);
                canonBall.GetComponent<Rigidbody2D>().AddRelativeForce((target.transform.position - canonBall.transform.position).normalized * this.getBulletSpeed(battleCanonBall.getAmmunition()));

                Physics2D.IgnoreCollision(canonBall.transform.GetComponent<Collider2D>(), this.transform.GetComponent<Collider2D>(), true);
                Physics2D.IgnoreCollision(canonBall.transform.GetComponent<Collider2D>(), this.getParentShip().transform.GetComponent<Collider2D>(), true);
                if (enemy != null && canAttack)
                {
                    this.setAttacking(true);
                    this.ready = false;
                    result = true;
                }
            }
            if (canAttack)
            {
                this.reloadCanon();
            }
            else
            {
                this.setAttacking(false);
            }
        }
        return result;
    }

    IEnumerator WaitForX(long x)
    {
        yield return new WaitForSeconds(x);
    }
    protected override void doDamageAnimation()
    {
        ParticleSystem canonShotExplosion = (ParticleSystem)transform.Find("CanonShotExplosion/PS_CanonShotExplosion").gameObject.GetComponent<ParticleSystem>();
        canonShotExplosion.Play();
    }

    private float getBulletSpeed(Ammunition ammunition)
    {
        return (this.power / ammunition.getWeight()) * this.basePowerShootSpeed;
    }

    private Vector3 getBulletAccuracy(Battle_CrewMember crew)
    {
        float x = 0; // SUCCESS
        float y; // MISS
        float z; // FAIL

        float distance = float.Parse(GameRulesManager.GetInstance().guiAccess.distanceToEnemy.text);
        if (distance > 20)
            x = 0.1f;
        else if (distance > 20)
            x = 0.1f;
        else if (distance > 15)
            x = 0.3f;
        else if (distance > 13)
            x = 0.5f;
        else if (distance > 10)
            x = 0.6f;
        else if (distance > 7)
            x = 0.7f;
        else if (distance > 3)
            x = 0.8f;
        else if (distance <= 3)
            x = 1f;

        y = ((1f - x) / 3) * 2;
        z = 1f - y - x;
        return new Vector3(x, y, z);
    }

    /** RECEIVE DAMAGE **/
    protected override bool receiveDamageAction(Battle_CanonBall canonBall)
    {
        this.setCurrentLife(this.currentLife - canonBall.getAmmunition().getDamage());
        return true;
    }

    protected override void receiveDamageAnimation()
    {
        ParticleSystem targetExplosion = transform.Find("BoatExplosion/PS_BoatExplosion").gameObject.GetComponent<ParticleSystem>();
        targetExplosion.Play();
    }

    /** GETTERS **/
    public override bool isWorking()
    {
        return this.isAvailable() && this.getPercentLife() > 20;
    }

    public string getName()
    {
        return this.name;
    }

    public int getPower()
    {
        return this.power;
    }

    public float getDamage()
    {
        return this.damage;
    }

    public int getViewFinder()
    {
        return this.viewFinder;
    }

    public bool isReloading()
    {
        return this.reloading;
    }

    public bool isAttacking()
    {
        return this.attacking;
    }

    public RoomElement getTarget()
    {
        return this.target;
    }

    public bool isSelectingTarget()
    {
        return this.selectingTarget;
    }

    public bool isInGoodPositionToShoot(RoomElement target)
    {
        bool canonIsRightPosition = this.transform.localPosition.y < 0;
        bool shipIsRightPosition = this.transform.root.transform.position.x < target.transform.root.transform.position.x;
        return (canonIsRightPosition && shipIsRightPosition || !canonIsRightPosition && !shipIsRightPosition);
    }

    public bool isInGoodPositionToShoot(Battle_Ship target)
    {
        bool canonIsRightPosition = this.transform.localPosition.y < 0;
        bool shipIsRightPosition = this.transform.root.transform.position.x < target.transform.position.x;
        return (canonIsRightPosition && shipIsRightPosition || !canonIsRightPosition && !shipIsRightPosition);
    }

    /** SETTERS **/
    public void setTarget(RoomElement target)
    {
        if (this.isInGoodPositionToShoot(target))
        {
            this.target = target;
            this.selectingTarget = false;
            this.updateParentActionMenu();
        }
    }

    protected void setCanonReady()
    {
        this.reloading = false;
        this.ready = true;
        this.updateParentActionMenu();
    }
}
