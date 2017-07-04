using UnityEngine;
using System.Collections;

public class testManager : MonoBehaviour {

    PlayerManager pm = null;
    IslandManager im = null;

    // Use this for initialization
    void Start() {
        //pm = PlayerManager.GetInstance();
        im = IslandManager.GetInstance();
        Debug.Log("Generated island inventory : " + im.island.inventory.food[0].name + " " + im.island.inventory.food[0].quantity);
        Debug.Log("Generated island inventory : " + im.island.inventory.weapons[0].name + " " + im.island.inventory.weapons[0].quantity);
        Debug.Log("Generated island questlog : " + im.island.questLog.quests[0].description + " " + im.island.questLog.quests[0].objective);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
