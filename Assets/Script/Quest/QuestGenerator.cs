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

/*
  public string title;
  public string description;
  public int type;
  public string objective;
  public InventoryObject reward;
  public EndQuest end;
  public int moneyReward;
 */

  public PlayerQuest GenerateQuest() {
    PlayerQuest quest = new PlayerQuest();
    
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
      
      if (UnityEngine.Random.Range(0, 20) > 18) {
        // rare quest
        amount = UnityEngine.Random.Range(1, 5);

        // Reward
        quest.reward.type = Reward.REWARD.MONEY;
      } else {
        amount = UnityEngine.Random.Range(4, 15);

        // Reward
        quest.reward.type = (Reward.REWARD)UnityEngine.Random.Range(0, 2);
      }

      // Title
      quest.title = "Avoir" + " " + amount.ToString()+ " " + "<Apple>";

      // Description
      quest.description = "Apporte " + amount.ToString()+ " " + "<Apple>" + " " + "sur" + " " + "<l'île des pommes>";


      // End
      quest.end.name = "<Apple>";
      quest.end.quantity = amount;

      // MoneyReward outdated (just in case)
      quest.moneyReward = UnityEngine.Random.Range(5, 50);
    } else if (quest.type == PlayerQuest.QUEST.FIND) {
      int amount = 0;
      int objectId = 0;
      //string title = "";

      if (UnityEngine.Random.Range(0, 20) > 18) {
        // rare quest
        amount = UnityEngine.Random.Range(1, 5);
        objectId = UnityEngine.Random.Range(0, 5);
        //title = json[objectId]

        // Reward
        quest.reward.type = Reward.REWARD.MONEY;
      } else {
        amount = UnityEngine.Random.Range(4, 15);
        objectId = UnityEngine.Random.Range(0, 10);
        //title = json[objectId]

        // Reward
        quest.reward.type = (Reward.REWARD)UnityEngine.Random.Range(0, 2);
      }

      // Title
      quest.title = "Avoir" + " " + amount.ToString()+ " " + "<Apple>";

      // Description
      quest.description = "Apporte " + amount.ToString()+ " " + "<Apple>" + " " + "sur" + " " + "<l'île des pommes>";


      // End
      quest.end.name = "<Apple>";
      quest.end.quantity = amount;

      // MoneyReward outdated (just in case)
      quest.moneyReward = UnityEngine.Random.Range(5, 50);
    }

    if (quest.reward.type == Reward.REWARD.INFLUENCE) {
      quest.reward.amount = (int)UnityEngine.Random.Range(5, 15);
    } else if (quest.reward.type == Reward.REWARD.MONEY) {
      quest.reward.amount = UnityEngine.Random.Range(100, 300);
    } else if (quest.reward.type == Reward.REWARD.OBJECT) {
      quest.reward.id = 10; // For example coconut we need to give an id to each object
      quest.reward.amount = UnityEngine.Random.Range(1, 10);      
    }
    return quest;
  }
}
