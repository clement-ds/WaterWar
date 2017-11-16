using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandTile : MapTile {

    public IslandTile()
    {
        tileType = "Sand";
        graphicAsset = Resources.Load("Tiles/SandTile") as GameObject;
        
    }
}
