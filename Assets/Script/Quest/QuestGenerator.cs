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

    quest.reward = new Reward();
//    quest.end = new InventoryObject("", "", 1, 10, 10);
    quest.rewards = new List<Reward>();
    quest.type = (PlayerQuest.QUEST)UnityEngine.Random.Range(0, 3);
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

      // Reward
      quest.reward.type = (Reward.REWARD)UnityEngine.Random.Range(0, 2);

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

        // Reward
        quest.reward.type = Reward.REWARD.MONEY;
      } else {
        LoadFile("PlayerJson/Objects.txt", objects);
        quest.end = JsonUtility.FromJson<InventoryObject>(objects[UnityEngine.Random.Range(0, 11)]);
        quest.end.quantity = UnityEngine.Random.Range(5, 15);

        // Reward
        quest.reward.type = (Reward.REWARD)UnityEngine.Random.Range(0, 2);
      }

      // Title
      quest.title = "Collect" + " " + quest.end.quantity.ToString()+ " " + quest.end.name;

      // Description
      quest.description = "Bring " + quest.end.quantity.ToString()+ " " + quest.end.name + " " + "to" + " " + islandName;

      // MoneyReward outdated (just in case)
      quest.moneyReward = UnityEngine.Random.Range(5, 50);
    } else if (quest.type == PlayerQuest.QUEST.FIND) {
      if (UnityEngine.Random.Range(0, 20) > 18) {
        // rare stuff
        LoadFile("PlayerJson/Rare.txt", objects);
        quest.end = new InventoryObject(JsonUtility.FromJson<InventoryObject>(objects[UnityEngine.Random.Range(0, 1)]));
        quest.end.quantity = UnityEngine.Random.Range(1, 7);

        // Reward
        quest.reward.type = Reward.REWARD.MONEY;
      } else {
        LoadFile("PlayerJson/Objects.txt", objects);
        quest.end = JsonUtility.FromJson<InventoryObject>(objects[UnityEngine.Random.Range(0, 11)]);
        quest.end.quantity = UnityEngine.Random.Range(5, 15);

        // Reward
        quest.reward.type = (Reward.REWARD)UnityEngine.Random.Range(0, 2);
      }

      // Title
      quest.title = "Collect" + " " + quest.end.quantity.ToString()+ " " + quest.end.name;

      // Description
      quest.description = "Find " + quest.end.quantity.ToString()+ " " + quest.end.name;

      // MoneyReward outdated (just in case)
      quest.moneyReward = UnityEngine.Random.Range(5, 50);
    } else if (quest.type == PlayerQuest.QUEST.RECRUIT) {
      int amount = UnityEngine.Random.Range(1, 3);

      // Title
      quest.title = "Recruit" + " " + amount.ToString() + " sailors";

      // Description
      quest.description = "Recruit " + amount.ToString()+ " " + "sailors on your shit !";
      quest.end.quantity = amount + PlayerManager.GetInstance().player.crew.crewMembers.Count;
    }

    int nbrOfObjects = UnityEngine.Random.Range(0, 3);

    if (quest.reward.type == Reward.REWARD.INFLUENCE) {
      Reward reward = new Reward();
      reward.amount = (int)UnityEngine.Random.Range(5, 15);
      reward.name = "influence";
      quest.rewards.Add(reward);
    } else if (quest.reward.type == Reward.REWARD.MONEY) {
      Reward reward = new Reward();
      reward.amount = UnityEngine.Random.Range(100, 300);
      reward.name = "money";
      quest.rewards.Add(reward);
    } else if (quest.reward.type == Reward.REWARD.OBJECT) {
      LoadFile("PlayerJson/Objects.txt", objects);

      for (int i = 0; i < nbrOfObjects; i++) {
        Reward reward = new Reward();
        InventoryObject objectReward = JsonUtility.FromJson<InventoryObject>(objects[UnityEngine.Random.Range(0, objects.Count)]);
        reward.id = objectReward.id;
        reward.name = objectReward.name;
        reward.amount = UnityEngine.Random.Range(1, 8);
        quest.rewards.Add(reward);
      }
    }

    return quest;
  }

  public bool CheckQuest(PlayerQuest quest, Player player, Island island) {
    List<string> objects = new List<string>();

    if (quest == null || player == null || island == null)
      return false;

    if (quest.localisation == null || !quest.localisation.Equals(island.name))
      return false;

    InventoryObject ret = player.inventory.food.Find((item) => item.id == quest.end.id);

    if ((quest.type == PlayerQuest.QUEST.FIND ||
        quest.type == PlayerQuest.QUEST.GET ||
        quest.type == PlayerQuest.QUEST.KILL) &&
        (ret == null || ret.quantity < quest.end.quantity))
      return false;

    if (quest.type == PlayerQuest.QUEST.RECRUIT && player.crew.crewMembers.Count < quest.end.quantity) {
      return false;
    }
    
    quest.rewards.ForEach((reward) => {
      if (reward.type == Reward.REWARD.INFLUENCE) {
        island.influence += reward.amount;
      } else if (reward.type == Reward.REWARD.MONEY) {
        player.money += reward.amount;
      } else if (reward.type == Reward.REWARD.OBJECT) {
        LoadFile("PlayerJson/Objects.txt", objects);
        InventoryObject objectReward = JsonUtility.FromJson<InventoryObject>(objects[reward.id]);
        player.inventory.addQuantityOfObject(objectReward, reward.amount);
      }
    });

    if (quest.type == PlayerQuest.QUEST.FIND || quest.type == PlayerQuest.QUEST.KILL) {
      player.inventory.removeQuantityOfObject(ret, quest.end.quantity);
    }

    return true;
  }
}
