using UnityEngine;
using System.Collections;

public class Economy
{

    public void generateRareItems()
    {

    }

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
