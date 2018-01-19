using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wheel : ShipElement
{

    public Ship_Direction direction;
    // Use this for initialization
    public Wheel() : base(200, Ship_Item.WHEEL)
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
        if (this.getMember() && this.isWorking())
        {
            if (this.direction != Ship_Direction.FRONT)
                actions.Add(new ActionMenuItem("Front", directionFront));
            if (this.direction != Ship_Direction.RIGHT)
                actions.Add(new ActionMenuItem("Right", directionRight));
            if (this.direction != Ship_Direction.LEFT)
                actions.Add(new ActionMenuItem("Left", directionLeft));
        }
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
        this.GetComponentInParent<Battle_Ship>().receiveDamage(canonBall.getAmmunition().getDamage() / 3);
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

    protected override void applyMalusOnNotWorking()
    {
    }

    protected override void applyChangeOnRevive()
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

    /** DIRECTION **/
    public bool directionFront()
    {
        this.direction = Ship_Direction.FRONT;
        this.getParentShip().changeDirection(this.direction);
        this.updateParentActionMenu();
        return true;
    }

    public bool directionRight()
    {
        this.direction = Ship_Direction.RIGHT;
        this.getParentShip().changeDirection(this.direction);
        this.updateParentActionMenu();
        return true;
    }

    public bool directionLeft()
    {
        this.direction = Ship_Direction.LEFT;
        this.getParentShip().changeDirection(this.direction);
        this.updateParentActionMenu();
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
    
    /** GETTERS **/
    public override bool isWorking()
    {
        return this.isAvailable() && this.getPercentLife() > 80;
    }

}
