using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileClick : MonoBehaviour
{

    public int islandID;
    IntroSceneManager introSceneManager;

    // Use this for initialization
    void Start()
    {
        string tmp = name.Replace("(Clone)", "");
        int variation;
        if (tmp == "WaterTile")
        {
            int weight = UnityEngine.Random.Range(0, 50);
            variation = weight < 1 ?UnityEngine.Random.Range(2, 6) : 1;
        }
        else if (tmp == "Center")
        {
            variation = UnityEngine.Random.Range(1, 11);
        }
        else
        {
            variation = UnityEngine.Random.Range(1, 4);
        }

        Sprite sprite = Resources.Load("Tiles/Sprite/" + tmp + variation, typeof(Sprite)) as Sprite;
        GetComponent<SpriteRenderer>().sprite = sprite;
        introSceneManager = GameObject.Find("SceneManager").GetComponent<IntroSceneManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        print(islandID);
        if (islandID >= 0)
        {
            PlayerManager.GetInstance().player.currentIsland = int.Parse("" + islandID);
            GameManager.Instance.nextTurn();
            introSceneManager.CameraStateChange("Interaction");
        }

    }
}
