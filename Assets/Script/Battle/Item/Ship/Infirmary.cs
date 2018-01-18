using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Infirmary : ShipElement
{
    protected float baseCooldown = 3.0f;
    private float baseHeal = 10f;

    // Use this for initialization
    public Infirmary() : base(100, Ship_Item.INFIRMARY)
    {
        this.baseCooldown = 3.0f;
        this.baseHeal = 10f;
    }

    public override void init()
    {
    }
    public override void reInitValues()
    {
    }

    /** SPECIFIC ACTION **/
    private void healCrew()
    {
        Battle_CrewMember doctor = this.GetComponentInChildren<Battle_CrewMember>();

        if (doctor != null)
        {
            String teamId = this.getParentShip().getId();
            Battle_CrewMember[] members = this.transform.GetComponentsInParent<Battle_CrewMember>();

            foreach (Battle_CrewMember member in members)
            {
                if (member.getTeamId() == teamId)
                    member.getProfile().healDamage(doctor.getProfile().getValueByCrewSkill(SkillAttribute.HealValue, this.baseHeal));
            }
            launchHealCrew();
        }
    }

    public void launchHealCrew()
    {
        Battle_CrewMember doctor = this.GetComponentInChildren<Battle_CrewMember>();

        if (doctor != null)
        {
            Invoke("healCrew", doctor.getProfile().getValueByCrewSkill(SkillAttribute.HealTime, this.baseCooldown));
        }
    }

    /** GUI CREATOR **/
    public override List<ActionMenuItem> createActionList()
    {
        List<ActionMenuItem> actions = new List<ActionMenuItem>();
        this.addGeneralActionsTo(actions);
        return actions;
    }

    /** AVAILABLE POSITION CREATOR **/
    public override void createAvailableCrewMemberPosition()
    {
        this.availablePosition = new AvailablePosition(new Vector3(-0.1f, -0.1f, -1f));
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
