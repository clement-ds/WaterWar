using UnityEngine;
using System.Collections;
using System;

public class Canon : ShipElement
{
    private ShipElement target = null;
    private bool ready = false;
    private bool reloading = false;
    private int power = 150;       // Random number
    private float damage = 5;      // Random number
    private int viewFinder = 0;  // Random number
    private ShotCutscene shotCutscene;
    private SimpleObjectPool canonBallPool;


    public Canon() : base(50, Ship_Item.CANON)
    {
    }

    public override void StartMyself()
    {
        base.StartMyself();
        shotCutscene = GameObject.Find("CutsceneManager").GetComponent<ShotCutscene>();
        canonBallPool = GameObject.Find("CanonBallPool").GetComponent<SimpleObjectPool>();
    }

    public void destroyCanon()
    {
        // Destroy all Components
        if (GetComponent<SpriteRenderer>())
            Destroy(GetComponent<SpriteRenderer>());
    }

    /** EFFECT **/


    /** GUI CREATOR **/
    protected override void createActionList()
    {
        this.actionList.RemoveRange(0, this.actionList.Count);
        if (this.isAvailable() && this.getMember() && this.getTarget() && !this.attacking && !this.reloading)
            this.actionList.Add(new ActionMenuItem("Attack", doDamage));
        if (this.attacking && this.canAttack)
            this.actionList.Add(new ActionMenuItem("Stop Attack", stopAttack));
        if (this.reloading)
            this.actionList.Add(new ActionMenuItem("Reloading..", none));
        if (this.getMember() && !this.isRepairing() && this.currentLife != this.life)
            this.actionList.Add(new ActionMenuItem("Repair", doRepair));
        if (this.getMember() && !ready && !this.reloading)
            this.actionList.Add(new ActionMenuItem("Load canon", doReload));
    }

    /** AVAILABLE POSITION CREATOR **/
    protected override void createAvailableCrewMemberPosition()
    {
        this.availablePosition.Add(new AvailablePosition(new Vector3(0f, -0.6f, 0f)));
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
    public override bool actionIsRunning()
    {
        if (this.reloading || this.repairing)
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
        this.updateActionMenu();
        return true;
    }

    /** REALOAD **/
    protected bool isPossibleToReload()
    {
        Gunpowder powder = this.transform.parent.GetComponentInChildren<Gunpowder>();

        print("Powder: " + powder);
        return (powder != null && powder.isAvailable());
    }

    protected bool doReload()
    {
        if (!this.isPossibleToReload())
            return false;
        this.reloading = true;
        this.updateActionMenu();
        Invoke("setCanonReady", this.GetComponentInChildren<Battle_CrewMember>().getMember().getValueByCrewSkill(SkillAttribute.RCanonTime, 3));
        return true;
    }

    protected void reloadCanon()
    {
        if (!this.isPossibleToReload())
            return;
        this.reloading = true;
        Invoke("reloadEnd", this.GetComponentInChildren<Battle_CrewMember>().getMember().getValueByCrewSkill(SkillAttribute.RCanonTime, 3));
    }

    protected void reloadEnd()
    {
        this.setCanonReady();
        this.doDamage();
    }

    /** REPAIR **/
    protected override void doRepairActionEnd()
    {
        this.setCurrentLife(this.currentLife + this.GetComponentInChildren<Battle_CrewMember>().getMember().getValueByCrewSkill(SkillAttribute.RepairValue, 10));
    }

    protected override bool doRepairAction()
    {
        Invoke("doRepairEnd", this.GetComponentInChildren<Battle_CrewMember>().getMember().getValueByCrewSkill(SkillAttribute.RepairTime, 1));
        return true;
    }

    /** DO DAMAGE **/
    protected override bool doDamageAction()
    {
        bool result = false;

        if (target != null)
        {
            if (ready)
            {
                if (!target.isAvailable())
                {
                    this.stopAttack();
                    this.target = null;
                    result = false;
                }
                else
                {
                    if (UnityEngine.Random.value > 0.80)
                    {
                        shotCutscene.StartCutscene();
                        WaitForX(shotCutscene.duration);
                    }
                    Battle_Ship enemy = target.GetComponentInParent<Battle_Ship>();

                    GameObject canonBall = canonBallPool.GetObject();

                    Battle_CanonBall battleCanonBall = canonBall.GetComponent<Battle_CanonBall>();
                    battleCanonBall.initialize(new CanonBall(1, this.GetComponentInChildren<Battle_CrewMember>().getMember().getValueByCrewSkill(SkillAttribute.ShootCanonValue, 1)), target, this.getBulletAccuracy(this.GetComponentInChildren<Battle_CrewMember>()));
                    canonBall.transform.position = this.transform.position;
                    canonBall.transform.SetParent(this.transform);
                    canonBall.GetComponent<Rigidbody2D>().AddRelativeForce((target.transform.position - canonBall.transform.position).normalized * this.getBulletSpeed(battleCanonBall.getAmmunition()));

                    Physics2D.IgnoreCollision(canonBall.transform.GetComponent<Collider2D>(), this.transform.GetComponent<Collider2D>(), true);
                    if (enemy != null && canAttack)
                    {
                        this.setAttacking(true);
                        this.ready = false;
                        result = true;
                    }

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

    private int getBulletSpeed(Ammunition ammunition)
    {
        return (this.power / ammunition.getWeight()) * 8;
    }

    private Vector3 getBulletAccuracy(Battle_CrewMember crew)
    {
        float x = 0.7f;
        float y = 0.1f;
        float z = 0.2f;

        //GameRulesManager.GetInstance().guiAccess.distanceToEnemy;
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

    public ShipElement getTarget()
    {
        return this.target;
    }

    /** SETTERS **/
    public void setTarget(ShipElement target)
    {
        this.target = target;
        this.updateActionMenu();
    }

    protected void setCanonReady()
    {
        this.reloading = false;
        this.ready = true;
        this.updateActionMenu();
    }
}
