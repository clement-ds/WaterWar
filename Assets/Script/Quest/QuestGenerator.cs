using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Assets.Script.Battle.Tools;

public class Adjectif
{
    public string name;
}

public class Name
{
    public string firstname;
    public string name;
    public string race;
}

public class QuestGenerator
{
    private List<string> stringJson;
    private List<InventoryObject> objects;
    private List<InventoryObject> rares;
    private List<InventoryObject> foods;
    private List<InventoryObject> weapons;
    private List<Name> names;
    private List<Adjectif> adjectifs;
    private List<Adjectif> adverbs;

    public QuestGenerator()
    {
        stringJson = new List<string>();
        objects = new List<InventoryObject>();
        rares = new List<InventoryObject>();
        foods = new List<InventoryObject>();
        weapons = new List<InventoryObject>();
        names = new List<Name>();
        adverbs = new List<Adjectif>();
        adjectifs = new List<Adjectif>();

        stringJson = FileUtils.LoadFile("PlayerJson/Objects");
        for (int i = 0; i < stringJson.Count; i++)
        {
            objects.Add(JsonUtility.FromJson<InventoryObject>(stringJson[i]));
            if (objects[i].type == "Food")
                foods.Add(objects[i]);
            else if (objects[i].type == "Weapon")
                weapons.Add(objects[i]);
            else if (objects[i].type == "Rare")
                rares.Add(objects[i]);
        }

        stringJson = FileUtils.LoadFile("PlayerJson/Names");
        for (int i = 0; i < stringJson.Count; i++)
            names.Add(JsonUtility.FromJson<Name>(stringJson[i]));

        stringJson = FileUtils.LoadFile("PlayerJson/Adjectif");
        for (int i = 0; i < stringJson.Count; i++)
            adjectifs.Add(JsonUtility.FromJson<Adjectif>(stringJson[i]));

        stringJson = FileUtils.LoadFile("PlayerJson/Adverb");
        for (int i = 0; i < stringJson.Count; i++)
            adverbs.Add(JsonUtility.FromJson<Adjectif>(stringJson[i]));
    }

