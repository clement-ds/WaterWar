using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI
{
    IslandManager im = IslandManager.GetInstance();
    int maxTradeTransaction = 3;
    Economy eco = new Economy();

    public void enemyTurn(Player enemy)
    {
        updateEnemyPosition(enemy);
        updateEnemyTrade(enemy);
    }

    void updateEnemyPosition(Player enemy)
    {
        int islandIndex;
        do
        {
            islandIndex = UnityEngine.Random.Range(0, im.islands.Count);
        } while (islandIndex == enemy.currentIsland);
        enemy.currentIsland = islandIndex;
    }

    void updateEnemyTrade(Player enemy)
    {
        enemySales(enemy);
        enemyBuys(enemy);
    }

    void enemySales(Player enemy)
    {
        int tradeTransaction = 0;
        bool transactionFlag = false;
        Island island = im.islands[enemy.currentIsland];
        for (int i = 0; i < enemy.inventory.food.Count && tradeTransaction < maxTradeTransaction; i += 1)
        {
            while (i < enemy.inventory.food.Count && enemy.inventory.food[i].price >= enemy.inventory.food[i].basePrice && enemy.inventory.food[i].quantity > 0)
            {
                island.inventory.addObject(enemy.inventory.food[i]);
                enemy.inventory.removeObject(enemy.inventory.food[i]);
                eco.setInventoryPrices(island.inventory, enemy.inventory);
                transactionFlag = true;
            }
            tradeTransaction = transactionFlag ? tradeTransaction + 1 : tradeTransaction;
            transactionFlag = false;
        }
        for (int i = 0; i < enemy.inventory.weapons.Count && tradeTransaction < maxTradeTransaction; i += 1)
        {
            while (i < enemy.inventory.weapons.Count && enemy.inventory.weapons[i].price >= enemy.inventory.weapons[i].basePrice && enemy.inventory.weapons[i].quantity > 0)
            {
                island.inventory.addObject(enemy.inventory.weapons[i]);
                enemy.inventory.removeObject(enemy.inventory.weapons[i]);
                eco.setInventoryPrices(island.inventory, enemy.inventory);
                transactionFlag = true;
            }
            tradeTransaction = transactionFlag ? tradeTransaction + 1 : tradeTransaction;
            transactionFlag = false;
        }
    }

    void enemyBuys(Player enemy)
    {
        int tradeTransaction = 0;
        bool transactionFlag = false;
        Island island = im.islands[enemy.currentIsland];
        for (int i = 0; i < island.inventory.food.Count && tradeTransaction < maxTradeTransaction; i += 1)
        {
            while (i < island.inventory.food.Count && island.inventory.food[i].price <= island.inventory.food[i].basePrice && island.inventory.food[i].quantity > 0)
            {
                enemy.inventory.addObject(island.inventory.food[i]);
                island.inventory.removeObject(island.inventory.food[i]);
                eco.setInventoryPrices(island.inventory, enemy.inventory);
                transactionFlag = true;
            }
            tradeTransaction = transactionFlag ? tradeTransaction + 1 : tradeTransaction;
            transactionFlag = false;
        }
        for (int i = 0; i < island.inventory.weapons.Count && tradeTransaction < maxTradeTransaction; i += 1)
        {
            while (i < island.inventory.weapons.Count && island.inventory.weapons[i].price <= island.inventory.weapons[i].basePrice && island.inventory.weapons[i].quantity > 0)
            {
                enemy.inventory.addObject(island.inventory.weapons[i]);
                island.inventory.removeObject(island.inventory.weapons[i]);
                eco.setInventoryPrices(island.inventory, enemy.inventory);
                transactionFlag = true;
            }
            tradeTransaction = transactionFlag ? tradeTransaction + 1 : tradeTransaction;
            transactionFlag = false;
        }
    }
}
