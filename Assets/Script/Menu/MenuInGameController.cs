using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuInGameController : MonoBehaviour {
    public Button StartMenu;
    public Button Quit;
    private GameManager gameManager;
    public Button Back;

    void Start() {
        gameManager = new GameManager();
        StartMenu.gameObject.SetActive(false);
        Quit.gameObject.SetActive(false);
        Back.gameObject.SetActive(false);

        StartMenu.onClick.AddListener(goToStartMenu);
        Quit.onClick.AddListener(QuitGame);
        Back.onClick.AddListener(invertMenu);
    }

    private void goToStartMenu() {
        gameManager.GoStartMenu();
    }

    private void QuitGame() {
        Application.Quit();
        this.invertMenu();
    }

	void Update () {
        if (Input.GetKey(KeyCode.Escape)) {
            this.invertMenu();
        }
	}

    public void invertMenu() {
        StartMenu.gameObject.SetActive(!StartMenu.gameObject.activeSelf);
        Quit.gameObject.SetActive(!Quit.gameObject.activeSelf);
        Back.gameObject.SetActive(!Back.gameObject.activeSelf);
    }
}
