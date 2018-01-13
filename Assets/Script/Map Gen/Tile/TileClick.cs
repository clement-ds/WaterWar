using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileClick : MonoBehaviour {

    public int islandID;
    IntroSceneManager introSceneManager;

	// Use this for initialization
	void Start () {
        string tmp = name.Replace("(Clone)", "");
        int variation;
        if (tmp == "Center" || tmp == "WaterTile")
        {
            variation = UnityEngine.Random.Range(1, 6);
        } else
        {
            variation = UnityEngine.Random.Range(1, 4);
        }
        
        Sprite sprite = Resources.Load("Tiles/Sprite/" + tmp + variation, typeof(Sprite)) as Sprite;
        GetComponent<SpriteRenderer>().sprite = sprite;
        introSceneManager = GameObject.Find("SceneManager").GetComponent<IntroSceneManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        print(islandID);
        if (islandID >= 0)
        {
            GameManager.Instance.nextTurn();
            PlayerManager.GetInstance().player.currentIsland = int.Parse("" + islandID);
            //GameManager.Instance.GoInteraction();
            introSceneManager.CameraStateChange("Interaction");
        }
        
    }
}
