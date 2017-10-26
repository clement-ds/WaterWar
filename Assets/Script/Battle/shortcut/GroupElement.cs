using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class GroupElement : GuiElement
{
    private List<ActionMenuItem> customList;

    public override void StartMyself()
    {
    }

    protected override void createActionList()
    {
        this.actionList.Add(new ActionMenuItem("SAY HI", sayHI));
        /*
        foreach (ActionMenuItem item in this.customList)
        {
            if (!this.isInList(item))
            {
                Func<bool> func3 = () => executeAction(item.actionName);
                this.actionList.Add(new ActionMenuItem(item.actionName, new ActionMenuItem.ActionMenuDelegate(func3)));
            }
        }*/
    }

    public bool sayHI()
    {
        Debug.Log("ca marche!");
        return true;
    }

    public void addActionList(List<ActionMenuItem> actionList)
    {
        foreach (ActionMenuItem item in this.actionList) {
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
                return true;
            }
        }
        return false;
    }
}
