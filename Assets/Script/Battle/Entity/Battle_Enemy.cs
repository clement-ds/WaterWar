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

    public Battle_Enemy() : base(200, false)
    {
    }

    public override void init()
    {
        this.createRoom();
        this.createCrew();
        this.enemy = GameRulesManager.GetInstance().getShip(GameRulesManager.GetInstance().playerID);
        this.canons = new List<Canon>();
        this.crucialItems = new List<Ship_Item>();

        List<RoomElement> rooms = this.parseShipElement(Ship_Item.CANON);
        foreach (var room in rooms)
        {
            this.canons.Add((Canon)room.getEquipment());
        }

        this.defineObjectiveAction();
    }

    // Update is called once per frame
    void Update()
    {
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
        } else if (this.objective == ObjectiveAction.SHOOT)
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
            if (!canon.isWorking() || canon.getMember() == null)
            {
                canon.callBestCrewMember();
            }
            else
            {
                if (!canon.isAttacking())
                {
                    canon.setTarget(this.findTargetElement());
                    canon.doDamage();
                }
            }
        }
    }

    /** RESEARCH **/
    RoomElement findTargetElement()
    {

        foreach (RoomElement room in this.enemy.getRooms())
        {
            if (room.getEquipment() != null && room.getEquipment().isAvailable())
            {
                return room;
            }
        }
        return null;
    }
}