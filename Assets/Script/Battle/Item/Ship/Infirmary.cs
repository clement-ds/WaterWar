using UnityEngine;
using System.Collections;
using System;

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

    public override void StartMyself()
    {
        base.StartMyself();

        this.task = new TimerTask(HealCrew, this.cooldown);
    }

    /** SPECIFIC ACTION **/
    private void HealCrew()
    {
        Battle_CrewMember[] members = this.transform.GetComponentsInParent<Battle_CrewMember>();

        foreach (Battle_CrewMember member in members)
        {
            if (member.getMember().job == CrewMember_Job.Medic)
            {
                this.task.cooldown = (this.task.cooldown > 1 ? this.task.cooldown - 1f : this.task.cooldown);
                this.healRatio += 10f;
            }
        }

        foreach (Battle_CrewMember member in members)
        {
            member.getMember().healDamage(this.healRatio);
        }
    }

    /** GUI CREATOR **/
    protected override void createActionList()
    {
        this.actionList.RemoveRange(0, this.actionList.Count);
        if (this.getMember() && !this.isRepairing() && this.currentLife != this.life)
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
    protected override void doRepairActionEnd()
    {
        this.setCurrentLife(this.currentLife + this.GetComponentInChildren<Battle_CrewMember>().getMember().getValueByCrewSkill(SkillAttribute.RepairValue, 40));
    }

    protected override bool doRepairAction()
    {
        Invoke("doRepairEnd", this.GetComponentInChildren<Battle_CrewMember>().getMember().getValueByCrewSkill(SkillAttribute.RepairTime, 1));
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
