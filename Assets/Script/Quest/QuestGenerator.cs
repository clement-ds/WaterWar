using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

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

    quest.type = (PlayerQuest.QUEST)UnityEngine.Random.Range(0, 2);

    if (quest.type == PlayerQuest.QUEST.KILL) {
      // Title
      quest.title = "Tuer le" + " " + "<Capitaine Barbosa>";

      // Description
      quest.description = "Tuer " + "<l'infâme>" + " " + "<Capitaine Barbosa>" + " " + "<qui térorise>" + " " + "<l'île des pommes>";

      // Reward
      quest.reward.type = (Reward.REWARD)UnityEngine.Random.Range(0, 2);

      // End
      quest.end.name = "Flag " + "<Capitaine Barbosa>";
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
      quest.title = "Avoir" + " " + amount.ToString()+ " " + objectName;

      // Description
      quest.description = "Apporte " + amount.ToString()+ " " + objectName + " " + "sur" + " " + "<l'île des pommes>";


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
        id = UnityEngine.Random.Range(0, 1);
        amount = UnityEngine.Random.Range(1, 7);
        objectName = objects[id];

        // Reward
        quest.reward.type = Reward.REWARD.MONEY;
      } else {
        id = UnityEngine.Random.Range(0, 11);
        amount = UnityEngine.Random.Range(5, 15);
        objectName = objects[id];

        // Reward
        quest.reward.type = (Reward.REWARD)UnityEngine.Random.Range(0, 2);
      }

      // Title
      quest.title = "Avoir" + " " + amount.ToString()+ " " + objectName;

      // Description
      quest.description = "Apporte " + amount.ToString()+ " " + objectName + " " + "sur" + " " + "<l'île des pommes>";


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
}
