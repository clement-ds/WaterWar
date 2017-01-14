using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.IO;

public class CrewDatabase : MonoBehaviour
{
    private List<Item> database = new List<Item>();
    private JsonData itemData;

    void Start()
    {
        itemData = JsonMapper.ToObject(string.Join("", File.ReadAllLines(Application.dataPath + "/StreamingAssets/Crew.json")));
        ConstructItemDatabase();
    }

    void ConstructItemDatabase()
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            database.Add(new Item(
                itemData[i]["type"].ToString(),
                (int)itemData[i]["id"],
                (int)itemData[i]["stats"]["power"],
                (int)itemData[i]["stats"]["defence"],
                (int)itemData[i]["stats"]["vitality"]));
        }
    }

    public Item fetchItemByNbr(int nbr)
    {
        return database[nbr];
    }
}

public class CrewItem
{
    public string Type { get; set; }
    public int Id { get; set; }
    public int Power { get; set; }
    public int Defence { get; set; }
    public int Vitality { get; set; }
    public Sprite Sprite { get; set; }

    public CrewItem(
        string type,
        int id,
        int power,
        int defence,
        int vitality)
    {
        this.Id = id;
        this.Power = Power;
        this.Defence = defence;
        this.Vitality = vitality;
        Sprite[] sheet = Resources.LoadAll<Sprite>("Sprites/" + type);
        this.Sprite = sheet[id];
    }

    public CrewItem()
    {
        this.Type = "None";
    }
}
