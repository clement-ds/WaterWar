using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class WaterTile : MapTile {

    public WaterTile()
    {
        tileType = "Water";
        graphicAsset = Resources.Load("Tiles/WaterTile") as GameObject;
    }

    public override GameObject getGraphicAsset(string top, string bottom, string left, string right)
    {
        return Resources.Load("Tiles/WaterTile") as GameObject;
    }
}
