using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public string SceneIntroMenuName = "Intro_Scene";
    public string SceneWorldMapName = "World_Map_Scene";
    public string SceneInteractionName = "Interaction_Scene";
    public string SceneFightName = "Fight";
    public Inventory inventory;
    public static GameManager Instance = null;
    public PlayerManager playerManager = PlayerManager.GetInstance();
    private int inGame;

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

    public void SetIsInGame(int inGame)
    {
        Instance.inGame = inGame;

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

}
