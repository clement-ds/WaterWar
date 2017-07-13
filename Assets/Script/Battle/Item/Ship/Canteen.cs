using UnityEngine;
using System.Collections;
using System;

public class Canteen : ShipElement {

    private bool isAutoRepair;
    private TimerTask autoRepair;

    // Use this for initialization
    public Canteen() : base(500, Ship_Item.CANTEEN)
    {
        this.isAutoRepair = false;
        this.autoRepair = new TimerTask(autoRepairing, 1f, false);
    }

    protected override void updateMyself()
    {
        this.autoRepair.update();
    }

    /** GUI CREATOR **/
    protected override void createActionList()
    {
        this.actionList.RemoveRange(0, this.actionList.Count);
        if (this.getMember() && this.isAutoRepair)
            this.actionList.Add(new ActionMenuItem("stop AutoRepair", stopAutoRepair));
        if (this.getMember() && !this.isAutoRepair)
            this.actionList.Add(new ActionMenuItem("launch AutoRepair", launchAutoRepair));
        if (this.getMember() && !this.isAutoRepair && !this.isRepairing() && this.currentLife != this.life)
            this.actionList.Add(new ActionMenuItem("Repair", doRepair));
    }

    /** AVAILABLE POSITION CREATOR **/
    protected override void createAvailableCrewMemberPosition()
    {
        this.availablePosition.Add(new AvailablePosition(new Vector3(-0.1f, -0.1f, 0f)));
        this.availablePosition.Add(new AvailablePosition(new Vector3(0.1f, -0.1f, 0f)));
    }

    /** ON HIT EFFECT **/
    protected override void dealDamageAsRepercution(Battle_CanonBall canonBall)
    {
        this.GetComponentInParent<Battle_Ship>().receiveDamage(canonBall.getAmmunition().getDamage() / 4);
    }

    protected override void dealDamageOnDestroy()
    {
        this.GetComponentInParent<Battle_Ship>().receiveDamage(this.life * 2);
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
        if (this.repairing)
        {
            return true;
        }
        return false;
    }

    public override bool actionStopRunning()
    {
        return false;
    }

    /** REPAIR **/
    private bool launchAutoRepair()
    {
        this.isAutoRepair = true;
        this.autoRepair.start();
        return true;
    }

    private bool stopAutoRepair()
    {
        this.isAutoRepair = false;
        this.autoRepair.stop();
        return true;
    }

    private void autoRepairing()
    {
        if (!this.isRepairing())
        {
            this.doRepair();
        }
    }

    protected override void doRepairActionEnd()
    {
        //TODO value life en fonction du member
        this.setCurrentLife(this.currentLife + this.GetComponentInChildren<Battle_CrewMember>().getMember().getCrewSkill(SkillAttribute.RepairValue));
    }

    protected override bool doRepairAction()
    {
        //TODO cooldown en fonction du member
        Invoke("doRepairEnd", this.GetComponentInChildren<Battle_CrewMember>().getMember().getCrewSkill(SkillAttribute.RepairTime));
        return true;
    }

    /** DO DAMAGE **/
    protected override bool doDamageAction()
    {
        return true;
    }

    protected override void doDamageAnimation()
    {
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
}
