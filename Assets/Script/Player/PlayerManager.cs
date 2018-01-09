using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class PlayerManager {

    private static PlayerManager instance = null;

    private List<String> json = new List<string>();
    private List<String> objectDictionary = new List<string>();
    public Player player;
    public Player ai;

    protected PlayerManager()
    {
        LoadFile("PlayerJson/Save.json");
        player = JsonUtility.FromJson<Player>(json[0]);
        Debug.Log("player : " + player.name + "/" + player.life);
        //Debug.Log("CHECK INVENTORY : " + player.inventory.food.Count + " / " + player.inventory.weapons.Count);
        //Debug.Log("CREW : ");
        //foreach (CrewMember member in player.crew.crewMembers)
        //    Debug.Log("--- " + member.type + " = " + member.id);
        //Debug.Log("CHECK QUEST : " + player.questLog.quests.Count);
        //Save();

        LoadFile("PlayerJson/AISave.json");
        ai = JsonUtility.FromJson<Player>(json[0]);

        LoadFile("PlayerJson/Objects.txt", objectDictionary);
    }

    public static PlayerManager GetInstance()
    {
        if (instance == null)
        {
            instance = new PlayerManager();
        }
        return instance;
    }

    public string getNameForObjectId(int id) {
        return JsonUtility.FromJson<InventoryObject>(objectDictionary[id]).name;
    }

    public void AcceptQuest(PlayerQuest quest) {
        quest.taken = true;
        player.questLog.quests.Add(quest);
        IslandManager.GetInstance().islands[player.currentIsland].questLog.quests.Remove(quest);
    }
    
    public List<PlayerQuest> GetQuest() {
        return player.questLog.quests;
    }

    public bool Save()
    {
        try
        {
            StreamWriter writer = new StreamWriter("PlayerJson/Save.json", false);
            writer.Write(JsonUtility.ToJson(player));
            writer.Close();
        } catch (Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
        return true;
    }

    public bool SaveAI()
    {
        try
        {
            StreamWriter writer = new StreamWriter("PlayerJson/AISave.json", false);
            writer.Write(JsonUtility.ToJson(ai));
            writer.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
        return true;
    }

    private bool LoadFile(string fileName, List<string> json) {
        try {
        string line;
        StreamReader theReader = new StreamReader(fileName, Encoding.Default);
        using (theReader) {
            do {
            line = theReader.ReadLine();
            if (line != null) {
                json.Add(line);
            }
            } while (line != null);
            theReader.Close();
            return true;
        }
        }
        catch (Exception e) {
        Debug.Log(e.Message);
        return false;
        }
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
    public Vector2 mapPosition;

    public PlayerInventory inventory = new PlayerInventory();
    public PlayerCrew crew = new PlayerCrew();
    public QuestLog questLog = new QuestLog();
    public PlayerShip ship = new PlayerShip();
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
    public List<CrewMember> crewMembers = new List<CrewMember>();
    public long crewIncrement = 0;

    public PlayerCrew()
    {
        AddCrew("Captain");
        AddCrew("Bego");
        AddCrew("Fighter");

    }

    public void AddCrew(CrewMember member)
    {
        crewMembers.Add(member);
    }

    public void AddCrew(string type)
    {
        string id = "CrewMember_" + type;
        string crewID = type + crewIncrement.ToString();
        if (id.Equals("CrewMember_Captain"))
            crewMembers.Add(new CrewMember_Captain(crewID));
        else if (id.Equals("CrewMember_Bego"))
            crewMembers.Add(new CrewMember_Bego(crewID));
        else if (id.Equals("CrewMember_Engineer"))
            crewMembers.Add(new CrewMember_Engineer(crewID));
        else if (id.Equals("CrewMember_FastUnit"))
            crewMembers.Add(new CrewMember_FastUnit(crewID));
        else if (id.Equals("CrewMember_Fighter"))
            crewMembers.Add(new CrewMember_Fighter(crewID));
        else
            Debug.Log("Invalid crew type: " + id);
        ++crewIncrement;
    }

    public void RemoveCrew(string id)
    {
        foreach (CrewMember member in crewMembers)
        {
            if (member.id.Equals(id))
            {
                crewMembers.Remove(member);
                break;
            }
        }
    }

}

[Serializable]
public class InventoryObject
{
    public InventoryObject(string name, string type, int number, int price, int basePrice)
    {
        this.name = name;
        this.type = type;
        this.quantity = number;
        this.price = price;
        this.basePrice = basePrice;
    }

    public InventoryObject(InventoryObject invObj)
    {
        this.name = invObj.name;
        this.type = invObj.type;
        this.quantity = invObj.quantity;
        this.price = invObj.price;
        this.basePrice = invObj.basePrice;
    }

    public string name;
    public string type;
    public int quantity;
    public int price;
    public int id;
    public bool isRareItem;
    public int basePrice;
}

[Serializable]
public class QuestLog
{
    public List<PlayerQuest> quests = new List<PlayerQuest>();
}

[Serializable]
public class EndQuest {
    public int type;
    public int enemyType;
    public int enemyId;
}

[Serializable]
public class Reward {
    public enum REWARD {MONEY = 0, OBJECT = 1, INFLUENCE = 2};  
    public REWARD type;
    public int id;
    public int amount;
}

[Serializable]
public class PlayerQuest
{
    public enum QUEST {KILL = 0, FIND, GET, RECRUIT, SACK, MORAL, PRINCIPAL, INFLUENCE, DESTROY};  

    public string description;
    public string title;
    public QUEST type;
    public string objective;
    public Reward reward;
    public InventoryObject end;
    public int moneyReward;
    public bool taken = false;

    public String Describe() {
        return ("TITLE: " + title + "\tDESCRIPTION: " + description + "\tTYPE: " + type + "\tOBJECTIVE: " + objective + "\tREWARD: " + reward.id + ':' + reward.amount + ':' + reward.type);
    }
}

[Serializable]
public class PlayerShip
{
    public String type;
    public ShipDisposition shipDisposition;
}

[Serializable]
public class ShipDisposition
{
    public List<Room> rooms = new List<Room>();

    public void ChangeRoom(int index, string newtype)
    {
        rooms[index].component = newtype;
    }
}

[Serializable]
public class Room
{
    public String id;
    public String type;
    public String component;
    public List<String> links;
    public float x;
    public float y;
    public float width;
    public float height;
    public float rotation;
}