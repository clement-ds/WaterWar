using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Statistics : MonoBehaviour {

    private PlayerManager managerP;
    public Text gold;

    // Use this for initialization
    void Start () {
        managerP = PlayerManager.GetInstance();
        gold.text = managerP.player.money.ToString() + "£";
    }

	// Update is called once per frame
	void Update () {
        gold.text = managerP.player.money.ToString() + "£";
    }
}
