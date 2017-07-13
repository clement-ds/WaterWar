using UnityEngine;
using System.Collections;

public class Helm : ShipElement {

    public Ship_Direction direction;
    // Use this for initialization
    public Helm() : base(200, Ship_Item.HELM)
    {
        this.direction = Ship_Direction.FRONT;
    }

    /** GUI CREATOR **/
    protected override void createActionList()
    {
        this.actionList.RemoveRange(0, this.actionList.Count);
        if (this.getMember())
        {
            if (!this.isRepairing() && this.currentLife != this.life)
            {
                this.actionList.Add(new ActionMenuItem("Repair", doRepair));
            } else if (this.getMember().getMember().job == CrewMember_Job.Captain)
            {
                if (this.direction != Ship_Direction.FRONT)
                    this.actionList.Add(new ActionMenuItem("Front", directionFront));
                if (this.direction != Ship_Direction.RIGHT)
                    this.actionList.Add(new ActionMenuItem("Right", directionRight));
                if (this.direction != Ship_Direction.LEFT)
                    this.actionList.Add(new ActionMenuItem("Left", directionLeft));
            }
        }
    }

    /** AVAILABLE POSITION CREATOR **/
    protected override void createAvailableCrewMemberPosition()
    {
        this.availablePosition.Add(new AvailablePosition(new Vector3(-0.1f, -0.1f, 0f)));
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

    /** DIRECTION **/
    public bool directionFront()
    {
        this.direction = Ship_Direction.FRONT;
        this.transform.parent.GetComponent<Battle_Ship>().changeDirection(this.direction);
        this.updateActionMenu();
        return true;
    }

    public bool directionRight()
    {
        this.direction = Ship_Direction.RIGHT;
        this.transform.parent.GetComponent<Battle_Ship>().changeDirection(this.direction);
        this.updateActionMenu();
        return true;
    }

    public bool directionLeft()
    {
        this.direction = Ship_Direction.LEFT;
        this.transform.parent.GetComponent<Battle_Ship>().changeDirection(this.direction);
        this.updateActionMenu();
        return true;
    }

    /** REPAIR **/
    protected override void doRepairActionEnd()
    {
        this.setCurrentLife(this.currentLife + this.GetComponentInChildren<Battle_CrewMember>().getMember().getValueByCrewSkill(SkillAttribute.RepairValue, 100));
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
