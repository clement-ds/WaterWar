using UnityEngine;
using System.Collections;

public class testManager : MonoBehaviour {

    PlayerManager pm = null;
    IslandManager im = null;
    public GameObject playerShip;

    // Use this for initialization
    void Start() {
        //pm = PlayerManager.GetInstance();
        im = IslandManager.GetInstance();

        Vector2 goodPos = PlayerManager.GetInstance().player.mapPosition;
        playerShip.transform.position = new Vector3(goodPos.x, goodPos.y, playerShip.transform.position.z);
        print("SHIP PUT IN POS " + goodPos);

        MapGenerator mg = new MapGenerator();
        mg.generateIsland();
        mg.spawnMap();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
