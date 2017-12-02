using UnityEngine;
using System.Collections;

public class testManager : MonoBehaviour {

    PlayerManager pm = null;
    IslandManager im = null;
    public GameObject playerShip;

    // Use this for initialization
    void Start() {
        //pm = PlayerManager.GetInstance();
        //im = IslandManager.GetInstance();

        //Vector2 goodPos = PlayerManager.GetInstance().player.mapPosition;
        //playerShip.transform.position = new Vector3(goodPos.x, goodPos.y, playerShip.transform.position.z);
        //print("SHIP PUT IN POS " + goodPos);

        mg.spawnMap();
        mg.displayMap();

        
    }
    MapGenerator mg = new MapGenerator();
    // Update is called once per frame
    void Update () {
        //mg.displayMapCinematic();
    }
}
