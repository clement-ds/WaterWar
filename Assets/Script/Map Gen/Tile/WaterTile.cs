using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTile : MapTile {

    public GameObject waterGraphicAsset;

    public WaterTile()
    {
        tileType = "Water";
        graphicAsset = Resources.Load("Tiles/WaterTile") as GameObject;
    }
}
