using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltFight : MonoBehaviour {

    PlayerManager pm;
    Player player1;
    Player player2;

	// Use this for initialization
	void Start () {
        this.pm = PlayerManager.GetInstance();
        getPlayers();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void getPlayers()
    {
        this.player1 = pm.player;
        this.player2 = pm.ai;
    }

    //SELECT UTILITY
    GameObject selectedCrew  = null;

    public void onSelect(GameObject go)
    {
        if (selectedCrew != null)
        {
            selectedCrew.transform.Find("Selection").gameObject.SetActive(false);
        }
        selectedCrew = go;
        selectedCrew.transform.Find("Selection").gameObject.SetActive(true);
    }

    //SPAWN UTILITY

}
