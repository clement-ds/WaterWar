using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTile : MapTile {

    public WaterTile()
    {
        tileType = "Water";
        graphicAsset = Resources.Load("Tiles/WaterTile") as GameObject;
    }

    public override GameObject getGraphicAsset(string top, string bottom, string left, string right)
    {
        return graphicAsset;
    }
}
