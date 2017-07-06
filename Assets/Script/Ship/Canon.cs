using UnityEngine;
using System.Collections;
using System;

public class Canon : ShipElement
{
    private ShipElement target = null;
    private bool ready = false;
    private bool reloading = false;
    private int power = 5;       // Random number
    private int damage = 5;      // Random number
    private int viewFinder = 0;  // Random number
    private ShotCutscene shotCutscene;


    public Canon() : base(50)
    {
    }

    protected override void StartMySelf()
    {
        base.StartMySelf();
        shotCutscene = GameObject.Find("CutsceneManager").GetComponent<ShotCutscene>();

    }

    public void destroyCanon()
    {
        // Destroy all Components
        if (GetComponent<SpriteRenderer>())
            Destroy(GetComponent<SpriteRenderer>());
    }

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
    protected override void dealDamageAsRepercution(int damage)
    {
    }

    protected override void dealDamageOnDestroy()
    {
        this.GetComponentInParent<Battle_Ship>().receiveDamage(20);
    }

    protected override void applyMalusOnHit()
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
    protected bool doReload()
    {
        this.reloading = true;
        this.updateActionMenu();
        Invoke("setCanonReady", 3);
        return true;
    }

    protected void reloadCanon()
    {
        //TODO: changement du cd en fonction des stat du getMember()
        this.reloading = true;
        Invoke("reloadEnd", 3);
    }

    protected void reloadEnd()
    {
        this.setCanonReady();
        this.doDamage();
    }

    /** REPAIR **/
    protected override void doRepairActionEnd()
    {
        //TODO value life en fonction du member
        this.setCurrentLife(this.currentLife + 20);
    }

    protected override bool doRepairAction()
    {
        print("repair canon");
        //TODO cooldown en fonction du member
        Invoke("doRepairEnd", 2);
        return true;
    }

    /** DO DAMAGE **/
    protected override bool doDamageAction()
    {
        bool result = false;

        print("fire canon");
        if (target != null)
        {
            if (ready)
            {
                if (UnityEngine.Random.value > 0.75)
                {
                    shotCutscene.StartCutscene();
                    WaitForX(shotCutscene.duration);
                }
                Battle_Ship enemy = target.GetComponentInParent<Battle_Ship>();
                if (!target.isAvailable())
                {
                    this.stopAttack();
                    this.target = null;
                    result = false;
                }
                if (enemy != null && canAttack)
                {
                    this.setAttacking(true);
                    // TODO send the bullet object with damage, type, onHitEffect etc
                    if (target.receiveDamage(20))
                    {
                        print("Aouch they lost 20 pv");
                    }
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

    /** RECEIVE DAMAGE **/
    protected override bool receiveDamageAction(int damage)
    {
        this.setCurrentLife(this.currentLife - damage);
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

    public int getDamage()
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
        print("canon ready");
    }
}
