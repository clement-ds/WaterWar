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
    {/*
        if (!GameRulesManager.GetInstance().endOfTheGame)
            this.doScriptAction();
        else
            this.stopAllAction();*/
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
    public void stopAllAction()
    {
        foreach (RoomElement element in this.rooms)
        {
            element.actionStopRunning();
        }
    }

    public void doScriptAction()
    {
        Battle_CrewMember member = getFreeCrewMember();

        if (member != null)
        {
            this.attackMode(member);
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
        foreach (RoomElement element in this.rooms)
        {
            if (element)
            {
                if (!element.actionIsRunning())
                {
                    foreach (Transform child2 in element.transform)
                    {
                        Battle_CrewMember member = child2.GetComponent<Battle_CrewMember>();

                        if (member)
                        {
                            //member.freeCrewMemberFromShipElement(element, element.transform.parent.gameObject);
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
        RoomElement target = this.findTargetElement();

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
                        //member.assignCrewMemberToShipElement(canon, this.gameObject);
                    }
                    canon.doDamage();
                    return true;
                }
            }
        }
        return false;
    }

    /** RESEARCH **/
    RoomElement findTargetElement()
    {
        Battle_Ship player = GameObject.Find("Player").GetComponent<Battle_Ship>();

        foreach (RoomElement room in player.getRooms())
        {
            if (room.getEquipment() != null && room.getEquipment().isAvailable())
            {
                return room;
            }
        }
        return null;
    }
}