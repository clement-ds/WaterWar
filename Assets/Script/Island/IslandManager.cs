using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class IslandManager {

    private static IslandManager instance = null;

    private List<String> json = new List<string>();
    public Island island;

    protected IslandManager()
    {
        LoadFile("PlayerJson/IslandSave.txt");
        island = JsonUtility.FromJson<Island>(json[0]);
        island.x = 5;
        island.y = 9;
        //Debug.Log("island : " + island.name);
        //Debug.Log("CHECK INVENTORY : " + island.inventory.food.Count + " / " + island.inventory.weapons.Count);
        //Debug.Log("CHECK CREW : " + island.crew.begos.Count + " / " + island.crew.captains.Count + " / " + island.crew.engineers.Count
        //    + " / " + island.crew.fastUnits.Count + " / " + island.crew.fighters.Count);
        //Debug.Log("CHECK QUEST : " + island.questLog.quests.Count);
        IslandGenerator iGen = new IslandGenerator();
        iGen.GenerateIsland(island);
        Save();
    }

    public static IslandManager GetInstance()
    {
        if (instance == null)
        {
            instance = new IslandManager();
        }
        return instance;
    }

    public bool Save()
    {
        try
        {
            StreamWriter writer = new StreamWriter("PlayerJson/IslandSave.txt", false);
            writer.Write(JsonUtility.ToJson(island));
            writer.Close();
        } catch (Exception e)
        {
            Debug.Log(e.Message);
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
    public int x;
    public int y;

    public IslandInventory inventory = new IslandInventory();
    public IslandCrew crew = new IslandCrew();
    public QuestLog questLog = new QuestLog();
}

[Serializable]
public class IslandInventory
{
    public List<InventoryObject> food = new List<InventoryObject>();
    public List<InventoryObject> weapons = new List<InventoryObject>();
}

[Serializable]
public class IslandCrew
{
    public List<CrewMember_Bego> begos = new List<CrewMember_Bego>();
    public List<CrewMember_Captain> captains = new List<CrewMember_Captain>();
    public List<CrewMember_Engineer> engineers = new List<CrewMember_Engineer>();
    public List<CrewMember_FastUnit> fastUnits = new List<CrewMember_FastUnit>();
    public List<CrewMember_Fighter> fighters = new List<CrewMember_Fighter>();
}


