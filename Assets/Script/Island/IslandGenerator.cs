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
        // Handle any problems that might arise when reading the text
        try
        {
            string line;
            // Create a new StreamReader, tell it which file to read and what encoding the file
            // was saved as
            StreamReader theReader = new StreamReader(fileName, Encoding.Default);
            // Immediately clean up the reader after this block of code is done.
            // You generally use the "using" statement for potentially memory-intensive objects
            // instead of relying on garbage collection.
            // (Do not confuse this with the using directive for namespace at the 
            // beginning of a class!)
            using (theReader)
            {
                // While there's lines left in the text file, do this:
                do
                {
                    line = theReader.ReadLine();

                    if (line != null)
                    {
                        // Do whatever you need to do with the text line, it's a string now
                        // In this example, I split it into arguments based on comma
                        // deliniators, then send that array to DoStuff()
                        json.Add(line);
                    }
                }
                while (line != null);
                // Done reading, close the reader and return true to broadcast success    
                theReader.Close();
                return true;
            }
        }
        // If anything broke in the try block, we throw an exception with information
        // on what didn't work
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
        int a = UnityEngine.Random.Range(0, 10);
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
