using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class Adjectif {
  public string name;
}

public class Name {
    public string firstname;
    public string name;
    public string race;
}
public class QuestGenerator {  
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

  public PlayerQuest GenerateQuest(Island currentIsland) {
    PlayerQuest quest = new PlayerQuest();
    List<string> objects = new List<string>();
    string islandName = currentIsland.name;
    int nbrOfReward = UnityEngine.Random.Range(1, 3);
    int maxReward = 0;
    bool influence = false;
    bool money = false;
    LoadFile("PlayerJson/Objects.txt", objects);

    quest.rewards = new List<Reward>();
    quest.type = (PlayerQuest.QUEST)UnityEngine.Random.Range(0, 4);
    quest.localisation = islandName;

    if (quest.type == PlayerQuest.QUEST.KILL) {
      List<string> names = new List<string>();
      List<string> adjectifs = new List<string>();

      LoadFile("PlayerJson/Names.txt", names);
      LoadFile("PlayerJson/Adjectif.txt", adjectifs);

      // Title
      string name = JsonUtility.FromJson<Name>(names[UnityEngine.Random.Range(0, 4)]).name;
      string adjectif = JsonUtility.FromJson<Adjectif>(adjectifs[UnityEngine.Random.Range(0, 4)]).name;

      quest.title = "Kill" + " " + "Captain " + name;

      // Description
      quest.description = "Kill " + adjectif + " " + "Captain " + name + " " + "<terrorizing>" + " " + islandName;

      // End
      quest.end = new InventoryObject("Flag " + name, "Quest", 1, 0, 0);
      quest.end.id = UnityEngine.Random.Range(100, 1000);
      quest.end.quantity = 1;

      // MoneyReward outdated (just in case)
      quest.moneyReward = UnityEngine.Random.Range(100, 300);
    } else if (quest.type == PlayerQuest.QUEST.GET) {
      if (UnityEngine.Random.Range(0, 20) > 18) {
        // rare stuff
        LoadFile("PlayerJson/Rare.txt", objects);
        quest.end = new InventoryObject(JsonUtility.FromJson<InventoryObject>(objects[UnityEngine.Random.Range(0, 1)]));
        quest.end.quantity = UnityEngine.Random.Range(1, 7);
      } else {
        quest.end = JsonUtility.FromJson<InventoryObject>(objects[UnityEngine.Random.Range(0, 11)]);
        quest.end.quantity = UnityEngine.Random.Range(5, 15);
      }

      // Title
      quest.title = "Collect" + " " + quest.end.quantity.ToString()+ " " + quest.end.name;

      // Description
      quest.description = "Bring " + quest.end.quantity.ToString()+ " " + quest.end.name + " " + "to" + " " + islandName;

      // MoneyReward outdated (just in case)
      maxReward += quest.end.price * quest.end.quantity;
      quest.moneyReward = UnityEngine.Random.Range(5, 50);
    } else if (quest.type == PlayerQuest.QUEST.FIND) {
      if (UnityEngine.Random.Range(0, 20) > 18) {
        // rare stuff
        LoadFile("PlayerJson/Rare.txt", objects);
        quest.end = new InventoryObject(JsonUtility.FromJson<InventoryObject>(objects[UnityEngine.Random.Range(0, 1)]));
        quest.end.quantity = UnityEngine.Random.Range(1, 7);
      } else {
        LoadFile("PlayerJson/Objects.txt", objects);
        quest.end = JsonUtility.FromJson<InventoryObject>(objects[UnityEngine.Random.Range(0, 11)]);
        quest.end.quantity = UnityEngine.Random.Range(5, 15);
      }

      // Title
      quest.title = "Collect" + " " + quest.end.quantity.ToString()+ " " + quest.end.name;

      // Description
      quest.description = "Find " + quest.end.quantity.ToString()+ " " + quest.end.name;

      // MoneyReward outdated (just in case)
      maxReward += quest.end.price * quest.end.quantity;
      quest.moneyReward = UnityEngine.Random.Range(5, 50);
    } else if (quest.type == PlayerQuest.QUEST.RECRUIT) {
      int amount = UnityEngine.Random.Range(1, 3);
      // Title
      quest.title = "Recruit" + " " + amount.ToString() + " sailors";

      // Description
      quest.description = "Recruit " + amount.ToString()+ " " + "sailors on your ship !";
      quest.end = new InventoryObject("CrewMember", "Quest", 1, 0, 0);
      quest.end.quantity = amount + PlayerManager.GetInstance().player.crew.crewMembers.Count;
    }

    // Generate Rewards
    maxReward = Convert.ToInt32(maxReward * 1.3);
    for (int i = 0; i < nbrOfReward; i++) {
      Reward reward = new Reward();
      reward.type = (Reward.REWARD)UnityEngine.Random.Range(0, 2);

      if (reward.type == Reward.REWARD.INFLUENCE) {
        if (influence) {
          reward.type = Reward.REWARD.OBJECT;
        } else {
          reward.amount = (int)UnityEngine.Random.Range(5, 15);
          reward.name = "influence";
          influence = true;
          nbrOfReward++;
        }
      }
      if (reward.type == Reward.REWARD.MONEY) {
        if (money || quest.type == PlayerQuest.QUEST.RECRUIT) {
          reward.type = Reward.REWARD.OBJECT;
        } else {
          reward.amount = UnityEngine.Random.Range(100, 300);
          reward.name = "money";
          money = true;
          maxReward -= reward.amount;
          if (maxReward < 0) {
            i += 3;
          } 
        }
      }
      if (reward.type == Reward.REWARD.OBJECT) {
        InventoryObject objectReward = JsonUtility.FromJson<InventoryObject>(objects[UnityEngine.Random.Range(0, objects.Count)]);

        while (quest.rewards.Find((r) => r.id == objectReward.id) != null) {
          objectReward = JsonUtility.FromJson<InventoryObject>(objects[UnityEngine.Random.Range(0, objects.Count)]);
        }
        reward.id = objectReward.id;
        reward.name = objectReward.name;
        reward.amount = UnityEngine.Random.Range(1, 5);
        maxReward -= (reward.amount * objectReward.price);
        if (maxReward < 0) {
          i += 3;
        }
      }
      quest.rewards.Add(reward);
    }

    while (maxReward > 0) {
      Reward reward = new Reward();
      reward.type = Reward.REWARD.OBJECT;

      InventoryObject objectReward = JsonUtility.FromJson<InventoryObject>(objects[UnityEngine.Random.Range(0, objects.Count)]);

      while (quest.rewards.Find((r) => r.id == objectReward.id) != null) {
        objectReward = JsonUtility.FromJson<InventoryObject>(objects[UnityEngine.Random.Range(0, objects.Count)]);
      }
      reward.id = objectReward.id;
      reward.name = objectReward.name;
      reward.amount = UnityEngine.Random.Range(1, 5);
      maxReward -= (reward.amount * objectReward.price);
      quest.rewards.Add(reward);
    }
    return quest;
  }

