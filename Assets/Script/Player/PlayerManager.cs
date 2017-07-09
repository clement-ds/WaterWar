using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class PlayerManager {

    private static PlayerManager instance = null;

    private List<String> json = new List<string>();
    public Player player;

    protected PlayerManager()
    {
        LoadFile("PlayerJson/Save.txt");
        player = JsonUtility.FromJson<Player>(json[0]);
        Debug.Log("player : " + player.name + "/" + player.life);
        Debug.Log("CHECK INVENTORY : " + player.inventory.food.Count + " / " + player.inventory.weapons.Count);
        Debug.Log("CHECK CREW : " + player.crew.begos.Count + " / " + player.crew.captains.Count + " / " + player.crew.engineers.Count
            + " / " + player.crew.fastUnits.Count + " / " + player.crew.fighters.Count);
        Debug.Log("CHECK QUEST : " + player.questLog.quests.Count);
        //Save();
    }

    public static PlayerManager GetInstance()
    {
        if (instance == null)
        {
            instance = new PlayerManager();
        }
        return instance;
    }

    public bool Save()
    {
        try
        {
            StreamWriter writer = new StreamWriter("PlayerJson/Save.txt", false);
            writer.Write(JsonUtility.ToJson(player));
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
public class Player
{
    public string name;
    public int life;
    public int money;
    public int currentIsland;

    public PlayerInventory inventory = new PlayerInventory();
    public PlayerCrew crew = new PlayerCrew();
    public QuestLog questLog = new QuestLog();
}

[Serializable]
public class PlayerInventory
{
    public List<InventoryObject> food = new List<InventoryObject>();
    public List<InventoryObject> weapons = new List<InventoryObject>();
}

[Serializable]
public class PlayerCrew
{
    public List<CrewMember_Bego> begos = new List<CrewMember_Bego>();
    public List<CrewMember_Captain> captains = new List<CrewMember_Captain>();
    public List<CrewMember_Engineer> engineers = new List<CrewMember_Engineer>();
    public List<CrewMember_FastUnit> fastUnits = new List<CrewMember_FastUnit>();
    public List<CrewMember_Fighter> fighters = new List<CrewMember_Fighter>();
}

[Serializable]
public class InventoryObject
{
    public InventoryObject(string name, string type, int number, int price)
    {
        this.name = name;
        this.type = type;
        this.quantity = number;
        this.price = price;
    }

    public InventoryObject(InventoryObject invObj)
    {
        this.name = invObj.name;
        this.type = invObj.type;
        this.quantity = invObj.quantity;
        this.price = invObj.price;
    }

    public string name;
    public string type;
    public int quantity;
    public int price;
}

[Serializable]
public class QuestLog
{
    public List<PlayerQuest> quests = new List<PlayerQuest>();
}

[Serializable]
public class PlayerQuest
{
    public string description;
    public string objective;
    public InventoryObject reward;
    public int moneyReward;
}
