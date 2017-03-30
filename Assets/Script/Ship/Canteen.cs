using UnityEngine;
using System.Collections;
using System;

public class Canteen : ShipElement {

    // Use this for initialization
    public Canteen() : base(100)
    {
    }

    /** GUI CREATOR **/
    protected override void createActionList()
    {
        this.actionList.RemoveRange(0, this.actionList.Count);
        if (this.currentLife != this.life)
            this.actionList.Add(new ActionMenuItem("Repair", doRepair));
    }

    /** AVAILABLE POSITION CREATOR **/
    protected override void createAvailableCrewMemberPosition()
    {
        this.availablePosition.Add(new AvailablePosition(new Vector3(-0.1f, -0.1f, 0f)));
        this.availablePosition.Add(new AvailablePosition(new Vector3(0.1f, -0.1f, 0f)));
    }

    /** ON HIT EFFECT **/
    protected override void dealDamageAsRepercution(int damage)
    {
        this.GetComponentInParent<Battle_Ship>().receiveDamage(damage / 2);
    }

    protected override void dealDamageOnDestroy()
    {
        this.GetComponentInParent<Battle_Ship>().receiveDamage(this.life);
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
        if (this.repairing)
        {
            return true;
        }
        return false;
    }

    /** REPAIR **/
    protected override void doRepairEnd()
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
        return true;
    }

    protected override void doDamageAnimation()
    {
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
}
