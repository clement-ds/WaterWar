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
    quest.end = new InventoryObject("", "", 1, 10, 10);

    quest.type = (PlayerQuest.QUEST)UnityEngine.Random.Range(0, 2);
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
      quest.end.name = "Flag " + name;
      quest.end.id = UnityEngine.Random.Range(100, 1000);
      quest.end.quantity = 1;

      // MoneyReward outdated (just in case)
      quest.moneyReward = UnityEngine.Random.Range(100, 300);
    } else if (quest.type == PlayerQuest.QUEST.GET) {
      int amount = 0;
      int id = 0;
      string objectName = "";
      
      if (UnityEngine.Random.Range(0, 20) > 18) {
        // rare stuff
        LoadFile("PlayerJson/Rare.txt", objects);
        id = UnityEngine.Random.Range(0, 1);
        amount = UnityEngine.Random.Range(1, 7);
        objectName = JsonUtility.FromJson<InventoryObject>(objects[id]).name;

        // Reward
        quest.reward.type = Reward.REWARD.MONEY;
      } else {
        LoadFile("PlayerJson/Objects.txt", objects);
        id = UnityEngine.Random.Range(0, 11);
        amount = UnityEngine.Random.Range(5, 15);
        objectName = JsonUtility.FromJson<InventoryObject>(objects[id]).name;

        // Reward
        quest.reward.type = (Reward.REWARD)UnityEngine.Random.Range(0, 2);
      }

      // Title
      quest.title = "Collect" + " " + amount.ToString()+ " " + objectName;

      // Description
      quest.description = "Bring " + amount.ToString()+ " " + objectName + " " + "to" + " " + islandName;

      // End
      quest.end.name = objectName;
      quest.end.quantity = amount;

      // MoneyReward outdated (just in case)
      quest.moneyReward = UnityEngine.Random.Range(5, 50);
    } else if (quest.type == PlayerQuest.QUEST.FIND) {
      int amount = 0;
      int id = 0;
      string objectName = "";
      
      if (UnityEngine.Random.Range(0, 20) > 18) {
        // rare stuff
        LoadFile("PlayerJson/Rare.txt", objects);
        id = UnityEngine.Random.Range(0, 1);
        amount = UnityEngine.Random.Range(1, 7);
        objectName = JsonUtility.FromJson<InventoryObject>(objects[id]).name;

        // Reward
        quest.reward.type = Reward.REWARD.MONEY;
      } else {
        LoadFile("PlayerJson/Objects.txt", objects);
        id = UnityEngine.Random.Range(0, 11);
        amount = UnityEngine.Random.Range(5, 15);
        objectName = JsonUtility.FromJson<InventoryObject>(objects[id]).name;

        // Reward
        quest.reward.type = (Reward.REWARD)UnityEngine.Random.Range(0, 2);
      }

      // Title
      quest.title = "Collect" + " " + amount.ToString() + " " + objectName;

      // Description
      quest.description = "Get " + amount.ToString() + " " + objectName + " " + "to" + " " + islandName;

      // End
      quest.end.name = objectName;
      quest.end.quantity = amount;

      // MoneyReward outdated (just in case)
      quest.moneyReward = UnityEngine.Random.Range(5, 50);
    } else if (quest.type == PlayerQuest.QUEST.RECRUIT) {
      int amount = UnityEngine.Random.Range(1, 3);

      // Title
      quest.title = "Recruit" + " " + amount.ToString() + " sailor";

      // Description
      quest.description = "Get " + amount.ToString()+ " " + "sailor";
      
    }

    if (quest.reward.type == Reward.REWARD.INFLUENCE) {
      quest.reward.amount = (int)UnityEngine.Random.Range(5, 15);
    } else if (quest.reward.type == Reward.REWARD.MONEY) {
      quest.reward.amount = UnityEngine.Random.Range(100, 300);
    } else if (quest.reward.type == Reward.REWARD.OBJECT) {
      quest.reward.id = UnityEngine.Random.Range(0, objects.Count);
      quest.reward.amount = UnityEngine.Random.Range(1, 10);
    }

    return quest;
  }

  public bool CheckQuest(PlayerQuest quest, Player player, Island island) {
    List<string> objects = new List<string>();

    if (quest == null || player == null || island == null)
      return false;

    if (quest.localisation == null || !quest.localisation.Equals(island.name))
      return false;
    
    InventoryObject ret = player.inventory.containsObject(quest.end.id);

    if ((quest.type == PlayerQuest.QUEST.FIND ||
        quest.type == PlayerQuest.QUEST.GET ||
        quest.type == PlayerQuest.QUEST.KILL ) &&
        (ret == null || ret.quantity < quest.end.quantity))
      return false;

    if (quest.reward.type == Reward.REWARD.INFLUENCE) {
      island.influence += quest.reward.amount;
    } else if (quest.reward.type == Reward.REWARD.MONEY) {
      player.money += quest.reward.amount;
    } else if (quest.reward.type == Reward.REWARD.OBJECT) {
      LoadFile("PlayerJson/Objects.txt", objects);
      InventoryObject objectReward = JsonUtility.FromJson<InventoryObject>(objects[quest.reward.id]);
      player.inventory.addQuantityOfObject(objectReward, quest.reward.amount);
    }

    if (quest.type == PlayerQuest.QUEST.FIND || quest.type == PlayerQuest.QUEST.KILL) {
      player.inventory.removeQuantityOfObject(ret, quest.end.quantity);
    }
    
    return true;
  }
}
