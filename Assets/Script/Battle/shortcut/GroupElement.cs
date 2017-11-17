using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class GroupElement : GuiElement
{
    private List<ActionMenuItem> customList = new List<ActionMenuItem>();

    public override void StartMyself()
    {
        Debug.Log("Start new group");
        this.buttonObjectPool = GameObject.Find("SimpleActionMenuPool").GetComponent<SimpleObjectPool>();
        this.createActionMenu();
    }

    protected override void createActionList()
    {

        this.actionList.RemoveRange(0, this.actionList.Count);
        Debug.Log("AAAAADDDD ACTION IN GROUP !!! : " + this.customList.Count);
        
        foreach (ActionMenuItem item in this.customList)
        {
            Debug.Log("try to add : " + item.actionName);
            if (!this.isInList(item))
            {
                Func<bool> func3 = () => executeAction(item.actionName);
                this.actionList.Add(new ActionMenuItem(item.actionName, new ActionMenuItem.ActionMenuDelegate(func3)));
            }
        }
    }
    
    public bool sayHI()
    {
        Debug.Log("ca marche!");
        return true;
    }

    public void initActions()
    {
        this.customList.RemoveRange(0, this.customList.Count);
    }

    public void addActionList(List<ActionMenuItem> actions)
    {
        Debug.Log("add action list: " + actions.Count);
        foreach (ActionMenuItem item in actions) {
            this.customList.Add(item);
        }
    }

    public bool executeAction(String name)
    {
        foreach (ActionMenuItem item in this.customList)
        {
            if (item.actionName == name)
            {
                item.actionDelegate();
            }
        }
        return true;
    }

    protected bool isInList(ActionMenuItem target)
    {
        foreach (ActionMenuItem item in this.actionList)
        {
            if (item.actionName == target.actionName)
            {
                Debug.Log("same action: " + item.actionName + ", " + target.actionName);
                return true;
            }
        }
        return false;
    }
}
