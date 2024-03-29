﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public abstract class GuiElement : MonoBehaviour
{
    protected bool selected = false;
    protected bool focused = false;
    protected SimpleObjectPool buttonObjectPool;
    protected GameObject actionMenu = null;
    protected SpriteOutline outline = null;
    protected List<ActionMenuItem> actionList = new List<ActionMenuItem>();

    void Start()
    {
        this.buttonObjectPool = GameObject.Find("SimpleActionMenuPool").GetComponent<SimpleObjectPool>();
        this.outline = GetComponent<SpriteOutline>();

        if (!this.GetComponentInParent<Battle_Enemy>())
        {
            this.createActionMenu();
        }
    }


    public abstract void StartMyself();

    /** INPUT **/
    public int hasInputMouse(Boolean clicked)
    {
        return hasInputMouse(clicked, false);
    }

    public int hasInputMouse(Boolean clicked, Boolean multipleSelect)
    {
        if (clicked)
        {
            if (this.focused)
            {
                this.unfocus();
                return 0;
            }
            else
            {
                this.focus();
                return 1;
            }
        } else if (!multipleSelect)
        {
            this.unfocus();
        }
        return -1;
    }

    /** INTERACTION **/
    public void focus()
    {
        this.focused = true;
        this.select();
        if (this.actionMenu)
            this.updateActionMenu();
    }

    public void unfocus()
    {
        this.focused = false;
        this.unselect();
        if (this.actionMenu)
            this.actionMenu.SetActive(false);
    }

    public void select()
    {
        //Debug.Log("Click on " + this.getIdentifier());
        this.selected = true;
        if (this.outline)
            this.outline.enabled = true;
    }

    public void unselect()
    {
        this.selected = false;
        if (this.outline)
            this.outline.enabled = false;
    }

    /** GUI CREATOR **/

    protected abstract void createActionList();

    protected void createActionMenu()
    {
        this.createActionList();
        this.actionMenu = buttonObjectPool.GetObject();
        this.actionMenu.transform.SetParent(GameObject.Find("TaskMenuArea").gameObject.transform);

        this.actionMenu.transform.localPosition = new Vector3(GameRulesManager.GetInstance().guiAccess.ShortCutCanon.transform.parent.transform.localPosition.x - 40, GameRulesManager.GetInstance().guiAccess.ShortCutCanon.transform.parent.transform.localPosition.y + 80, GameRulesManager.GetInstance().guiAccess.ShortCutCanon.transform.parent.transform.localPosition.z);
        this.actionMenu.transform.localScale = new Vector3(1, 1, 1);

        this.actionMenu.GetComponentInChildren<ActionMenuList>().init(this.actionList);
        this.actionMenu.SetActive(false);
    }

    private void updateActionMenuItem()
    {
        if (!this.actionMenu)
            return;
        this.createActionList();
        this.actionMenu.GetComponentInChildren<ActionMenuList>().update(this.actionList);
    }

    public void updateActionMenu()
    {
        if (!this.actionMenu || !this.focused)
            return;
        this.updateActionMenuItem();
        if (this.actionList.Count != 0)
            this.actionMenu.SetActive(true);
        else
            this.actionMenu.SetActive(false);
    }

    // action null
    protected bool none()
    {
        return true;
    }

    /** GETTERS **/
    public virtual string getIdentifier()
    {
        return this.GetInstanceID() + "";
    }

    public bool isSelected()
    {
        return this.selected;
    }

    public bool isFocused()
    {
        return this.focused;
    }

    public List<ActionMenuItem> getActionList()
    {
        return this.actionList;
    }
}