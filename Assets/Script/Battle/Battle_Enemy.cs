using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Battle_Enemy : Battle_Ship
{
    protected bool needAFreeCrewMember = false;

    public Battle_Enemy() : base(200, false)
    {
    }

    // Update is called once per frame
    void Update()
    {
        //if (!GameRulesManager.GetInstance().endOfTheGame)
            //this.doScriptAction();
    }

    /** ACTIONS **/
    public override void aboardingEnemy()
    {
    }

    public override void escape()
    {
    }

    public override void canAboarding(bool value)
    {
        this.canAboardingAction = value;
    }

    public override void canEscape(bool value)
    {
        this.canEscapeAction = value;
    }

    public override void die()
    {
        GameRulesManager.GetInstance().guiAccess.endMessage.text = "You killed your ennemy";
        GameRulesManager.GetInstance().guiAccess.endPanel.gameObject.SetActive(true);
        GameRulesManager.GetInstance().endOfTheGame = true;
    }

    /** SCRIPT **/
    public void doScriptAction()
    {
        Battle_CrewMember member = getFreeCrewMember();

        if (!this.repairCanteen(member))
        {
            if (!this.repairCanon(member))
            {
                this.attackMode(member);
            }
            else
            {
                this.needAFreeCrewMember = false;
            }
        }
        else
        {
            this.needAFreeCrewMember = false;
        }
        print("NEED A CREW : " + needAFreeCrewMember);
        if (this.needAFreeCrewMember)
        {
            this.freeACrewMember();
        }
    }

    /** UTILS **/
    protected bool freeACrewMember()
    {
        GameObject player = GameObject.Find("Enemy");

        foreach (Transform child in player.transform)
        {
            ShipElement element = child.GetComponent<ShipElement>();

            if (element)
            {
                if (!element.actionIsRunning())
                {
                    foreach (Transform child2 in element.transform)
                    {
                        Battle_CrewMember member = child2.GetComponent<Battle_CrewMember>();

                        if (member)
                        {
                            member.freeCrewMemberFromShipElement(element, element.transform.parent.gameObject);
                            print("MEMBER CREW FREE");
                            return true;
                        }
                    }
                }
                else if (element.actionStopRunning())
                {
                    print("ACTION STOP");
                    return true;
                }
            }
        }
        return false;
    }

    protected Battle_CrewMember getFreeCrewMember()
    {
        Battle_CrewMember result = null;

        GameObject player = GameObject.Find("Enemy");

        foreach (Transform child in player.transform)
        {
            Battle_CrewMember member = child.GetComponent<Battle_CrewMember>();

            if (member != null)
            {
                return member;
            }
        }
        return result;
    }

    /** ATTACK **/
    public bool attackMode(Battle_CrewMember member)
    {
        ShipElement target = this.findTargetElement();

        if (target)
        {
            foreach (Transform child in this.transform)
            {
                Canon canon = child.GetComponent<Canon>();

                if (canon != null && canon.isAvailable() && !canon.isAttacking() && !canon.actionIsRunning())
                {
                    if (canon.getTarget() == null)
                    {
                        canon.setTarget(target);
                    }
                    if (canon.getMember() == null)
                    {
                        if (member == null)
                        {
                            this.needAFreeCrewMember = true;
                            return false;
                        }
                        member.assignCrewMemberToShipElement(canon, this.gameObject);
                    }
                    canon.doDamage();
                    return true;
                }
            }
        }
        return false;
    }

    /** RESEARCH **/
    ShipElement findTargetElement()
    {
        GameObject player = GameObject.Find("Player");

        foreach (Transform child in player.transform)
        {
            ShipElement target = child.GetComponent<ShipElement>();

            if (target != null && target.isAvailable())
            {
                return target;
            }
        }
        return null;
    }

    /** REPAIR **/
    bool repairCanon(Battle_CrewMember member)
    {
        foreach (Transform child in this.transform)
        {
            Canon canon = child.GetComponent<Canon>();

            if (canon != null && !canon.isAvailable())
            {
                if (member == null)
                {
                    this.needAFreeCrewMember = true;
                    return false;
                }
                member.assignCrewMemberToShipElement(canon, this.gameObject);
                canon.doRepair();
                return true;
            }
        }
        return false;
    }

    bool repairCanteen(Battle_CrewMember member)
    {
        Canteen canteen = this.GetComponent<Canteen>();

        if (canteen)
            print("canteeLife: " + canteen.getPercentLife());
        if (canteen && canteen.getPercentLife() < 90)
        {
            if (member == null)
            {
                this.needAFreeCrewMember = true;
                return false;
            }
            member.assignCrewMemberToShipElement(canteen, this.gameObject);
            canteen.doRepair();
            return true; ;
        }
        return false;
    }
}
