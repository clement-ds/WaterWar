using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class IslandManager {

    private static IslandManager instance = null;

    private List<String> json = new List<string>();
    public List<Island> islands = new List<Island>();
    public Island island;
    public Island island2;
    public Island island3;
    public Island island4;
    public Island island5;
    public Island island6;
    public Island island7;
    public Island island8;
    public Island island9;

    protected IslandManager()
    {
        //LoadFile("PlayerJson/IslandSave.txt");
        //island = JsonUtility.FromJson<Island>(json[0]);
        //island.x = 5;
        //island.y = 9;
        islands.Add(new Island());
        islands.Add(new Island());
        islands.Add(new Island());
        islands.Add(new Island());
        islands.Add(new Island());
        islands.Add(new Island());
        islands.Add(new Island());
        islands.Add(new Island());
        islands.Add(new Island());

        IslandGenerator iGen = new IslandGenerator();
        Economy eco = new Economy();
        for (int i = 0; i < islands.Count; ++i)
        {
            islands[i] = iGen.GenerateIsland(islands[i]);
            eco.setInventoryPrices(islands[i]);
        }

        Debug.Log("island : " + islands[0].name);
        Debug.Log("CHECK INVENTORY : " + islands[0].inventory.food.Count + " / " + islands[0].inventory.weapons.Count);
//        Debug.Log("CHECK CREW : " + islands[0].crew.begos.Count + " / " + islands[0].crew.captains.Count + " / " + islands[0].crew.engineers.Count
//            + " / " + islands[0].crew.fastUnits.Count + " / " + islands[0].crew.fighters.Count);
        Debug.Log("CHECK QUEST : " + islands[0].questLog.quests.Count);

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
    public float x;
    public float y;

    public IslandInventory inventory = new IslandInventory();
    public List<CrewMember> crew = new List<CrewMember>();
    public QuestLog questLog = new QuestLog();


    public void removeCrewMember(CrewMember member)
    {
        crew.Remove(member);
    }
}

[Serializable]
public class IslandInventory
{
    public List<InventoryObject> food = new List<InventoryObject>();
    public List<InventoryObject> weapons = new List<InventoryObject>();
}
