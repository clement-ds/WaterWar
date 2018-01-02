using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Infirmary : ShipElement
{

    protected TimerTask task;
    protected float cooldown;
    private float healRatio;


    // Use this for initialization
    public Infirmary() : base(100, Ship_Item.INFIRMARY)
    {
        this.cooldown = 3.0f;
        this.healRatio = 10f;
    }

    public override void init()
    {
        this.task = new TimerTask(HealCrew, this.cooldown);
    }
    public override void reInitValues()
    {
    }

    /** SPECIFIC ACTION **/
    private void HealCrew()
    {
        Battle_CrewMember[] members = this.transform.GetComponentsInParent<Battle_CrewMember>();

        foreach (Battle_CrewMember member in members)
        {
            if (member.getProfile().job == CrewMember_Job.Medic)
            {
                this.task.cooldown = (this.task.cooldown > 1 ? this.task.cooldown - 1f : this.task.cooldown);
                this.healRatio += 10f;
            }
        }

        foreach (Battle_CrewMember member in members)
        {
            member.getProfile().healDamage(this.healRatio);
        }
    }

    /** GUI CREATOR **/
    public override List<ActionMenuItem> createActionList()
    {
        List<ActionMenuItem> actionList = new List<ActionMenuItem>();
        return actionList;
    }

    /** AVAILABLE POSITION CREATOR **/
    public override void createAvailableCrewMemberPosition()
    {
        this.availablePosition = new AvailablePosition(new Vector3(-0.1f, -0.1f, 0f));
    }

    /** ON HIT EFFECT **/
    protected override void dealDamageAsRepercution(Battle_CanonBall canonBall)
    {
        this.GetComponentInParent<Battle_Ship>().receiveDamage(canonBall.getAmmunition().getDamage() / 2);
    }

    protected override void dealDamageOnDestroy()
    {
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
        return false;
    }

    public override bool actionStopRunning()
    {
        return false;
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

    /** GETTERS **/
    public override bool isWorking()
    {
        return this.isAvailable() && this.getPercentLife() > 70;
    }

}
