using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum ObjectiveAction { SHOOT, ABOARD, ESCAPE }
public class Battle_Enemy : Battle_Ship
{
    private Battle_Ship enemy;
    private List<Canon> canons;
    private ObjectiveAction objective;
    private List<Ship_Item> crucialItems;

    private bool canPlay = false;

    public Battle_Enemy() : base(200, false)
    {
    }

    protected override void selfInit()
    {
        this.enemy = GameRulesManager.GetInstance().getShip(GameRulesManager.GetInstance().playerID);

        this.canons = new List<Canon>();
        this.crucialItems = new List<Ship_Item>();

        List<RoomElement> rooms = this.parseShipElement(Ship_Item.CANON);
        foreach (var room in rooms)
        {
            this.canons.Add((Canon)room.getEquipment());
        }

        this.defineObjectiveAction();

        this.canPlay = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.canPlay)
            if (!GameRulesManager.GetInstance().isEndOfTheGame())
                this.doScriptAction();
            else
                this.stopAllAction();
    }

    public void defineObjectiveAction()
    {
        this.objective = ObjectiveAction.ABOARD;

        if (this.objective == ObjectiveAction.ABOARD || this.objective == ObjectiveAction.ESCAPE)
        {
            this.crucialItems.Add(Ship_Item.SAILS);
            this.crucialItems.Add(Ship_Item.WHEEL);
        }
        else if (this.objective == ObjectiveAction.SHOOT)
        {
            this.crucialItems.Add(Ship_Item.POWDER);
        }
        if (this.objective != ObjectiveAction.ESCAPE)
        {
            this.crucialItems.Add(Ship_Item.CANTEEN);
            this.crucialItems.Add(Ship_Item.ALCOHOL);
        }
    }

    /** ACTIONS **/
    public override void aboardingEnemy(bool status)
    {
    }

    public override void escape()
    {
        GameRulesManager.GetInstance().enemyDestroyed(this.id, DestroyedStatus.NONE);
    }

    public override void canEscape(bool value)
    {
        this.canEscapeAction = value;
    }

    public override void die(DestroyedStatus status)
    {
        GameRulesManager.GetInstance().enemyDestroyed(this.id, status);
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
        this.manageCanon();
    }

    /** UTILS **/


    /** ATTACK **/
    public void manageCanon()
    {
        foreach (var canon in this.canons)
        {
            if (!canon.isInGoodPositionToShoot(this.enemy))
                continue;
            if (canon.isWorking() && canon.getMember() != null && !canon.isAttacking())
            {
                if (canon.setTarget(this.findTargetElement()))
                {
                    //Debug.Log("CANNON SHOOOT");
                    canon.doDamage();
                }
            }
        }
        foreach (var canon in this.canons)
        {
            if (!canon.isInGoodPositionToShoot(this.enemy))
                continue;
            if (!canon.isWorking() || canon.getMember() == null)
            {
                canon.callBestCrewMember();
            }
        }
    }

    /** RESEARCH **/
    RoomElement findTargetElement()
    {
        if (this.enemy == null)
            return null;
        foreach (RoomElement room in this.enemy.getRooms())
        {
            if (room.getEquipment() != null && room.getEquipment().isAvailable() && room.getEquipment().getType() != Ship_Item.WHEEL)
            {
                return room;
            }
        }
        return null;
    }
}