    public PlayerQuest GenerateQuest(Island currentIsland)
    {
        PlayerQuest quest = new PlayerQuest();
        string islandName = currentIsland.name;
        int nbrOfReward = UnityEngine.Random.Range(1, 3);
        int maxReward = 0;
        bool influence = false;
        bool money = false;

        quest.rewards = new List<Reward>();
        quest.type = (PlayerQuest.QUEST)UnityEngine.Random.Range(0, 4);
        quest.localisation = islandName;

        if (quest.type == PlayerQuest.QUEST.KILL)
        {
            // Title
            string name = names[UnityEngine.Random.Range(0, names.Count)].name;
            string adjectif = adjectifs[UnityEngine.Random.Range(0, adjectifs.Count)].name;

            quest.title = "Kill" + " " + "Captain " + name + " for " + islandName;

            // Description
            quest.description = "Kill " + adjectif + " " + "Captain " + name + " " + adverbs[UnityEngine.Random.Range(0, adverbs.Count)].name + " for " + islandName;

            // End
            quest.end = new InventoryObject("Flag " + name, "Quest", 1, 0, 0);
            quest.end.id = UnityEngine.Random.Range(100, 1000);
            quest.end.quantity = 1;

            // MoneyReward outdated (just in case)
            maxReward = UnityEngine.Random.Range(100, 300);
            quest.moneyReward = UnityEngine.Random.Range(100, 300);
        }
        else if (quest.type == PlayerQuest.QUEST.GET)
        {
            if (UnityEngine.Random.Range(0, 20) > 18)
            {
                quest.end = new InventoryObject(rares[UnityEngine.Random.Range(0, rares.Count)]);
                quest.end.quantity = UnityEngine.Random.Range(1, 7);
            }
            else
            {
                quest.end = new InventoryObject(foods[UnityEngine.Random.Range(0, foods.Count)]);
                quest.end.quantity = UnityEngine.Random.Range(5, 15);
            }

            // Title
            quest.title = "Collect" + " " + quest.end.quantity.ToString() + " " + quest.end.name + " for " + islandName;

            // Description
            quest.description = "Bring " + quest.end.quantity.ToString() + " " + quest.end.name + " " + "to" + " " + islandName;

            // MoneyReward outdated (just in case)
            maxReward += quest.end.price * quest.end.quantity;
            quest.moneyReward = UnityEngine.Random.Range(5, 50);
        }
        else if (quest.type == PlayerQuest.QUEST.FIND)
        {
            if (UnityEngine.Random.Range(0, 20) > 18)
            {
                // rare stuff
                quest.end = new InventoryObject(rares[UnityEngine.Random.Range(0, rares.Count)]);
                quest.end.quantity = UnityEngine.Random.Range(1, 7);
            }
            else
            {
                quest.end = new InventoryObject(foods[UnityEngine.Random.Range(0, foods.Count)]);
                quest.end.quantity = UnityEngine.Random.Range(5, 15);
            }

            // Title
            quest.title = "Collect" + " " + quest.end.quantity.ToString() + " " + quest.end.name + " for " + islandName;

            // Description
            quest.description = "Find " + quest.end.quantity.ToString() + " " + quest.end.name;

            // MoneyReward outdated (just in case)
            maxReward += quest.end.price * quest.end.quantity;
            quest.moneyReward = UnityEngine.Random.Range(5, 50);
        }
        else if (quest.type == PlayerQuest.QUEST.RECRUIT)
        {
            int amount = UnityEngine.Random.Range(1, 3);
            // Title
            quest.title = "Recruit" + " " + amount.ToString() + " sailors for " + islandName;

            // Description
            quest.description = "Recruit " + amount.ToString() + " " + "sailors on your ship for " + islandName;
            quest.end = new InventoryObject("CrewMember", "Quest", 1, 0, 0);
            quest.end.quantity = amount + PlayerManager.GetInstance().player.crew.crewMembers.Count;
            maxReward = UnityEngine.Random.Range(1, 50);
        }

        // Generate Rewards
        maxReward = Convert.ToInt32(maxReward * 1.2);
        for (int i = 0; i < nbrOfReward; i++)
        {
            Reward reward = new Reward();
            reward.type = (Reward.REWARD)UnityEngine.Random.Range(0, 2);

            if (reward.type == Reward.REWARD.INFLUENCE)
            {
                if (influence)
                {
                    reward.type = Reward.REWARD.OBJECT;
                }
                else
                {
                    reward.amount = (int)UnityEngine.Random.Range(5, 15);
                    reward.name = "influence";
                    influence = true;
                    nbrOfReward++;
                }
            }
            if (reward.type == Reward.REWARD.MONEY)
            {
                if (money || quest.type == PlayerQuest.QUEST.RECRUIT)
                {
                    reward.type = Reward.REWARD.OBJECT;
                }
                else
                {
                    reward.amount = UnityEngine.Random.Range(100, maxReward);
                    reward.name = "money";
                    money = true;
                    maxReward -= reward.amount;
                    if (maxReward < 0)
                    {
                        i += 3;
                    }
                }
            }
            if (reward.type == Reward.REWARD.OBJECT)
            {
                InventoryObject objectReward = (UnityEngine.Random.Range(0, 20) > 16) ? rares[UnityEngine.Random.Range(0, rares.Count)] : foods[UnityEngine.Random.Range(0, foods.Count)];

                while (quest.rewards.Find((r) => r.id == objectReward.id) != null)
                {
                    objectReward = (UnityEngine.Random.Range(0, 20) > 16) ? rares[UnityEngine.Random.Range(0, rares.Count)] : foods[UnityEngine.Random.Range(0, foods.Count)];
                }
                reward.id = objectReward.id;
                reward.name = objectReward.name;
                reward.amount = (objectReward.type == "Rare") ? 1 : UnityEngine.Random.Range(1, 5);
                maxReward -= (reward.amount * objectReward.price);
                if (maxReward < 0)
                {
                    i += 3;
                }
            }
            quest.rewards.Add(reward);
        }

        while (maxReward > 0 && quest.rewards.Count < 4)
        {
            Reward reward = new Reward();
            reward.type = Reward.REWARD.OBJECT;

            InventoryObject objectReward = new InventoryObject(objects[UnityEngine.Random.Range(0, objects.Count)]);

            while (quest.rewards.Find((r) => r.id == objectReward.id) != null)
            {
                objectReward = new InventoryObject(objects[UnityEngine.Random.Range(0, objects.Count)]);
            }
            reward.id = objectReward.id;
            reward.name = objectReward.name;
            reward.amount = UnityEngine.Random.Range(1, 5);
            maxReward -= (reward.amount * objectReward.price);
            quest.rewards.Add(reward);
        }
        if (maxReward > 0)
        {
            bool isNew = false;
            Reward reward = quest.rewards.Find((r) => r.name == "money");

            if (reward == null)
            {
                isNew = true;
                reward = new Reward();
            }
            reward.amount += maxReward + UnityEngine.Random.Range(0, 50);
            reward.name = "money";
            if (isNew)
                quest.rewards.Add(reward);
        }

        return quest;
    }

    public bool CheckQuest(PlayerQuest quest, Player player, Island island)
    {
        if (quest == null || player == null || island == null)
            return false;

        if (quest.localisation == null || quest.localisation.Equals(island.name) == false)
            return false;

        if (quest.type == PlayerQuest.QUEST.GET || quest.type == PlayerQuest.QUEST.FIND || quest.type == PlayerQuest.QUEST.KILL)
        {
            InventoryObject objects = player.inventory.food.Find((item) => item.name == quest.end.name);
            if (objects != null && objects.quantity >= quest.end.quantity)
                return true;
        }
        if (quest.type == PlayerQuest.QUEST.RECRUIT)
        {
            return player.crew.crewMembers.Count >= quest.end.quantity;
        }
        return false;
    }

    public bool ValidateQuest(PlayerQuest quest, Player player, Island island)
    {
        if (!CheckQuest(quest, player, island))
            return false;

        quest.rewards.ForEach((reward) =>
        {
            if (reward.type == Reward.REWARD.INFLUENCE)
            {
                island.influence += reward.amount;
            }
            else if (reward.type == Reward.REWARD.MONEY)
            {
                player.money += reward.amount;
            }
            else if (reward.type == Reward.REWARD.OBJECT)
            {
                InventoryObject objectReward = new InventoryObject(objects[reward.id]);
                player.inventory.addQuantityOfObject(objectReward, reward.amount);
            }
        });

        if (quest.type == PlayerQuest.QUEST.GET || quest.type == PlayerQuest.QUEST.KILL)
        {
            InventoryObject ret = player.inventory.food.Find((item) => item.name == quest.end.name);
            player.inventory.removeQuantityOfObject(ret, quest.end.quantity);
        }
        island.influence += 5;
        player.questLog.quests.Remove(quest);
        return true;
    }
}
