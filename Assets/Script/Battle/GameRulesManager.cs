using System.Collections.Generic;
using UnityEngine;

public enum GameStatus { VICTORY, DEFEAT, ESCAPE }

public enum DestroyedStatus { NONE, ALIVE, DESTROY_SHIP, KILL_MEMBERS }

public class GameRulesManager
{

    private bool endOfTheGame;
    public GuiAccess guiAccess;

    public List<Battle_Ship> ships;

    public Dictionary<string, Pair<Player, DestroyedStatus>> characters;

    protected GameRulesManager()
    {
        this.endOfTheGame = false;
        this.ships = new List<Battle_Ship>();
        this.characters = new Dictionary<string, Pair<Player, DestroyedStatus>>();
    }

    /** INIT **/
    public void init()
    {
        this.endOfTheGame = false;
    }

    public void initializeGui()
    {
        this.guiAccess = GameObject.Find("Battle_UI").GetComponent<GuiAccess>();
    }

    /** RULES **/
    public void enemyDestroyed(string id, DestroyedStatus status)
    {
        for (var i = 0; i < this.ships.Count; ++i)
        {
            if (this.ships[i].getId() == id)
            {
                this.ships.RemoveAt(i);
                --i;
            }
        }
        this.characters[id].V2 = status;
        if (this.ships.Count == 1)
        {
            this.launchEndOfTheGame(GameStatus.VICTORY);
        }
    }

    public void playerDestroyed(string id, DestroyedStatus status)
    {
        this.characters[id].V2 = status;
        this.launchEndOfTheGame(GameStatus.DEFEAT);
    }

    public void playerEscaped(string id)
    {
        this.characters[id].V2 = DestroyedStatus.NONE;
        this.launchEndOfTheGame(GameStatus.ESCAPE);
    }

    private void launchEndOfTheGame(GameStatus status)
    {
        string message = "";

        if (status == GameStatus.VICTORY)
            message = "You killed your ennemy";
        else if (status == GameStatus.DEFEAT)
            message = "Your opponent" + (this.characters.Count > 2 ? "s" : "") + " killed you";
        else if (status == GameStatus.ESCAPE)
            message = "You escape the fight";
        GameRulesManager.GetInstance().guiAccess.endMessages[1].text = message;
        GameRulesManager.GetInstance().guiAccess.endPanel.gameObject.SetActive(true);

        shareLoot();
    }

    private void shareLoot()
    {
        List<InventoryObject> lootFood = new List<InventoryObject>();
        List<InventoryObject> lootWeapon = new List<InventoryObject>();

        List<string> winners = new List<string>();

        foreach (var character in this.characters)
        {
            if (character.Value.V2 != DestroyedStatus.ALIVE)
            {
                this.collectSpecificLoot(lootFood, character.Value.V1.inventory.food, character.Value.V2, 1f);
                this.collectSpecificLoot(lootWeapon, character.Value.V1.inventory.weapons, character.Value.V2, 0.5f);
            }
            else
            {
                winners.Add(character.Key);
                Debug.Log("winner: " + character.Key);
            }
        }
        this.shareLoot(lootFood, winners);
        this.shareLoot(lootWeapon, winners);
    }

    private void shareLoot(List<InventoryObject> loot, List<string> winners)
    {
        foreach (var item in loot)
        {
            int winnerId = Random.Range(0, winners.Count - 1);
            
            if (winnerId >= 0 && winnerId < winners.Count)
            {
                Debug.Log("win Loot: " + item.name);
                this.characters[winners[winnerId]].V1.inventory.food.Add(item);
            }
        }
    }

    private void collectSpecificLoot(List<InventoryObject> loot, List<InventoryObject> items, DestroyedStatus status, float ratio)
    {
        int looted = 0;

        foreach (var item in items)
        {
            if (item.type == "Quest")
            {
                loot.Add(item);
            }
            else if (status == DestroyedStatus.DESTROY_SHIP && Random.value <= (0.2f * ratio))
            {
                loot.Add(item);
                ++looted;
            }
            else if (status == DestroyedStatus.KILL_MEMBERS && Random.value <= (0.5f * ratio))
            {
                loot.Add(item);
                ++looted;
            }
            else if (status == DestroyedStatus.NONE && Random.value <= (0.1f * ratio))
            {
                loot.Add(item);
                ++looted;
            }
        }
    }


    /** GETTERS **/
    public Battle_Ship getShip(string id)
    {

        foreach (var ship in this.ships)
        {
            if (ship.getId() == id)
            {
                return ship;
            }
        }
        return null;
    }

    public bool isEndOfTheGame()
    {
        return this.endOfTheGame;
    }

    /** SINGLETON **/
    private static GameRulesManager instance = null;

    public static GameRulesManager GetInstance()
    {
        if (instance == null)
        {
            instance = new GameRulesManager();
        }
        return instance;
    }
}
