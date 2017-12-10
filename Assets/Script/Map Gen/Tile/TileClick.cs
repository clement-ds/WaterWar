using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileClick : MonoBehaviour {

    public int islandID;

	// Use this for initialization
	void Start () {
        string tmp = name.Replace("(Clone)", "");
        Sprite sprite = Resources.Load("Tiles/Sprite/" + tmp, typeof(Sprite)) as Sprite;
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        print(islandID);
        if (islandID >= 0)
        {
            PlayerManager.GetInstance().player.currentIsland = int.Parse("" + islandID);
            GameManager.Instance.GoInteraction();
        }
        
    }
}