  public bool CheckQuest(PlayerQuest quest, Player player, Island island) {
    if (quest == null || player == null || island == null)
      return false;

    if (quest.localisation == null || quest.localisation.Equals(island.name) == false)
      return false;

    if (quest.type == PlayerQuest.QUEST.GET || quest.type == PlayerQuest.QUEST.FIND || quest.type == PlayerQuest.QUEST.KILL) {
        InventoryObject objects = player.inventory.food.Find((item) => item.name == quest.end.name);
        if (objects != null && objects.quantity >= quest.end.quantity)
          return true;
    }
    if (quest.type == PlayerQuest.QUEST.RECRUIT) {
      return player.crew.crewMembers.Count >= quest.end.quantity;
    }
    return false;
  }

  public bool ValidateQuest(PlayerQuest quest, Player player, Island island) {
    if (!CheckQuest(quest, player, island))
      return false;

    List<string> objects = new List<string>();

    quest.rewards.ForEach((reward) => {
      if (reward.type == Reward.REWARD.INFLUENCE) {
        island.influence += reward.amount;
      } else if (reward.type == Reward.REWARD.MONEY) {
        player.money += reward.amount;
      } else if (reward.type == Reward.REWARD.OBJECT) {
        LoadFile("PlayerJson/Objects.txt", objects);
        InventoryObject objectReward = JsonUtility.FromJson<InventoryObject>(objects[reward.id]);
        Debug.Log("id: " + reward.id);
        Debug.Log("name: " + reward.name);
        Debug.Log("amount: " + reward.amount);
        player.inventory.addQuantityOfObject(objectReward, reward.amount);
      }
    });

    if (quest.type == PlayerQuest.QUEST.GET || quest.type == PlayerQuest.QUEST.KILL) {
      InventoryObject ret = player.inventory.food.Find((item) => item.name == quest.end.name);
      player.inventory.removeQuantityOfObject(ret, quest.end.quantity);
    }
    player.questLog.quests.Remove(quest);
    return true;
  }
}
