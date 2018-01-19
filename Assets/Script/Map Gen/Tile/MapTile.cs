using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MapTile {

    public string tileType;
    public GameObject graphicAsset;
    public int islandID = -1;
    List<TileAsset> tiles = new List<TileAsset>();

    public virtual GameObject getGraphicAsset(string top, string bottom, string left, string right)
    {
        return graphicAsset;
    }
}
