using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Battle_Player : MonoBehaviour {
    public Slider slider = null;
    public Text distanceText = null;
    public Button actionFuite = null;
    public Button actionAbordage = null;

    private int life = 100;
    private int position = 2;


    // Use this for initialization
    void Start () {
        slider.value = life;
        distanceText.text = position.ToString();
        actionFuite.gameObject.SetActive(false);
        actionAbordage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        slider.value = life;
        distanceText.text = position.ToString();
        if (position == 3) {
            actionFuite.gameObject.SetActive(true); 
            actionAbordage.gameObject.SetActive(false);
        } else if (position == 1) {
            actionFuite.gameObject.SetActive(false);
            actionAbordage.gameObject.SetActive(true);
        } else {
            actionFuite.gameObject.SetActive(false);
            actionAbordage.gameObject.SetActive(false);
        }

    }

    public int getCurrentLife() {
        return life;
    }

    public void approcheAction() {
        if (position > 1) position--;
    }

    public void eloignementAction() {
        if (position < 3) position++;
    }

}
