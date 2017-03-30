using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class ActionMenuItem
{
    public delegate bool ActionMenuDelegate();

    public ActionMenuDelegate actionDelegate;
    public string actionName;

    public ActionMenuItem(string name, ActionMenuDelegate action)
    {
        this.actionName = name;
        this.actionDelegate = action;
    }
}

public class ActionMenuList : MonoBehaviour
{

    public List<ActionMenuItem> itemList = new List<ActionMenuItem>();
    public Transform contentPanel;
    protected SimpleObjectPool buttonObjectPool = null;

    // Use this for initialization
    void Start()
    {
        /*
        buttonObjectPool = GameObject.Find("SimpleActionMenuItemPool").GetComponent<SimpleObjectPool>();
        RefreshDisplay();*/
    }

    void RefreshDisplay()
    {
        RemoveButtons();
        AddButtons();
    }

    public void init(List<ActionMenuItem> items)
    {
        this.itemList.AddRange(items);
        buttonObjectPool = GameObject.Find("SimpleActionMenuItemPool").GetComponent<SimpleObjectPool>();
    }

    public void update(List<ActionMenuItem> items)
    {
        this.itemList.RemoveRange(0, this.itemList.Count);
        this.itemList.AddRange(items);
        this.RefreshDisplay();
    }

    private void RemoveButtons()
    {
        while (contentPanel.childCount > 0)
        {
            print("remove");
            GameObject toRemove = transform.GetChild(0).gameObject;
            buttonObjectPool.ReturnObject(toRemove);
        }
    }

    private void AddButtons()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            print("add " + itemList[i].actionName);
            ActionMenuItem item = itemList[i];
            GameObject newButton = buttonObjectPool.GetObject();
            newButton.transform.SetParent(contentPanel);

            ActionMenuButton sampleButton = newButton.GetComponent<ActionMenuButton>();
            sampleButton.Setup(item, this);
        }
    }
}