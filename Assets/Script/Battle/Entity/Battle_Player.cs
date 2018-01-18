using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

public class Battle_Player : Battle_Ship
{
    List<Battle_CrewMember> selectedCrewMembers;

    public Battle_Player() : base(200, true)
    {
        this.selectedCrewMembers = new List<Battle_CrewMember>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameRulesManager.GetInstance().isEndOfTheGame())
        {
            this.hasMouseInteraction();
            try
            {
                if (!this.canEscapeAction && GameRulesManager.GetInstance().guiAccess.distanceToEnemy.text != "" && float.Parse(GameRulesManager.GetInstance().guiAccess.distanceToEnemy.text) > 15)
                {
                    this.canEscape(true);
                }
                else if (this.canEscapeAction && float.Parse(GameRulesManager.GetInstance().guiAccess.distanceToEnemy.text) < 15)
                {
                    this.canEscape(false);
                }
            }
            catch { }
        }
    }

    /** ACTIONS **/

    public override void aboardingEnemy()
    {
    }

    public override void escape()
    {
        GameRulesManager.GetInstance().playerEscaped(this.id);
    }

    public override void canEscape(bool value)
    {
        this.canEscapeAction = value;
        GameRulesManager.GetInstance().guiAccess.escapeButton.gameObject.SetActive(value);
    }

    public override void die(DestroyedStatus status)
    {
        GameRulesManager.GetInstance().playerDestroyed(this.id, status);
    }

    /** INPUT **/
    private void hasMouseInteraction()
    {
        if (Camera.main)
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 touchPos = new Vector2(wp.x, wp.y);

            bool leftMouseButtonOn = Input.GetMouseButtonDown(0);

            if (Input.GetMouseButtonDown(1))
            {
                checkTargetForCrewMember(touchPos);
            }
            else
            {
                if (!this.checkSelfShip(touchPos, leftMouseButtonOn))
                {
                    this.checkEnemyShip(touchPos, leftMouseButtonOn);
                }
            }
        }
    }

    /** CHECK SELF SHIP **/
    private bool checkElementInSelfShip(Vector2 touchPos, bool hasClick)
    {
        bool result = false;
        foreach (RoomElement item in this.rooms)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            result = item.transform.GetComponent<BoxCollider>().Raycast(ray, out hit, 100.0F);

            if (!hasClick && result)
            {
                /* do hover here */
                return result;
            }

            if (result)
            {
                foreach (RoomElement item2 in this.rooms)
                {
                    if (item2 != null && item.getId() != item2.getId())
                    {
                        item2.unfocus();
                    }
                }
            }
            item.hasInputMouse(result);
        }
        return result;
    }

    private bool checkCrewMemberInSelfShip(Vector2 touchPos, bool hasClick)
    {
        bool result = false;
        bool hasFocus = false;
        bool canSelect = true;

        if (!hasClick)
        {
            return result;
        }
        foreach (Battle_CrewMember item in this.crewMembers)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            result = item.transform.GetComponent<BoxCollider>().Raycast(ray, out hit, 100.0F);

            if (result)
            {
                hasFocus = true;
            }

            if (!canSelect)
            {
                result = false;
            }

            int isFocus = item.hasInputMouse(result);

            if (isFocus == 0)
            {
                this.selectedCrewMembers.Remove(item);
                if (this.selectedCrewMembers.Count > 0)
                    this.selectedCrewMembers[0].focus();
            }
            else if (isFocus == 1)
            {
                this.selectedCrewMembers.Remove(item);
                if (this.selectedCrewMembers.Count > 0)
                {
                    this.selectedCrewMembers[0].unfocus();
                    this.selectedCrewMembers[0].select();
                }
                this.selectedCrewMembers.Insert(0, item);
            }
            if (result && canSelect)
            {
                canSelect = false;
            }
        }
        return hasFocus;
    }

    private bool checkSelfShip(Vector2 touchPos, bool hasClick)
    {
        bool result = false;

        result = checkElementInSelfShip(touchPos, hasClick);
        if (!result)
            checkCrewMemberInSelfShip(touchPos, hasClick);

        return result;
    }

    private bool checkTargetForCrewMember(Vector2 touchPos)
    {
        RoomElement target = null;
        bool result = false;

        foreach (Battle_CrewMember crewMember in this.crewMembers)
        {
            if (crewMember.isFocused() && !crewMember.isMoving())
            {
                foreach (RoomElement tmp in RoomUtils.Rooms)
                {
                    target = tmp;
                    if (target != null)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        if (tmp.transform.GetComponent<BoxCollider>().Raycast(ray, out hit, 100.0F))
                        {
                            break;
                        }
                        target = null;
                    }
                }
                if (target != null)
                {
                    if (crewMember.getEquipment() == null || !crewMember.getEquipment().actionIsRunning())
                    {
                        if (target.hasAvailableCrewMemberPosition())
                        {
                            result = crewMember.assignCrewMemberToRoom(target);
                        }
                    }
                }
                break;
            }
        }
        return result;
    }

    /** CHECK ENEMY SHIP **/
    private bool checkEnemyShip(Vector2 touchPos, bool hasClick)
    {
        Battle_Ship enemy = GameObject.Find("Enemy").GetComponent<Battle_Ship>();
        bool targetFocused = false;
        bool targetClicked = false;

        foreach (RoomElement target in enemy.getRooms())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (target.transform.GetComponent<BoxCollider>().Raycast(ray, out hit, 100.0F))
            {
                if (MouseManager.getInstance().getCursorTexture() == ECursor.SEARCH_TARGET)
                {
                    targetFocused = true;
                    MouseManager.getInstance().setCursor(ECursor.FOCUS_TARGET);
                }
                if (!hasClick)
                    continue;
                foreach (RoomElement room in this.rooms)
                {
                    if (room.getEquipment() != null && room.getEquipment().getType() == Ship_Item.CANON && ((Canon)room.getEquipment()).isSelectingTarget())
                    {
                        print("change target to : " + target);
                        ((Canon)room.getEquipment()).setTarget(target);
                        targetClicked = true;
                    }
                }
            }
        }
        if (!targetFocused && MouseManager.getInstance().getCursorTexture() == ECursor.FOCUS_TARGET)
            MouseManager.getInstance().setCursor(ECursor.SEARCH_TARGET);
        if (hasClick)
            MouseManager.getInstance().setCursor(ECursor.BASIC);
        return targetClicked;
    }

    /** CREW MANAGER **/
    private Battle_CrewMember getSelectedCrewMember()
    {
        foreach (Battle_CrewMember target in this.crewMembers)
        {
            if (target.isSelected())
            {
                return target;
            }
        }
        return null;
    }

    /** GETTERS **/
    public List<Battle_CrewMember> getSelectedCrewMembers()
    {
        return this.selectedCrewMembers;
    }
}
