﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IslandGraphic = System.Collections.Generic.List<System.Collections.Generic.List<MapTile>>;
using SeaGraphic = System.Collections.Generic.List<System.Collections.Generic.List<MapTile>>;
using WorldMap = System.Collections.Generic.List<System.Collections.Generic.List<MapTile>>;

public class MapGenerator {


    public WorldMap worldMap = new WorldMap();
    int worldMapXSize = 100;
    int worldMapYSize = 100;

    public IslandGraphic generateIsland()
    {
        IslandGraphic island = new IslandGraphic();

        int xMaxRange = UnityEngine.Random.Range(5, 15);
        int yMaxRange = 15;

        island.Add(new List<MapTile>());
        for (int y = 0; y < yMaxRange; ++y)
        {
            island[0].Add(new WaterTile());
        }

        for (int x = 1; x < xMaxRange; ++x)
        {
            int yRange = UnityEngine.Random.Range(7, 11);
            int startCell = UnityEngine.Random.Range(1, 4);

            island.Add(new List<MapTile>());

            for (int y = 0; y < startCell; ++y)
            {
                island[x].Add(new WaterTile());
            }
            for (int y = startCell; y < yRange; ++y)
            {
                island[x].Add(new IslandTile());
            }
            for (int y = yRange; y < yMaxRange; ++y)
            {
                island[x].Add(new WaterTile());
            }
        }

        island.Add(new List<MapTile>());
        for (int y = 0; y < yMaxRange; ++y)
        {
            island[xMaxRange].Add(new WaterTile());
        }

        return island;

    }

    public void generateWorldMap()
    {
        for (int x = 0; x < worldMapXSize; ++x)
        {
            worldMap.Add(new List<MapTile>());
            for (int y = 0; y < worldMapYSize; ++y)
            {
                worldMap[x].Add(new WaterTile());
            }
        }
    }

    public void addIslandToMap(int x, int y, IslandGraphic island)
    {
        for (int xIndex = 0; xIndex < island.Count; ++xIndex)
        {
            for (int yIndex = 0; yIndex < island[xIndex].Count; ++yIndex)
            {
                worldMap[xIndex + x][yIndex + y] = island[xIndex][yIndex];
            }
        }
    }

    public void spawnMap()
    {
        generateWorldMap();

        for (int i = 0; i < 8; ++i)
        {
            IslandGraphic island = generateIsland();
            foreach (List<MapTile> column in island)
            {
                foreach (MapTile tile in column)
                {
                    tile.islandID = i;
                }
            }
            int xSpawn = UnityEngine.Random.Range(1, 75);
            int ySpawn = UnityEngine.Random.Range(1, 75);
            addIslandToMap(xSpawn, ySpawn, island);
        }

    }

    public void displayMap()
    {
        for (int x = 0; x < worldMapXSize; ++x)
        {
            for (int y = 0; y < worldMapYSize; ++y)
            {
                if (x == 0 || y == 0 || x == worldMapXSize - 1 || y == worldMapYSize - 1)
                {
                    GameObject.Instantiate(worldMap[x][y].getGraphicAsset("", "", "", ""), new Vector3(x * 50, y * 50, 10), new Quaternion());
                }
                else
                {
                    Debug.Log(worldMap[x][y - 1].tileType + " " + worldMap[x][y + 1].tileType + " " + worldMap[x - 1][y].tileType + " " + worldMap[x + 1][y].tileType);
                    GameObject tile = GameObject.Instantiate(worldMap[x][y].getGraphicAsset(worldMap[x][y + 1].tileType, worldMap[x][y - 1].tileType, worldMap[x - 1][y].tileType, worldMap[x + 1][y].tileType), new Vector3(x * 50, y * 50, 10), new Quaternion());
                    tile.GetComponent<TileClick>().islandID = worldMap[x][y].islandID;
                }
            }
        }
    }

    int xCin = 0;
    int yCin = 0;

    public void displayMapCinematic()
    {
        if (xCin < worldMapXSize)
        {
            if (yCin < worldMapYSize)
            {
                if (xCin == 0 || yCin == 0 || xCin == worldMapXSize - 1 || yCin == worldMapYSize - 1)
                {
                    Debug.Log(xCin + " " + yCin);
                    GameObject.Instantiate(worldMap[xCin][yCin].getGraphicAsset("", "", "", ""), new Vector3(xCin * 50, yCin * 50, 10), new Quaternion());
                }
                else
                {
                    Debug.Log(worldMap[xCin][yCin - 1].tileType + " " + worldMap[xCin][yCin + 1].tileType + " " + worldMap[xCin - 1][yCin].tileType + " " + worldMap[xCin + 1][yCin].tileType);
                    GameObject.Instantiate(worldMap[xCin][yCin].getGraphicAsset(worldMap[xCin][yCin + 1].tileType, worldMap[xCin][yCin - 1].tileType, worldMap[xCin - 1][yCin].tileType, worldMap[xCin + 1][yCin].tileType), new Vector3(xCin * 50, yCin * 50, 10), new Quaternion());
                }
                ++yCin;
            } else
            {
                yCin = 0;
                ++xCin;
            }  
        }
    }
}
