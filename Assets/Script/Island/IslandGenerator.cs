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
        island = new Island();
        //island.questLog = new QuestLog();
        //island.inventory = new IslandInventory();
        //island.crew = new IslandCrew();
        System.Random rng = new System.Random();
        GenerateFood(rng, island);
        GenerateWeapons(rng, island);
        GenerateQuests(rng, island);
        GenerateCrew(rng, island);
        GenerateName(island);
        return island;
    }

    private void GenerateFood(System.Random rng, Island island)
    {
        LoadFile("PlayerJson/Food.txt");
        for (int i = 0; i < 5; ++i)
        {
            int a = rng.Next(0, 12);
            int b = rng.Next(10, 101);
            InventoryObject obj = JsonUtility.FromJson<InventoryObject>(json[a]);
            obj.quantity = b;
            if (CheckingDouble(obj, island))
            {
                island.inventory.food.Add(obj);
            }
        }
    }

    private void GenerateWeapons(System.Random rng, Island island)
    {
        int a = rng.Next(20, 101);
        island.inventory.weapons.Add(new InventoryObject("Black powder", "Weapon", a, 10));
        a = rng.Next(20, 101);
        island.inventory.weapons.Add(new InventoryObject("Canon ball", "Weapon", a, 10));
        a = rng.Next(20, 101);
        island.inventory.weapons.Add(new InventoryObject("Shrapnel", "Weapon", a, 10));
        a = rng.Next(20, 101);
        island.inventory.weapons.Add(new InventoryObject("Sabre", "Weapon", a, 10));
        a = rng.Next(20, 101);
        island.inventory.weapons.Add(new InventoryObject("Musket", "Weapon", a, 10));
        a = rng.Next(20, 101);
        island.inventory.weapons.Add(new InventoryObject("Rifle", "Weapon", a, 10));
        a = rng.Next(20, 101);
        island.inventory.weapons.Add(new InventoryObject("Canon", "Weapon", a, 10));
        a = rng.Next(20, 101);
        island.inventory.weapons.Add(new InventoryObject("Bullet", "Weapon", a, 10));
        a = rng.Next(20, 101);
        island.inventory.weapons.Add(new InventoryObject("Graplin hooks", "Weapon", a, 10));
    }

    private void GenerateQuests(System.Random rng, Island island)
    {
        LoadFile("PlayerJson/Quests.txt");
        int i = rng.Next(0, 5);
        PlayerQuest quest = JsonUtility.FromJson<PlayerQuest>(json[i]);
        island.questLog.quests.Add(quest);
    }

    private void GenerateCrew(System.Random rng, Island island)
    {
        int a = rng.Next(0, 10);
        for (int i = 0; i < a; ++i)
        {
            island.crew.Add(new CrewMember_Bego(island.name + "Bego" + i));
        }
        a = rng.Next(0, 5);
        for (int i = 0; i < a; ++i)
        {
            island.crew.Add(new CrewMember_Fighter(island.name + "Fighter" + i));
        }
        a = rng.Next(0, 5);
        for (int i = 0; i < a; ++i)
        {
            island.crew.Add(new CrewMember_Engineer(island.name + "Engineer" + i));
        }
        a = rng.Next(0, 5);
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
