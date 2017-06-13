﻿using UnityEngine;
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

    private List<InventoryObject> objects = new List<InventoryObject>();

    public void GenerateObjects()
    {
        objects.Add(new InventoryObject("Coconut", "Food", 1));
        objects.Add(new InventoryObject("Ruhm", "Food", 1));
        objects.Add(new InventoryObject("Banana", "Food", 1));
        objects.Add(new InventoryObject("Fish", "Food", 1));
        objects.Add(new InventoryObject("Beef", "Food", 1));
        objects.Add(new InventoryObject("Biscuit", "Food", 1));
        objects.Add(new InventoryObject("Apple", "Food", 1));
        objects.Add(new InventoryObject("Wine", "Food", 1));
        objects.Add(new InventoryObject("Olives", "Food", 1));
        objects.Add(new InventoryObject("Water", "Food", 1));
        objects.Add(new InventoryObject("Bread", "Food", 1));
        objects.Add(new InventoryObject("Coffee", "Food", 1));
        objects.Add(new InventoryObject("Tea", "Food", 1));
    }

    public void GenerateIsland(Island island)
    {
        island.inventory = new IslandInventory();
        System.Random rng = new System.Random();
        GenerateFood(rng, island);
        GenerateWeapons(rng, island);
    }

    void GenerateFood(System.Random rng, Island island)
    {
        for (int i = 0; i < 5; ++i)
        {
            int a = rng.Next(1, 14);
            int b = rng.Next(10, 101);
            objects[a].number = b;
            //Debug.Log("obj gen : " + objects[a].name + " " + objects[a].number);
            if (CheckingDouble(objects[a], island))
            {
                island.inventory.food.Add(new InventoryObject(objects[a]));
            }
        }
    }

    void GenerateWeapons(System.Random rng, Island island)
    {
        int a = rng.Next(20, 101);
        island.inventory.weapons.Add(new InventoryObject("Black powder", "weapon", a));
        a = rng.Next(20, 101);
        island.inventory.weapons.Add(new InventoryObject("Canon ball", "weapon", a));
        a = rng.Next(20, 101);
        island.inventory.weapons.Add(new InventoryObject("Shrapnel", "weapon", a));
        a = rng.Next(20, 101);
        island.inventory.weapons.Add(new InventoryObject("Sabre", "weapon", a));
        a = rng.Next(20, 101);
        island.inventory.weapons.Add(new InventoryObject("Musket", "weapon", a));
        a = rng.Next(20, 101);
        island.inventory.weapons.Add(new InventoryObject("Rifle", "weapon", a));
        a = rng.Next(20, 101);
        island.inventory.weapons.Add(new InventoryObject("Canon", "weapon", a));
        a = rng.Next(20, 101);
        island.inventory.weapons.Add(new InventoryObject("Bullet", "weapon", a));
        a = rng.Next(20, 101);
        island.inventory.weapons.Add(new InventoryObject("Graplin hooks", "weapon", a));
    }

    public void generateQuests(System.Random rng, Island island)
    {
        JsonUtility.FromJson<Island>(json[0]);
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
