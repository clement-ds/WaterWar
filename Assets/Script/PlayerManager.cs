using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class PlayerManager : MonoBehaviour {

    private List<String> json = new List<string>();
    private Player player;

	// Use this for initialization
	void Start () {
        LoadFile("PlayerJson/Player.txt");
        CreatePlayer();
        print("player : " + player.name + "/" + player.life);
        LoadFile("PlayerJson/Inventory.txt");
        CreateInventory();
	}
	
	// Update is called once per frame
	void Update () {
	
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
            print(e.Message);
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

}

[Serializable]
public class Player
{
    public string name;
    public int life;

    [NonSerialized]
    public PlayerInventory inventory = new PlayerInventory();
    [NonSerialized]
    public PlayerCrew crew;
}

public class PlayerInventory
{
    public List<InventoryObject> food = new List<InventoryObject>();
    public List<InventoryObject> ammunition = new List<InventoryObject>();
}

[Serializable]
public class PlayerCrew
{

}

[Serializable]
public class InventoryObject
{
    public string name;
    public string type;
    public int number;
}