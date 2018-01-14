using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public string SceneIntroMenuName = "Intro_Scene";
    public string SceneWorldMapName = "World_Map_Scene";
    public string SceneInteractionName = "Interaction_Scene";
    public string SceneFightName = "Fight";
    public Inventory inventory;
    public static GameManager Instance = null;
    public PlayerManager playerManager = PlayerManager.GetInstance();
    public IslandManager islandManager;
    private int inGame;
    public int xMapSize, yMapSize, islandsAmount;
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

            islandManager = IslandManager.GetInstance();
            enemyAI = new EnemyAI();
            islandGenerator = new IslandGenerator();
            qgen = new QuestGenerator();

            mapGenerator = new MapGenerator();
            mapGenerator.spawnMap(xMapSize, yMapSize, islandsAmount);
        }

        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

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
    int turnCount = 0;

    public void nextTurn()
    {
        turnCount += 1;

        //Enemies turn
        if (turnCount % 2 == 0)
        {
            foreach (Player enemy in playerManager.enemies)
            {
                enemyAI.enemyTurn(enemy);
            }
        }

        //Gen new enemies

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

    }
    #endregion

}
