using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        GameRulesManager.GetInstance().guiAccess.endPanel.gameObject.SetActive(true);
        GameRulesManager.GetInstance().guiAccess.endPanel.sprite = Resources.Load<Sprite>((status == GameStatus.VICTORY ? "Sprites/panelVictory" : "Sprites/panelDefeat"));

        Debug.Log("sprite: " + (status == GameStatus.VICTORY ? "panelVictory" : "panelDefeat") + ": " + GameRulesManager.GetInstance().guiAccess.endPanel.sprite);

        Dictionary<string, int> playerLoot = new Dictionary<string, int>();
        shareLoot(playerLoot);
        printPlayerGUILoot(status, playerLoot);
    }

    private void printPlayerGUILoot(GameStatus status, Dictionary<string, int> playerLoot)
    {
        if (playerLoot.Count == 0)
        {
            GameRulesManager.GetInstance().guiAccess.noLoot.gameObject.SetActive(true);
            GameRulesManager.GetInstance().guiAccess.noLoot.text = (status == GameStatus.VICTORY ? "Captain ! They is no treasure there !" : "Your crew defended well your gold");

            GameRulesManager.GetInstance().guiAccess.lootListView.gameObject.SetActive(false);
        }
        else
        {
            GameRulesManager.GetInstance().guiAccess.noLoot.gameObject.SetActive(false);
            GameRulesManager.GetInstance().guiAccess.lootListView.gameObject.SetActive(true);
            SimpleObjectPool generator = GameObject.Find("LootItemPool").GetComponent<SimpleObjectPool>();
            foreach (var loot in playerLoot)
            {
                GameObject item = generator.GetObject();

                item.GetComponent<ManageLootLine>().initLootLine(loot.Key, loot.Value, (status == GameStatus.VICTORY ? new Color(75, 167, 45) : Color.red));
                item.transform.SetParent(GameRulesManager.GetInstance().guiAccess.contentLootListTransform, false);
            }
        }
    }

    private void shareLoot(Dictionary<string, int> playerLoot)
    {
        List<InventoryObject> lootFood = new List<InventoryObject>();
        List<InventoryObject> lootWeapon = new List<InventoryObject>();

        List<string> winners = new List<string>();

        foreach (var character in this.characters)
        {
            if (character.Value.V2 != DestroyedStatus.ALIVE)
            {
                this.collectSpecificLoot(lootFood, character.Value.V1.inventory.food, character.Value.V2);
                this.collectSpecificLoot(lootWeapon, character.Value.V1.inventory.weapons, character.Value.V2);

                if (character.Key == "p")
                {
                    this.combineLoot(playerLoot, lootFood);
                    this.combineLoot(playerLoot, lootWeapon);
                    Debug.Log("player lost: " + playerLoot.Count);
                }
            }
            else
            {
                winners.Add(character.Key);
                Debug.Log("winner: " + character.Key);
            }
        }
        this.shareLootBetweenWinners(lootFood, winners, playerLoot);
        this.shareLootBetweenWinners(lootWeapon, winners, playerLoot);
    }

    private void shareLootBetweenWinners(List<InventoryObject> loot, List<string> winners, Dictionary<string, int> playerLoot)
    {
        foreach (var item in loot)
        {
            int winnerId = Random.Range(0, winners.Count - 1);
            
            if (winnerId >= 0 && winnerId < winners.Count)
            {
                Debug.Log("win Loot: " + item.name);
                this.characters[winners[winnerId]].V1.inventory.food.Add(item);

                if (winners[winnerId] == "p")
                {
                    this.combineLoot(playerLoot, loot);
                }
            }
        }
    }

    private void combineLoot(Dictionary<string, int> playerLoot, List<InventoryObject> loot)
    {
        foreach (var item in loot)
        {
            if (playerLoot.ContainsKey(item.name))
            {
                playerLoot[item.name] += 1;
            }
            else
            {
                playerLoot.Add(item.name, 1);
            }
        }
    }

    private void collectSpecificLoot(List<InventoryObject> loot, List<InventoryObject> items, DestroyedStatus status)
    {
        int looted = 0;

        for (var i = 0; i < items.Count; ++i)
        {
            if (items[i].type == "Quest")
            {
                loot.Add(items[i]);
                --i;
                items.RemoveAt(i);
            }
            else if (status == DestroyedStatus.DESTROY_SHIP && Random.value <= 0.2f)
            {
                loot.Add(items[i]);
                items.RemoveAt(i);
                --i;
                ++looted;
            }
            else if (status == DestroyedStatus.KILL_MEMBERS && Random.value <= 0.5f)
            {
                loot.Add(items[i]);
                items.RemoveAt(i);
                --i;
                ++looted;
            }
            else if (status == DestroyedStatus.NONE && Random.value <= 0.1f)
            {
                loot.Add(items[i]);
                items.RemoveAt(i);
                --i;
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
