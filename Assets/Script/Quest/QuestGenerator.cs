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

  public PlayerQuest GenerateQuest() {
    PlayerQuest quest = new PlayerQuest();
    List<string> objects = new List<string>();

    quest.reward = new Reward();
    quest.end = new InventoryObject("", "", 1, 10, 10);

    quest.type = (PlayerQuest.QUEST)UnityEngine.Random.Range(0, 2);

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
      quest.description = "Kill " + adjectif + " " + "Captain " + name + " " + "<terrorizing>" + " " + "<Apple Island>";

      // Reward
      quest.reward.type = (Reward.REWARD)UnityEngine.Random.Range(0, 2);

      // End
      quest.end.name = "Flag " + "<Captain Barbosa>";
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
      quest.description = "Bring " + amount.ToString()+ " " + objectName + " " + "to" + " " + "<Apple Island>";


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
      quest.title = "Collect" + " " + amount.ToString()+ " " + objectName;

      // Description
      quest.description = "Bring " + amount.ToString()+ " " + objectName + " " + "to" + " " + "<Apple Island>";


      // End
      quest.end.name = objectName;
      quest.end.quantity = amount;

      // MoneyReward outdated (just in case)
      quest.moneyReward = UnityEngine.Random.Range(5, 50);
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

    // Check if parameters exist
    if (quest == null || player == null || island == null) {
      return false;
    }

    // Check & get the correct object
   InventoryObject ret = player.inventory.containsObject(quest.end.id);

    if (ret != null && ret.quantity >= quest.end.quantity) {
      Debug.Log("Yes");
      if (quest.reward.type == Reward.REWARD.INFLUENCE) {
        Debug.Log("1");
        island.influence += quest.reward.amount;
      } else if (quest.reward.type == Reward.REWARD.MONEY) {
        Debug.Log("2");
        player.money += quest.reward.amount;
      } else if (quest.reward.type == Reward.REWARD.OBJECT) {
        Debug.Log("3");
        LoadFile("PlayerJson/Objects.txt", objects);
        InventoryObject objectReward = JsonUtility.FromJson<InventoryObject>(objects[quest.reward.id]);
        Debug.Log("3.1");
        player.inventory.addQuantityOfObject(objectReward, quest.reward.amount);
        Debug.Log("3.2");
      }
      Debug.Log("4");
      player.inventory.removeQuantityOfObject(ret, quest.end.quantity);
      Debug.Log("Ok");
      return true;
    }
    return false;
  }
}
