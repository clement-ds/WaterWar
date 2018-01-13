using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Economy
{
    public void initInventoryPrices(IslandInventory island)
    {
        foreach (InventoryObject weapon in island.weapons)
        {
            weapon.price = weapon.basePrice + (weapon.quantity - 50) / -10;
        }
        foreach (InventoryObject food in island.food)
        {
            food.price = food.basePrice + (food.quantity - 50) / -10;
        }
    }

    public void setInventoryPrices(IslandInventory island, PlayerInventory player)
    {
        foreach (InventoryObject weapon in island.weapons)
        {
            weapon.price = weapon.basePrice + (weapon.quantity - 50) / -10;
        }
        foreach (InventoryObject food in island.food)
        {
            food.price = food.basePrice + (food.quantity - 50) / -10;
        }
        foreach (InventoryObject pWeapon in player.weapons)
        {
            pWeapon.price = pWeapon.basePrice + 5;
            foreach (InventoryObject weapon in island.weapons)
            {
                if (weapon.name == pWeapon.name)
                {
                    pWeapon.price = weapon.price;
                    break;
                }
            }
        }
        foreach (InventoryObject pFood in player.food)
        {
            pFood.price = pFood.basePrice + 5;
            foreach (InventoryObject food in island.food)
            {
                if (food.name == pFood.name)
                {
                    pFood.price = food.price;
                    break;
                }
            }
        }
    }
}
