using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Economy
{
    public void setInventoryPrices(Island island)
    {
        foreach (InventoryObject food in island.inventory.food)
        {
            food.price = food.price + (food.quantity - 50) / -10;
        }
        foreach (InventoryObject weapon in island.inventory.weapons)
        {
            weapon.price = weapon.price + (weapon.quantity - 50) / -10;
        }
    }
}
