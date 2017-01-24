using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class PlayerManager {

    private static PlayerManager instance = null;

    private List<String> json = new List<string>();
    private Player player;

    protected PlayerManager()
    {
        //LoadFile("PlayerJson/Player.txt");
        //CreatePlayer();
        //Debug.Log("player : " + player.name + "/" + player.life);
        //LoadFile("PlayerJson/Inventory.txt");
        //CreateInventory();
        //Debug.Log("CHECK INVENTORY : " + player.inventory.food.Count + " / " + player.inventory.ammunition.Count);
        //LoadFile("PlayerJson/Crew.txt");
        //CreateCrew();
        //Debug.Log("CHECK CREW : " + player.crew.begos.Count + " / " + player.crew.captains.Count + " / " + player.crew.engineers.Count
        //    + " / " + player.crew.fastUnits.Count + " / " + player.crew.fighters.Count);

        LoadFile("PlayerJson/Player.txt");
        player = JsonUtility.FromJson<Player>(json[0]);
        Debug.Log("player : " + player.name + "/" + player.life);
        Debug.Log("CHECK INVENTORY : " + player.inventory.food.Count + " / " + player.inventory.ammunition.Count);
        Debug.Log("CHECK CREW : " + player.crew.begos.Count + " / " + player.crew.captains.Count + " / " + player.crew.engineers.Count
            + " / " + player.crew.fastUnits.Count + " / " + player.crew.fighters.Count);
    }

    public static PlayerManager GetInstance()
    {
        if (instance == null)
        {
            instance = new PlayerManager();
        }
        return instance;
    }

	//// Use this for initialization
	//void Start () {
 //       instance = this;
        
	//}
	
	//// Update is called once per frame
	//void Update () {
	
	//}

    public bool WriteToFile()
    {
        try
        {
            StreamWriter writer = new StreamWriter("PlayerJson/test.txt", false);
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

    public void CreatePlayer ()
    {
        for (int i = 0; i < json.Count; ++i)
        {
            player = JsonUtility.FromJson<Player>(json[i]);
        }
    }

    public void CreateInventory()
    {
        for (int i = 0; i < json.Count; ++i)
        {
            InventoryObject tmp = JsonUtility.FromJson<InventoryObject>(json[i]);
            if (tmp.type == "Food")
            {
                player.inventory.food.Add(tmp);
            } else if (tmp.type == "Ammunition")
            {
                player.inventory.ammunition.Add(tmp);
            }
        }
    }

    public void CreateCrew()
    {
        for (int i = 0; i < json.Count; ++i)
        {
            CrewMember tmp = JsonUtility.FromJson<CrewMember>(json[i]);
            Debug.Log(tmp.type);
            if (tmp.type == "Bego")
            {
                player.crew.begos.Add(new CrewMember_Bego(tmp));
            }
            else if (tmp.type == "Captain")
            {
                player.crew.captains.Add(new CrewMember_Captain());
            }
            else if (tmp.type == "Engineer")
            {
                player.crew.engineers.Add(new CrewMember_Engineer());
            }
            else if (tmp.type == "FastUnit")
            {
                player.crew.fastUnits.Add(new CrewMember_FastUnit());
            }
            else if (tmp.type == "Fighter")
            {
                player.crew.fighters.Add(new CrewMember_Fighter());
            }
        }
    }
}

[Serializable]
public class Player
{
    public string name;
    public int life;

    //[NonSerialized]
    public PlayerInventory inventory = new PlayerInventory();
    //[NonSerialized]
    public PlayerCrew crew = new PlayerCrew();
}

[Serializable]
public class PlayerInventory
{
    public List<InventoryObject> food = new List<InventoryObject>();
    public List<InventoryObject> ammunition = new List<InventoryObject>();
}

[Serializable]
public class PlayerCrew
{
    public List<CrewMember> begos = new List<CrewMember>();
    public List<CrewMember> captains = new List<CrewMember>();
    public List<CrewMember> engineers = new List<CrewMember>();
    public List<CrewMember> fastUnits = new List<CrewMember>();
    public List<CrewMember> fighters = new List<CrewMember>();
}

[Serializable]
public class InventoryObject
{
    public string name;
    public string type;
    public int number;
}
