using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sails : ShipElement
{

    public Ship_Direction direction;
    // Use this for initialization
    public Sails() : base(100, Ship_Item.SAILS)
    {
        this.direction = Ship_Direction.FRONT;
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
        this.availablePosition = new AvailablePosition(new Vector3(-0.1f, -0.1f, -1f));
    }

    /** ON HIT EFFECT **/
    protected override void dealDamageAsRepercution(Battle_CanonBall canonBall)
    {
        this.getParentShip().receiveDamage(canonBall.getAmmunition().getDamage());
    }

    protected override void dealDamageOnDestroy()
    {
        this.getParentShip().receiveDamage(20);
    }

    protected override void applyMalusOnHit(Battle_CanonBall canonBall)
    {

    }

    protected override void applyMalusOnDestroy()
    {

    }

    protected override void applyMalusOnNotWorking()
    {
        this.getParentShip().setSpeed(this.getParentShip().getSpeed() / 2);
    }

    protected override void applyChangeOnRevive()
    {
        this.getParentShip().setSpeed(this.getParentShip().getSpeed() * 2);
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
        return this.isAvailable() && this.getPercentLife() > 50;
    }

}
