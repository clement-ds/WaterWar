using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Assets.Script.Battle.Tools;

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

    protected IslandManager(int islandAmount, Boolean newGame)
    {
        if (newGame)
        {
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
        } else
        {
            //json = FileUtils.LoadFile("PlayerJson/IslandSave");
            islandsSave = JsonUtility.FromJson<IslandsSave>(PlayerPrefs.GetString("IslandSave"));
            islands = islandsSave.islands;
        }
    }

    public static IslandManager GetInstance(int islandAmount = 9, Boolean newGame = true)
    {
        if (instance == null)
        {
            instance = new IslandManager(islandAmount, newGame);
        }
        return instance;
    }

    public bool Save()
    {
        PlayerPrefs.SetString("IslandSave", JsonUtility.ToJson(islandsSave));
        PlayerPrefs.Save();
        //try
        //{
        //    StreamWriter writer = new StreamWriter("Assets/Resources/PlayerJson/IslandSave.txt", false);
        //    writer.Write(JsonUtility.ToJson(islandsSave));
        //    writer.Close();
        //} catch (Exception e)
        //{
        //    Debug.Log("SAVE ISLAND : " + e.Message);
        //    return false;
        //}
        return true;
    }

    //private bool LoadFile(string fileName)
    //{
    //    json = new List<string>();
    //    try
    //    {
    //        string line;
    //        StreamReader theReader = new StreamReader(fileName, Encoding.Default);
    //        using (theReader)
    //        {
    //            do
    //            {
    //                line = theReader.ReadLine();

    //                if (line != null)
    //                {
    //                    json.Add(line);
    //                }
    //            }
    //            while (line != null);    
    //            theReader.Close();
    //            return true;
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.Log(e.Message);
    //        return false;
    //    }
    //}
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

