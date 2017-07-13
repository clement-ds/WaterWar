using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

public class Battle_Player : Battle_Ship
{
    public Battle_Player() : base(200, true)
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameRulesManager.GetInstance().endOfTheGame)
        {
            this.hasInputMouse();
            if (!this.canEscapeAction && float.Parse(this.guiAccess.distanceToEnemy.text) > 20)
            {
                this.canEscape(true);
            }
            else if (this.canEscapeAction && float.Parse(this.guiAccess.distanceToEnemy.text) < 20)
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
        this.guiAccess.endMessage.text = "You escape the fight";
        this.guiAccess.endPanel.gameObject.SetActive(true);
    }

    public override void canAboarding(bool value)
    {
        this.canAboardingAction = value;
        this.guiAccess.boardingButton.gameObject.SetActive(value);
    }

    public override void canEscape(bool value)
    {
        this.canEscapeAction = value;
        this.guiAccess.escapeButton.gameObject.SetActive(value);
    }

    public override void die()
    {
        this.guiAccess.endMessage.text = "Your opponent killed you";
        this.guiAccess.endPanel.gameObject.SetActive(true);
        GameRulesManager.GetInstance().endOfTheGame = true;
    }

    /** INPUT **/
    void hasInputMouse()
    {
        if (Camera.main)
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 touchPos = new Vector2(wp.x, wp.y);
            if (Input.GetMouseButtonDown(0))
            {
                if (!this.checkSelfShip(touchPos))
                {
                    this.checkEnemyShip(touchPos);
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                checkTargetForCrewMember(touchPos);
            }
        }
    }

    /** CHECK SELF SHIP **/
    private bool checkShipElementInSelfShip(Vector2 touchPos, GameObject player, ShipElement target)
    {
        bool result = false;
        if (target != null)
        {
            result = target.GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos);
            if (result)
            {
                foreach (Transform child2 in player.transform)
                {
                    ShipElement target2 = child2.GetComponent<ShipElement>();

                    if (target2 != null && target.GetInstanceID() != target2.GetInstanceID())
                    {
                        target2.unfocus();
                    }
                }
            }
            target.hasInputMouse(result);
        }
        return result;
    }

    private bool checkCrewMemberInSelfShip(Vector2 touchPos, Battle_CrewMember target)
    {
        bool result = false;

        if (target != null)
        {
            result = target.GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos); ;

            target.hasInputMouse(result);
        }
        return result;
    }

    private bool checkSelfShip(Vector2 touchPos)
    {
        GameObject player = GameObject.Find("Player");
        bool result = false;

        foreach (Transform child in player.transform)
        {
            result = checkShipElementInSelfShip(touchPos, player, child.GetComponent<ShipElement>());

            if (!result)
            {
                result = checkCrewMemberInSelfShip(touchPos, child.GetComponentInChildren<Battle_CrewMember>());
            }
        }
        return result;
    }

    private bool checkTargetForCrewMember(Vector2 touchPos)
    {
        GameObject player = GameObject.Find("Player");
        ShipElement target = null;
        bool result = false;

        foreach (Transform child in player.transform)
        {
            Battle_CrewMember crewMember = child.GetComponentInChildren<Battle_CrewMember>();

            if (crewMember != null && crewMember.isFocused())
            {
                crewMember.freeCrewMemberFromParent(player);
                foreach (Transform child2 in player.transform)
                {
                    target = child2.GetComponent<ShipElement>();
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
                            crewMember.assignCrewMemberToShipElement(target, player);
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
    private bool checkEnemyShip(Vector2 touchPos)
    {
        GameObject player = GameObject.Find("Player");
        GameObject enemy = GameObject.Find("Enemy");
        bool result = false;

        foreach (Transform child in enemy.transform)
        {
            ShipElement target = child.GetComponent<ShipElement>();

            if (target != null)
            {
                if (target.GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
                {
                    foreach (Transform child2 in player.transform)
                    {
                        Canon target2 = child2.GetComponent<Canon>();

                        if (target2)
                            if (target2 != null && target2.isFocused())
                            {
                                print("change target to : " + target);
                                target2.setTarget(target);
                                result = true;
                            }
                    }
                }
            }
        }
        return result;
    }

    /** CREW MANAGER **/
    private Battle_CrewMember getSelectedCrewMember()
    {
        GameObject player = GameObject.Find("Player");
        foreach (Transform child in player.transform)
        {
            Battle_CrewMember target = child.GetComponent<Battle_CrewMember>();

            if (target != null && target.isSelected())
            {
                return target;
            }
        }
        return null;
    }
}
