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
        if (!GameRulesManager.GetInstance().endOfTheGame)
        {
            this.hasMouseInteraction();
            if (!this.canEscapeAction && GameRulesManager.GetInstance().guiAccess.distanceToEnemy.text != "" && float.Parse(GameRulesManager.GetInstance().guiAccess.distanceToEnemy.text) > 20)
            {
                this.canEscape(true);
            }
            else if (this.canEscapeAction && float.Parse(GameRulesManager.GetInstance().guiAccess.distanceToEnemy.text) < 20)
            {
                this.canEscape(false);
            }
        }
    }

    /** ACTIONS **/

    public override void aboardingEnemy()
    {
    }

    public override void escape()
    {
        GameRulesManager.GetInstance().guiAccess.endMessage.text = "You escape the fight";
        GameRulesManager.GetInstance().guiAccess.endPanel.gameObject.SetActive(true);
        GameRulesManager.GetInstance().endOfTheGame = true;
    }

    public override void canAboarding(bool value)
    {
        this.canAboardingAction = value;
        GameRulesManager.GetInstance().guiAccess.boardingButton.gameObject.SetActive(value);
    }

    public override void canEscape(bool value)
    {
        this.canEscapeAction = value;
        GameRulesManager.GetInstance().guiAccess.escapeButton.gameObject.SetActive(value);
    }

    public override void die()
    {
        GameRulesManager.GetInstance().guiAccess.endMessage.text = "Your opponent killed you";
        GameRulesManager.GetInstance().guiAccess.endPanel.gameObject.SetActive(true);
        GameRulesManager.GetInstance().endOfTheGame = true;
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
    private bool checkShipElementInSelfShip(Vector2 touchPos, bool hasClick)
    {
        bool result = false;
        foreach (ShipElement item in this.shipElements)
        {
            result = item.GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos);

            if (!hasClick)
            {
                /* do hover here */
                return result;
            }

            if (result)
            {
                foreach (ShipElement item2 in this.shipElements)
                {
                    if (item2 != null && item.GetInstanceID() != item2.GetInstanceID())
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

        foreach (Battle_CrewMember item in this.crewMembers)
        {
            result = item.GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos);

            if (!hasClick)
            {
                /* do hover here */
                return result;
            }

            int isFocus = item.hasInputMouse(result);
            if (isFocus == 0)
            {
                this.selectedCrewMembers.Remove(item);
                if (this.selectedCrewMembers.Count > 0)
                    this.selectedCrewMembers[0].focus();
            } else if (isFocus == 1)
            {
                if (this.selectedCrewMembers.Count > 0)
                {
                    this.selectedCrewMembers[0].unfocus();
                    this.selectedCrewMembers[0].select();
                }
                this.selectedCrewMembers.Insert(0, item);
            }
        }
        return result;
    }

    private bool checkSelfShip(Vector2 touchPos, bool hasClick)
    {
        bool result = false;

        Debug.Log("elements: " + this.shipElements.Count);

        result = checkShipElementInSelfShip(touchPos, hasClick);
        if (!result)
            checkCrewMemberInSelfShip(touchPos, hasClick);

        return result;
    }

    private bool checkTargetForCrewMember(Vector2 touchPos)
    {
        ShipElement target = null;
        bool result = false;

        foreach (Battle_CrewMember crewMember in this.crewMembers)
        {
            if (crewMember.isFocused())
            {
                crewMember.freeCrewMemberFromParent(crewMember.transform.root.gameObject);
                foreach (ShipElement tmp in this.shipElements)
                {
                    target = tmp;
                    if (target != null)
                    {
                        if (target.GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
                        {
                            break;
                        }
                        target = null;
                    }
                }
                if (target != null)
                {
                    if ((crewMember.GetComponentInParent<ShipElement>() == null || !crewMember.GetComponentInParent<ShipElement>().actionIsRunning())
                        && target.GetComponent<Battle_CrewMember>() == null)
                    {
                        if (target.hasAvailableCrewMemberPosition())
                        {
                            crewMember.assignCrewMemberToShipElement(target, crewMember.transform.root.gameObject);
                            result = true;
                        }
                    }
                }
                else
                {
                    //crewMember.moveTo(new Vector3(touchPos.x, touchPos.y, crewMember.transform.position.z));
                }
                break;
            }
        }
        return result;
    }

    /** CHECK ENEMY SHIP **/
    private bool checkEnemyShip(Vector2 touchPos, bool hasClick)
    {
        GameObject player = GameObject.Find("Player");
        GameObject enemy = GameObject.Find("Enemy");
        bool targetFocused = false;
        bool targetClicked = false;

        foreach (Transform child in enemy.transform)
        {
            ShipElement target = child.GetComponent<ShipElement>();

            if (target != null)
            {
                if (target.GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
                {
                    if (MouseManager.getInstance().getCursorTexture() == ECursor.SEARCH_TARGET)
                    {
                        targetFocused = true;
                        MouseManager.getInstance().setCursor(ECursor.FOCUS_TARGET);
                    }
                    if (!hasClick)
                        continue;
                    foreach (Transform child2 in player.transform)
                    {
                        Canon target2 = child2.GetComponent<Canon>();

                        if (target2)
                            if (target2 != null && target2.isSelectingTarget())
                            {
                                print("change target to : " + target);
                                target2.setTarget(target);
                                targetClicked = true;
                            }
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
