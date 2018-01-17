using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

[Serializable]
public class Inventory
{
    public List<InventoryObject> food = new List<InventoryObject>();
    public List<InventoryObject> weapons = new List<InventoryObject>();

    public InventoryObject containsObject(InventoryObject obj)
    {
        InventoryObject ret = null;
        ret = food.Find((item) => item.id == obj.id);
        if (ret != null) return ret;
        ret = weapons.Find((item) => item.id == obj.id);
        return ret;
    }

    public InventoryObject containsObject(int id)
    {
        InventoryObject ret = null;
        ret = food.Find((item) => item.id == id);
        if (ret != null) return ret;
        ret = weapons.Find((item) => item.id == id);
        return ret;
    }
    public bool addObject(InventoryObject obj)
    {
        if (obj.quantity <= 0)
        {
            return false;
        }
        InventoryObject tmp = this.containsObject(obj);
        if (tmp != null)
        {
            tmp.quantity += 1;
        }
        else
        {
            if (obj.type == "Food")
            {
                this.food.Add(new InventoryObject(obj, 1));
            }
            if (obj.type == "Weapon")
            {
                this.weapons.Add(new InventoryObject(obj, 1));
            }
        }
        return true;
    }

    public bool addQuantityOfObject(InventoryObject obj, int quantity)
    {
        if (obj.quantity <= 0)
        {
            return false;
        }
        InventoryObject tmp = food.Find((item) => item.name == obj.name);
        if (tmp != null)
        {
            tmp.quantity += quantity;
        }
        else
        {
            if (obj.type == "Food")
            {
                this.food.Add(new InventoryObject(obj, quantity));
            }
            if (obj.type == "Weapon")
            {
                this.weapons.Add(new InventoryObject(obj, quantity));
            }
        }
        return true;
    }

    public void removeObject(InventoryObject obj)
    {
        obj.quantity -= 1;
        if (obj.type == "Food")
        {
            TrimList(this.food);
        } else
        {
            TrimList(this.weapons);
        }
    }

    public void removeQuantityOfObject(InventoryObject obj, int quantity)
    {
        obj.quantity -= quantity;
        if (obj.type == "Food")
        {
            TrimList(this.food);
        } else
        {
            TrimList(this.weapons);
        }
    }

    private void TrimList(List<InventoryObject> list)
    {
        List<InventoryObject> toDelete = new List<InventoryObject>();

        foreach (InventoryObject item in list)
        {
            if (item.quantity <= 0)
            {
                toDelete.Add(item);
            }
        }
        foreach (InventoryObject item in toDelete)
        {
            list.Remove(item);
        }
        toDelete.Clear();
    }
}