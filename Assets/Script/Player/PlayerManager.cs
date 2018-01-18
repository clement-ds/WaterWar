using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Assets.Script.Battle.Tools;

public class PlayerManager
{

    private static PlayerManager instance = null;

    private List<String> json = new List<string>();
    private List<String> objectDictionary = new List<string>();
    public Player player;

    [Serializable]
    public class EnemiesSave
    {
        public List<Player> enemies = new List<Player>();
    }
    public EnemiesSave enemiesSave = new EnemiesSave();
    public List<Player> enemies;

    int maxEnemies = 10;

    protected PlayerManager()
    {
        //player = JsonUtility.FromJson<Player>(FileUtils.readJSON("PlayerJson/Save.json"));
        //player.graphicAsset = Resources.Load("Ship/PlayerShip") as GameObject;
        player = new Player("PlayerShip");
        Debug.Log("player : " + player.name + "/" + player.life);
        //Save();

        //LoadFile("PlayerJson/AISave.json");
        //enemiesSave = JsonUtility.FromJson<EnemiesSave>(json[0]);
        enemies = enemiesSave.enemies;

        for (int i = 0; i < maxEnemies / 2; i += 1)
        {
            enemies.Add(new Player());
        }

        //SaveAI();

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

    public void removeAI(string id)
    {
        for (var i = 0; i < this.enemies.Count; ++i)
        {
            if (this.enemies[i].id == id)
            {
                this.enemies.RemoveAt(i);
            }
        }
    }

    public Player getCharacter(string id)
    {
        Debug.Log("id search : " + id);
        if (this.player.id == id)
            return this.player;
        foreach (Player c in this.enemies)
        {
            Debug.Log("== " +c.id);
            if (c.id == id)
            {
                return c;
            }
        }
        return null;
    }

    public List<Player> getCharacterForEnemy(int currentIsland)
    {
        List<Player> result = new List<Player>();

        foreach (Player c in this.enemies)
        {
            if (c.currentIsland == currentIsland)
            {
                result.Add(c);
            }
        }

        // remove for rendu !
        if (result.Count == 0)
        {
            result.Add(enemies[0]);
        }
        return result;
    }

    public string getNameForObjectId(int id)
    {
        return JsonUtility.FromJson<InventoryObject>(objectDictionary[id]).name;
    }

    public void AcceptQuest(PlayerQuest quest)
    {
        quest.taken = true;
        if (quest.type == PlayerQuest.QUEST.KILL) {
            Player enemie = new Player();
            enemie.name = quest.end.name.Substring(4);
            enemie.inventory.addObject(new InventoryObject("Flag " + enemie.name, "Quest", 1, 100, 10));
            enemie.graphicAsset = Resources.Load("Ship/AiShip") as GameObject;
            enemies.Add(enemie);
            GameManager.Instance.spawnShips(GameObject.Find("MapPivot"));
        }

        player.questLog.quests.Add(quest);
        IslandManager.GetInstance().islands[player.currentIsland].questLog.quests.Remove(quest);
    }

    public List<PlayerQuest> GetQuest()
    {
        return player.questLog.quests;
    }

    public bool Save()
    {
        try
        {
            StreamWriter writer = new StreamWriter("PlayerJson/Save.json", false);
            writer.Write(JsonUtility.ToJson(player));
            writer.Close();
        }
        catch (Exception e)
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
            writer.Write(JsonUtility.ToJson(enemiesSave));
            writer.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
        return true;
    }

    private bool LoadFile(string fileName, List<string> json)
    {
        try
        {
            string line;
            StreamReader theReader = new StreamReader(fileName, Encoding.Default);
            using (theReader)
            {
                do
                {
                    line = theReader.ReadLine();
                    if (line != null)
                    {
                        json.Add(line);
                    }
                } while (line != null);
                theReader.Close();
                return true;
            }
        }
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
    public string id;
    public int life;
    public int money;
    public int currentIsland;
    public Vector2 mapPosition;
    public GameObject graphicAsset;
    public GameObject mapShip;

    public PlayerInventory inventory = new PlayerInventory();
    public PlayerCrew crew = new PlayerCrew();
    public QuestLog questLog = new QuestLog();
    public PlayerShip ship = new PlayerShip();

    private List<String> json = new List<string>();

    public Player(String assetName = "AiShip")
    {
        //Gen ID
        id = Guid.NewGuid().ToString();

        Debug.Log("PLAYER ID : " + id);

        //Gen name
        LoadFile("PlayerJson/Names.txt");
        int rng = UnityEngine.Random.Range(0, json.Count);
        Name name = JsonUtility.FromJson<Name>(json[rng]);
        this.name = name.name;

        //Set graphic asset
        graphicAsset = Resources.Load("Ship/" + assetName) as GameObject;

        //Gen ship
        Debug.Log(json);
        this.ship = JsonUtility.FromJson<PlayerShip>(FileUtils.readJSON("PlayerJson/ship5.json"));

        //Set starter island
        this.currentIsland = UnityEngine.Random.Range(0, (GameManager.Instance == null ? 0 : GameManager.Instance.islandsAmount));

        //Starter life and money
        this.life = 100;
        this.money = 100;

        //Gen crew
        this.crew.AddCrew("Captain");
        this.crew.AddCrew("Bego");

        //Gen inventory
        this.inventory.food.Add(new InventoryObject("Water", "Food", 5, 10, 10));
        this.inventory.weapons.Add(new InventoryObject("Bread", "Food", 5, 10, 10));
    }

    private bool LoadFile(string fileName)
    {
        json = new List<string>();
        try
        {
            string line;
            StreamReader theReader = new StreamReader(fileName, Encoding.Default);
            using (theReader)
            {
                do
                {
                    line = theReader.ReadLine();

                    if (line != null)
                    {
                        json.Add(line);
                    }
                }
                while (line != null);
                theReader.Close();
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
    }
}

[Serializable]
public class PlayerInventory : Inventory
{
    private static List<string> crewWeapon = new List<string>() { "Sabre", "Musket", "Riffle"};

    public int getCrewWeapon()
    {
        int number = 0;


        foreach (var weapon in weapons)
        {
            if (crewWeapon.Contains(weapon.name))
            {
                ++number;
            }
        }
        return number;
    }
}

[Serializable]
public class PlayerCrew
{
    public List<CrewMember> crewMembers = new List<CrewMember>();
    public long crewIncrement = 0;

    public PlayerCrew()
    {
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
public enum Rarity { LEGENDARY, EPIC, RARE, COMMON}

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

    public InventoryObject(InventoryObject obj)
    {
        this.name = obj.name;
        this.type = obj.type;
        this.quantity = obj.quantity;
        this.price = obj.price;
        this.id = obj.id;
        this.basePrice = obj.basePrice;
    }    

    public InventoryObject(InventoryObject invObj, int quantity = -1)
    {
        this.name = invObj.name;
        this.type = invObj.type;
        this.quantity = quantity == -1 ? invObj.quantity : quantity;
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
public class EndQuest
{
    public int type;
    public int enemyType;
    public int enemyId;
}

[Serializable]
public class Reward
{
    public enum REWARD { MONEY = 0, OBJECT = 1, INFLUENCE = 2 };
    public REWARD type;
    public int id;
    public string name;
    public int amount;
}

[Serializable]
public class PlayerQuest
{
    public enum QUEST { KILL = 0, GET, FIND, RECRUIT, SACK, MORAL, PRINCIPAL, INFLUENCE, DESTROY };

    public string description;
    public string title;
    public string localisation;
    public QUEST type;
    public string objective;
    public List<Reward> rewards;
    public InventoryObject end;
    public int moneyReward;
    public bool taken = false;

    public List<string> GetRewardString() {
        List<string> rewardString = new List<string>();

        rewards.ForEach((reward) => {
            if (reward.type == Reward.REWARD.MONEY) {
                rewardString.Add(reward.amount.ToString() + "$");
            } else if (reward.type == Reward.REWARD.INFLUENCE) {
                rewardString.Add(reward.amount.ToString() + " influence on " + localisation);
            } else if (reward.type == Reward.REWARD.OBJECT) {
                rewardString.Add(reward.amount.ToString() + " " + reward.name);
            }
        });
        return rewardString;
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