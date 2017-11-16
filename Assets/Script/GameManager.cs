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
    private int inGame = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            this.SetIsInGame(0);
        }

        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void SetIsInGame(int inGame)
    {
        this.inGame = inGame;
    }

    void OnDestroy()
    {
        SetIsInGame(0);
    }


    public int IsInGame()
    {
        return inGame;
    }

    #region Navigation
    public void GoIntroMenu()
    {
        ChangeScene(SceneIntroMenuName);
    }

    public void GoWorldMap()
    {
        ChangeScene(SceneWorldMapName);
    }

    public void GoInteraction()
    {
        ChangeScene(SceneInteractionName);
    }

    public void GoFight()
    {
        GameRulesManager.GetInstance().init();
        ChangeScene(SceneFightName);
    }


    void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    #endregion

}
