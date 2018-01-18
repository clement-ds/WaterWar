﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class IslandTile : MapTile {

    TileAsset topRightCorner;
    TileAsset bottomRightCorner;
    TileAsset topLeftCorner;
    TileAsset bottomLeftCorner;
    TileAsset rightSide;
    TileAsset topSide;
    TileAsset leftSide;
    TileAsset bottomSide;
    TileAsset center;
    List<TileAsset> tiles = new List<TileAsset>();

    public IslandTile()
    {
        tileType = "Sand";
        loadGraphicAsset();
    }

    public override GameObject getGraphicAsset(string top, string bottom, string left, string right)
    {
        foreach (TileAsset tile in tiles)
        {
            if (tile.top == top && tile.bottom == bottom && tile.left == left && tile.right == right)
            {
                graphicAsset = tile.asset;
                return tile.asset;
            }
        }
        Debug.Log("RETURNS NULL");
        return null;
    }

    public void loadGraphicAsset()
    {
        tiles = new List<TileAsset>();

        tiles.Add(new TileAsset("Water", "Sand", "Sand", "Water", Resources.Load("Tiles/TopRightCorner") as GameObject));
        tiles.Add(new TileAsset("Sand", "Water", "Sand", "Water", Resources.Load("Tiles/BottomRightCorner") as GameObject));
        tiles.Add(new TileAsset("Water", "Sand", "Water", "Sand", Resources.Load("Tiles/TopLeftCorner") as GameObject));
        tiles.Add(new TileAsset("Sand", "Water", "Water", "Sand", Resources.Load("Tiles/BottomLeftCorner") as GameObject));

        tiles.Add(new TileAsset("Sand", "Sand", "Sand", "Water", Resources.Load("Tiles/RightSide") as GameObject));
        tiles.Add(new TileAsset("Water", "Sand", "Sand", "Sand", Resources.Load("Tiles/TopSide") as GameObject));
        tiles.Add(new TileAsset("Sand", "Sand", "Water", "Sand", Resources.Load("Tiles/LeftSide") as GameObject));
        tiles.Add(new TileAsset("Sand", "Water", "Sand", "Sand", Resources.Load("Tiles/BottomSide") as GameObject));

        tiles.Add(new TileAsset("Sand", "Sand", "Sand", "Sand", Resources.Load("Tiles/Center") as GameObject));

        tiles.Add(new TileAsset("Water", "Water", "Water", "Sand", Resources.Load("Tiles/LeftTip") as GameObject));
        tiles.Add(new TileAsset("Water", "Water", "Sand", "Water", Resources.Load("Tiles/RightTip") as GameObject));
        tiles.Add(new TileAsset("Water", "Sand", "Water", "Water", Resources.Load("Tiles/TopTip") as GameObject));
        tiles.Add(new TileAsset("Sand", "Water", "Water", "Water", Resources.Load("Tiles/BottomTip") as GameObject));

        tiles.Add(new TileAsset("Water", "Water", "Sand", "Sand", Resources.Load("Tiles/StripHorizontal") as GameObject));
        tiles.Add(new TileAsset("Sand", "Sand", "Water", "Water", Resources.Load("Tiles/StripVertical") as GameObject));

        tiles.Add(new TileAsset("Water", "Water", "Water", "Water", Resources.Load("Tiles/Center") as GameObject));
    }
}

[Serializable]
public class TileAsset
{
    public string top;
    public string bottom;
    public string left;
    public string right;

    public GameObject asset;

    public TileAsset(string top, string bottom, string left, string right, GameObject asset)
    {
        this.top = top;
        this.bottom = bottom;
        this.left = left;
        this.right = right;
        this.asset = asset;
    }
}

