using UnityEngine;
using System.Collections;
using System;

public class IAMapRoamer : MonoBehaviour {

    public long coolDown;
    private long previousTime;


	// Use this for initialization
	void Start () {
        previousTime = DateTime.Now.Ticks;
	}
	
	// Update is called once per frame
	void Update () {
        if (TimeSpan.FromTicks(DateTime.Now.Ticks - previousTime).TotalSeconds >= coolDown)
        {
           // Debug.Log("UPDATE IA");
            int nextIsland = UnityEngine.Random.Range(0, 8);
            Island island = IslandManager.GetInstance().islands[nextIsland];
           // Debug.Log("Going to " + island.name + "(" + nextIsland + ")"  + " in " + island.x + " - " + island.y);
            transform.position = new Vector3(island.x, island.y, 0);
            previousTime = DateTime.Now.Ticks;
        }
    }
}
