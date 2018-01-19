using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Warehouse : ShipElement {
    
    // Use this for initialization
    protected Warehouse(float life, Ship_Item type) : base(life, type)
    {
    }

    public override void init()
    {
    }

    public override void reInitValues()
    {
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
    }

    /** ON HIT EFFECT **/
    protected override void dealDamageAsRepercution(Battle_CanonBall canonBall)
    {
        this.GetComponentInParent<Battle_Ship>().receiveDamage(canonBall.getAmmunition().getDamage() / 4);
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
        return this.isAvailable() && this.getPercentLife() > 30;
    }

}
