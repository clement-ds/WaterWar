using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapTile {

    public string tileType;
    public GameObject graphicAsset;

    public abstract GameObject getGraphicAsset(string top, string bottom, string left, string right);

}
