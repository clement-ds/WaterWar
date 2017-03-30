using UnityEngine;
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
        this.StartMySelf();
        this.buttonObjectPool = GameObject.Find("SimpleActionMenuPool").GetComponent<SimpleObjectPool>();
        this.outline = GetComponent<SpriteOutline>();

        if (this.GetComponentInParent<Battle_Player>())
        {
            this.createActionMenu();
        }
    }

    protected abstract void StartMySelf();

    /** INPUT **/
    public void hasInputMouse(Boolean clicked)
    {
        if (clicked)
        {
            if (this.focused)
            {
                this.unfocus();
            }
            else
            {
                this.focus();
            }
        }
        else
        {
            this.unselect();
        }
    }

    /** INTERACTION **/
    public void focus()
    {
        this.selected = true;
        this.focused = true;
        if (this.outline)
            this.outline.enabled = true;
        if (this.actionMenu)
            this.updateActionMenu();
    }

    public void unfocus()
    {
        this.selected = false;
        this.focused = false;
        if (this.outline)
            this.outline.enabled = false;
        if (this.actionMenu)
            this.actionMenu.SetActive(false);
    }

    public void unselect()
    {
        this.selected = false;
    }

    /** GUI CREATOR **/
    protected abstract void createActionList();

    private void createActionMenu()
    {
        this.actionMenu = buttonObjectPool.GetObject();
        this.actionMenu.transform.SetParent(GameObject.Find("Battle_UI").gameObject.transform);

        this.actionMenu.GetComponent<RectTransform>().offsetMin = new Vector2(-100, -100);
        this.actionMenu.GetComponent<RectTransform>().offsetMax = new Vector2(100, 100);
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
        if (!this.actionMenu)
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

    public bool isSelected()
    {
        return this.selected;
    }

    public bool isFocused()
    {
        return this.focused;
    }
}