using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public string SceneIntroMenuName = "Intro_Scene";
    public string SceneWorldMapName = "World_Map_Scene";
    public string SceneInteractionName = "Interaction_Scene";
    public Inventory inventory;
    public static GameManager Instance = null;


    void Awake()
    {
        if (Instance == null)
            Instance = this;

        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
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

    void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    #endregion

}
