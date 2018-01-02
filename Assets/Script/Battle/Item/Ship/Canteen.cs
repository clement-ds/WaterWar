using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Canteen : ShipElement {

    // Use this for initialization
    public Canteen() : base(500, Ship_Item.CANTEEN)
    {
    }

    public override void init()
    {
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
}
