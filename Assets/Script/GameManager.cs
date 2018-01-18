using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{

    public string SceneIntroMenuName = "Intro_Scene";
    public string SceneWorldMapName = "World_Map_Scene";
    public string SceneInteractionName = "Interaction_Scene";
    public string SceneFightName = "Fight";
    public Inventory inventory;
    public static GameManager Instance = null;
    public PlayerManager playerManager;
    public IslandManager islandManager;

    public Settings settings;
    private int inGame;
    public int xMapSize = 100;
    public int yMapSize = 100;
    public int islandsAmount = 8;
    private bool shouldCheckGameVictory = true;
    EnemyAI enemyAI;
    QuestGenerator qgen;
    IslandGenerator islandGenerator;

    public MapGenerator mapGenerator;

    void Awake()
    {
        if (Instance == null)
        {
            Debug.Log("GameManager: Creating Instance");
            Instance = this;
            this.SetIsInGame(0);

        }

        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    #region Main menu

    public void newGame()
    {
        playerManager = PlayerManager.GetInstance();
        islandManager = IslandManager.GetInstance(islandsAmount);
        mapGenerator = new MapGenerator();
        mapGenerator.spawnMap(xMapSize, yMapSize, islandsAmount);

        enemyAI = new EnemyAI();
        islandGenerator = new IslandGenerator();
        qgen = new QuestGenerator();

        GoIntroMenu();
    }

    public void continueGame()
    {
        playerManager = PlayerManager.GetInstance(false);
        islandManager = IslandManager.GetInstance(islandsAmount, false);
        mapGenerator = new MapGenerator();
        mapGenerator.loadMap(xMapSize, yMapSize);

        enemyAI = new EnemyAI();
        islandGenerator = new IslandGenerator();
        qgen = new QuestGenerator();

        GoIntroMenu();
    }

    public void SaveGame()
    {
        playerManager.Save();
        playerManager.SaveAI();
        islandManager.Save();
        mapGenerator.Save();
    }

    #endregion

    public void SetIsInGame(int inGame)
    {
        Instance.inGame = inGame;

    }

    public void displayMap(GameObject root)
    {
        mapGenerator.displayMap(root);
    }

    public int IsInGame()
    {
        return Instance.inGame;
    }

    #region Navigation
    public void GoIntroMenu()
    {
        Instance.ChangeScene(SceneIntroMenuName);
    }

    public void GoWorldMap()
    {
        Instance.ChangeScene(SceneWorldMapName);
    }

    public void GoInteraction()
    {
        Instance.ChangeScene(SceneInteractionName);
    }

    public void GoFight()
    {
        GameRulesManager.GetInstance().init();
        Instance.ChangeScene(SceneFightName);
    }


    void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    #endregion

    #region Turn


    public void spawnShips(GameObject mapPivot)
    {
        Island island = islandManager.islands[playerManager.player.currentIsland];
        if (playerManager.player.mapShip != null)
        {
            Destroy(playerManager.player.mapShip);
        }

        playerManager.player.mapShip = GameObject.Instantiate(playerManager.player.graphicAsset, (new Vector3(island.x * 50, island.y * 50, 8)), new Quaternion());
        TextMeshProUGUI textMeshProUGUI = playerManager.player.mapShip.GetComponentInChildren<TextMeshProUGUI>();
        textMeshProUGUI.SetText(playerManager.player.name);
        playerManager.player.mapShip.transform.SetParent(mapPivot.transform, false);

        int playerX = 0;
        int playerY = 0;
        foreach (Player enemy in playerManager.enemies)
        {
            playerX += 150;
            island = islandManager.islands[enemy.currentIsland];
            if (enemy.mapShip != null)
            {
                Destroy(enemy.mapShip);
            }
            enemy.mapShip = GameObject.Instantiate(enemy.graphicAsset, (new Vector3(island.x * 50 + playerX, island.y * 50 + playerY, 8)), new Quaternion());
            textMeshProUGUI = enemy.mapShip.GetComponentInChildren<TextMeshProUGUI>();
            textMeshProUGUI.SetText(enemy.name);
            enemy.mapShip.transform.SetParent(mapPivot.transform, false);
        }

        SaveGame();
    }

    int turnCount = 0;

    public void nextTurn()
    {
        turnCount += 1;

        Player player = playerManager.player;

        //Player position
        Island islandP = islandManager.islands[player.currentIsland];
        Vector3 pos = new Vector3(islandP.x * 50, islandP.y * 50, 8);
        player.mapShip.transform.localPosition = pos;

        //Enemies turn
        if (turnCount % 2 == 0)
        {
            foreach (Player enemy in playerManager.enemies)
            {
                enemyAI.enemyTurn(enemy);
            }
            int i = 0;
            foreach (Island island in islandManager.islands)
            {
                int playerX = 0;
                int playerY = 0;
                foreach (Player enemy in playerManager.enemies)
                {
                    if (enemy.currentIsland == i)
                    {
                        playerX += 150;
                        Vector3 pos1 = new Vector3(island.x * 50 + playerX, island.y * 50 + playerY, 8);
                        enemy.mapShip.transform.localPosition = pos1;
                    }
                }
                i += 1;
            }
        }

        //Meals and wages
        foreach (CrewMember crew in playerManager.player.crew.crewMembers)
        {
            if (player.inventory.food.Count >= 1)
            {
                player.inventory.removeObject(player.inventory.food[0]);
            } else
            {
                crew.morale = false;
            }
            if (player.money >= (int) crew.wage)
            {
                player.money -= (int)crew.wage;
            } else
            {
                crew.morale = false;
            }
        }

        //Gen new enemies
        if (turnCount % 5 == 0)
        {
            playerManager.enemies.Add(new Player());
        }

        //Island iventory refresh
        if (turnCount % 5 == 0)
        {
            foreach (Island island in islandManager.islands)
            {
                islandGenerator.GenerateFood(island, 3);
            }
        }

        //Island quests refresh
        if (turnCount % 5 == 0)
        {
            foreach (Island island in islandManager.islands)
            {
                qgen.GenerateQuest(island);
            }
        }

        //Check for Game Victory
        IntroSceneManager sceneManager = GameObject.Find("SceneManager").GetComponent<IntroSceneManager>();
        string victoryString = CheckGameVictory();
        if (shouldCheckGameVictory && victoryString != "") {
            shouldCheckGameVictory = false;
            if (sceneManager) {
                sceneManager.DisplayWinCanvas(victoryString);
            }
            return ;
        }

        //Display influence flag
        sceneManager.DisplayInfluenceFlag(islandManager.islands[playerManager.player.currentIsland].influence == 100);


        SaveGame();

        foreach (Player enemy in playerManager.enemies)
        {
            if (enemy.currentIsland == playerManager.player.currentIsland)
            {
                GoFight();
                //Je sais pas si il y a besoin mais au cas ou
                break;
            }
        }
    }

    private string CheckGameVictory() {
        IEnumerable<Island> islandWithTopInfluance = islandManager.islands.Where(island => island.influence == 100);

        if (playerManager.player.money > 10000) {
            return "You're rich!";
        }
        if (playerManager.enemies.Count == 0) {
            return "Everybody around you is dead";
        }
        if (islandWithTopInfluance.Count() >= (islandsAmount / 2)) {
            return "Everybody loves you!";
        }

        return "";
    }
    #endregion

}
