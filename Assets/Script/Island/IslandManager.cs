﻿using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class IslandManager {

    private static IslandManager instance = null;

    private List<String> json = new List<string>();

    [Serializable]
    public class IslandsSave
    {
        public List<Island> islands = new List<Island>();
    }

    public IslandsSave islandsSave = new IslandsSave();
    public List<Island> islands;

    protected IslandManager(int islandAmount)
    {
        //LoadFile("PlayerJson/IslandSave.txt");
        //island = JsonUtility.FromJson<Island>(json[0]);

        islands = islandsSave.islands;
        for (int i = 0; i < islandAmount; i += 1)
        {
            islands.Add(new Island());
        }

        IslandGenerator iGen = new IslandGenerator();
        Economy eco = new Economy();
        for (int i = 0; i < islands.Count; ++i)
        {
            islands[i] = iGen.GenerateIsland(islands[i]);
            eco.initInventoryPrices(islands[i].inventory);
        }
    }

    public static IslandManager GetInstance(int islandAmount = 9)
    {
        if (instance == null)
        {
            instance = new IslandManager(islandAmount);
        }
        return instance;
    }

    public bool Save()
    {
        try
        {
            StreamWriter writer = new StreamWriter("PlayerJson/IslandSave.txt", false);
            writer.Write(JsonUtility.ToJson(islandsSave));
            writer.Close();
        } catch (Exception e)
        {
            Debug.Log("SAVE ISLAND : " + e.Message);
            return false;
        }
        return true;
    }

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
}

[Serializable]
public class Island
{
    public string name;
    public float x;
    public float y;
    public int influence;
    public IslandInventory inventory = new IslandInventory();
    public List<CrewMember> crew = new List<CrewMember>();
    public QuestLog questLog = new QuestLog();


    public void removeCrewMember(CrewMember member)
    {
        crew.Remove(member);
    }
}

[Serializable]
public class IslandInventory: Inventory
{
}

