using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator {

    public List<List<MapTile>> island = new List<List<MapTile>>();

    public void generateIsland()
    {
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

    }

    public void spawnMap()
    {
        for (int x  = 1; x < island.Count - 1; ++x)
        {
            for (int y = 1; y < island[x].Count - 1; ++y)
            {
                //Debug.Log(island[x][y - 1].tileType + " " + island[x][y + 1].tileType + " " + island[x - 1][y].tileType + " " + island[x + 1][y].tileType);
                GameObject.Instantiate(island[x][y].getGraphicAsset(island[x][y + 1].tileType, island[x][y - 1].tileType, island[x - 1][y].tileType, island[x + 1][y].tileType), new Vector3(x * 50, y * 50, 10), new Quaternion());
            }
        }
    }

}
