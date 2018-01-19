using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class IslandGenerator {

    /* 
     * - Coconut
     * - Ruhm
     * - Banana
     * - Fish
     * - Beef
     * - Biscuit
     * - Apple
     * - Wine
     * - Olives
     * - Water
     * - Bread
     * - Coffee
     * - Tea
     * 
     * - Powder
     * - Canon ball
     * - Shrapnel
     * - Sabre
     * - Muskets
     * - Rifles
     * - Graplin hooks
    */

    private List<String> json = new List<string>();

    private bool LoadFile(string fileName)
    {
        json = new List<string>();
        try
        {
            string line;

            StreamReader theReader = new StreamReader(fileName, Encoding.Default);

            using (theReader)
            {
                do
                {
                    line = theReader.ReadLine();

                    if (line != null)
                    {
                        json.Add(line);
                    }
                }
                while (line != null);
                theReader.Close();
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
    }

    public Island GenerateIsland(Island island)
    {
        Debug.Log("TEST-------------: " + island.name);
        island = new Island();
        //island.questLog = new QuestLog();
        //island.inventory = new IslandInventory();
        //island.crew = new IslandCrew();
        GenerateFood(island);
        GenerateName(island);
        GenerateWeapons(island);
        GenerateCrew(island);
        GenerateQuests(island);
        island.influence = 50;
        Debug.Log("GENERATE: " + island.name);
        return island;
    }

    private void GenerateFood(Island island)
    {
        LoadFile("PlayerJson/Food.txt");
        for (int i = 0; i < 5; ++i)
        {
            int tmp = UnityEngine.Random.Range(0, 15);
            InventoryObject obj = JsonUtility.FromJson<InventoryObject>(json[tmp]);
            obj.quantity = UnityEngine.Random.Range(10, 101);
            if (CheckingDouble(obj, island))
            {
                island.inventory.food.Add(obj);
            }
        }
    }

    public void GenerateFood(Island island, int amount)
    {
        LoadFile("PlayerJson/Food.txt");
        for (int i = 0; i < amount; i += 1)
        {
            int tmp = UnityEngine.Random.Range(0, 15);
            InventoryObject obj = JsonUtility.FromJson<InventoryObject>(json[tmp]);
            island.inventory.addQuantityOfObject(obj, UnityEngine.Random.Range(10, 31));
        }
    }

    private void GenerateWeapons(Island island)
    {
        int a = UnityEngine.Random.Range(20, 101);
        island.inventory.weapons.Add(new InventoryObject("Black powder", "Weapon", a, 10, 10));
        a = UnityEngine.Random.Range(20, 101);
        island.inventory.weapons.Add(new InventoryObject("Canon ball", "Weapon", a, 10, 10));
        a = UnityEngine.Random.Range(20, 101);
        island.inventory.weapons.Add(new InventoryObject("Shrapnel", "Weapon", a, 10, 10));
        a = UnityEngine.Random.Range(20, 101);
        island.inventory.weapons.Add(new InventoryObject("Sabre", "Weapon", a, 10, 10));
        a = UnityEngine.Random.Range(20, 101);
        island.inventory.weapons.Add(new InventoryObject("Musket", "Weapon", a, 10, 10));
        a = UnityEngine.Random.Range(20, 101);
        island.inventory.weapons.Add(new InventoryObject("Rifle", "Weapon", a, 10, 10));
        a = UnityEngine.Random.Range(20, 101);
        island.inventory.weapons.Add(new InventoryObject("Canon", "Weapon", a, 10, 10));
        a = UnityEngine.Random.Range(20, 101);
        island.inventory.weapons.Add(new InventoryObject("Bullet", "Weapon", a, 10, 10));
        a = UnityEngine.Random.Range(20, 101);
        island.inventory.weapons.Add(new InventoryObject("Graplin hooks", "Weapon", a, 10, 10));
    }

    public void GenerateWeapons(Island island, int amount)
    {

    }

    private void GenerateQuests(Island island)
    {
        int a = UnityEngine.Random.Range(2, 6);
        QuestGenerator questGen = new QuestGenerator();
        for (int i = 0; i < a; ++i) {
            island.questLog.quests.Add(questGen.GenerateQuest(island));
        }
    }

    private void GenerateCrew(Island island)
    {
        int a = UnityEngine.Random.Range(0, 10);
        for (int i = 0; i < a; ++i)
        {
            island.crew.Add(new CrewMember_Bego(island.name + "Bego" + i));
        }
        a = UnityEngine.Random.Range(0, 5);
        for (int i = 0; i < a; ++i)
        {
            island.crew.Add(new CrewMember_Fighter(island.name + "Fighter" + i));
        }
        a = UnityEngine.Random.Range(0, 5);
        for (int i = 0; i < a; ++i)
        {
            island.crew.Add(new CrewMember_Engineer(island.name + "Engineer" + i));
        }
        a = UnityEngine.Random.Range(0, 5);
        for (int i = 0; i < a; ++i)
        {
            island.crew.Add(new CrewMember_FastUnit(island.name + "FastUnit" + i));
        }
    }

    void GenerateName(Island island)
    {
        int max = 0;
        for (int i = 0; i < island.inventory.food.Count; ++i)
        {
            if (island.inventory.food[i].quantity >= max)
            {
                island.name = island.inventory.food[i].name + " Island";
                max = island.inventory.food[i].quantity;
            }
        }
    }

    bool CheckingDouble(InventoryObject obj, Island island)
    {
        for (int i = 0; i < island.inventory.food.Count; ++i)
        {
            if (obj.name == island.inventory.food[i].name)
            {
                return false;
            }
        }
        return true;
    }
}
