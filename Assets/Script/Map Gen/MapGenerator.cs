using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IslandGraphic = System.Collections.Generic.List<System.Collections.Generic.List<MapTile>>;
using SeaGraphic = System.Collections.Generic.List<System.Collections.Generic.List<MapTile>>;
using WorldMap = System.Collections.Generic.List<System.Collections.Generic.List<MapTile>>;

public class MapGenerator {


    public WorldMap worldMap = new WorldMap();
    int worldMapXSize = 200;
    int worldMapYSize = 100;
    bool isMapGenerated = false;
    public int islandsAmount = 8;
    int islandXMaxRange = 25;
    int islandYMaxRange = 25;

    public IslandGraphic generateIsland()
    {
        IslandGraphic island = new IslandGraphic();

        int xMaxRange = UnityEngine.Random.Range(5, islandXMaxRange - 10);
        int yMaxRange = islandYMaxRange;

        island.Add(new List<MapTile>());
        for (int y = 0; y < yMaxRange; y++)
        {
            island[0].Add(new WaterTile());
        }
        int previousStartCell = 0;
        for (int x = 1; x < xMaxRange; x++)
        {
            int minY;
            if (x <= xMaxRange / 2)
            {
                minY = x + 1;
            }
            else
            {
                minY = (xMaxRange - x) + 1;
            }

            int yRange = UnityEngine.Random.Range(minY, minY + 4);

            int startCell;
            if (x == 1)
            {
                startCell = UnityEngine.Random.Range(1, 4);
            }
            else
            {
                if (previousStartCell == 1)
                {
                    startCell = UnityEngine.Random.Range(previousStartCell, previousStartCell + 2);
                } else
                {
                    startCell = UnityEngine.Random.Range(previousStartCell - 1, previousStartCell + 2);
                }
            }

            while (startCell + yRange >= xMaxRange - 1)
            {
                if (startCell >= previousStartCell)
                {
                    startCell--;
                } else
                {
                    yRange--;
                }
            }
            previousStartCell = startCell;

            island.Add(new List<MapTile>());
            for (int y = 0; y < startCell; y++)
            {
                island[x].Add(new WaterTile());
            }
            for (int y = 0; y < yRange; y++)
            {
                island[x].Add(new IslandTile());
            }
            for (int y = island[x].Count; y < yMaxRange; y++)
            {
                island[x].Add(new WaterTile());
            }
        }

        for (int x = xMaxRange; x < islandXMaxRange; x++)
        {
            island.Add(new List<MapTile>());
            for (int y = 0; y < yMaxRange; y++)
            {
                island[x].Add(new WaterTile());
            }
        }
        
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            return generateAlternateIsland(island);
        }
        return island;
    }

    IslandGraphic generateAlternateIsland(IslandGraphic island)
    {
        IslandGraphic islandAlt = new IslandGraphic();
        for (int x = 0; x < island.Count; x++)
        {
            islandAlt.Add(new List<MapTile>());
            for (int y = 0; y < island[x].Count; y++)
            {
                islandAlt[x].Add(null);
            }
        }
        for (int x = 0; x < island.Count; x++)
        {
            for (int y = 0; y < island[x].Count; y++)
            {
                islandAlt[y][x] = island[x][y];
            }
        }
        return islandAlt;
    }

    public void generateWorldMap()
    {
        for (int x = 0; x < worldMapXSize; x++)
        {
            worldMap.Add(new List<MapTile>());
            for (int y = 0; y < worldMapYSize; y++)
            {
                worldMap[x].Add(new WaterTile());
            }
        }
    }

    public void addIslandToMap(int x, int y, IslandGraphic island)
    {
        for (int xIndex = 0; xIndex < island.Count; xIndex++)
        {
            for (int yIndex = 0; yIndex < island[xIndex].Count; yIndex++)
            {
                worldMap[xIndex + x][yIndex + y] = island[xIndex][yIndex];
            }
        }
    }

    bool checkLocationForIsland(int x, int y)
    {
        for (int xIndex = x; xIndex < x + islandXMaxRange; xIndex++)
        {
            for (int yIndex = y; yIndex < y + islandYMaxRange; yIndex++)
            {
                if (worldMap[xIndex][yIndex].tileType == "Sand")
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void spawnMapLoop() {
        generateWorldMap();

        for (int i = 0; i < islandsAmount; i++)
        {
            IslandGraphic island = generateIsland();
            foreach (List<MapTile> column in island)
            {
                foreach (MapTile tile in column)
                {
                    tile.islandID = i;
                }
            }
            int xSpawn = UnityEngine.Random.Range(1, worldMapXSize - 25);
            int ySpawn = UnityEngine.Random.Range(1, worldMapYSize - 25);
            int repeat = 0;
            while (checkLocationForIsland(xSpawn, ySpawn) && repeat < 10) {
                xSpawn = UnityEngine.Random.Range(1, worldMapXSize - 25);
                ySpawn = UnityEngine.Random.Range(1, worldMapYSize - 25);
                Debug.Log(xSpawn + " " + ySpawn);
                repeat++;
            }
            addIslandToMap(xSpawn, ySpawn, island);
        }
        isMapGenerated = true;
    }

    public void spawnMap()
    {
        spawnMapLoop();
    }

    public void spawnMap(int xSize, int ySize, int islandsAmount = 4)
    {
        worldMapXSize = xSize;
        worldMapYSize = ySize;
        this.islandsAmount = islandsAmount;
        spawnMapLoop();
    }

    public void displayMap(GameObject parent)
    {
        if (isMapGenerated) {

            for (int x = 0; x < worldMapXSize; x++)
            {
                for (int y = 0; y < worldMapYSize; y++)
                {
                    if (x == 0 || y == 0 || x == worldMapXSize - 1 || y == worldMapYSize - 1)
                    {
                        GameObject tile = GameObject.Instantiate(worldMap[x][y].getGraphicAsset("", "", "", ""), (new Vector3(x * 50, y * 50, 10)), new Quaternion());
                        tile.GetComponent<TileClick>().islandID = worldMap[x][y].islandID;
                        //tile.transform.localScale = parent.transform.localScale;
                        //tile.transform.parent = parent.transform;
                        tile.transform.SetParent(parent.transform, false);
                    }
                    else
                    {
                        //Debug.Log(worldMap[x][y - 1].tileType + " " + worldMap[x][y + 1].tileType + " " + worldMap[x - 1][y].tileType + " " + worldMap[x + 1][y].tileType);
                        GameObject tile = GameObject.Instantiate(worldMap[x][y].getGraphicAsset(worldMap[x][y + 1].tileType, worldMap[x][y - 1].tileType, worldMap[x - 1][y].tileType, worldMap[x + 1][y].tileType), (new Vector3(x * 50, y * 50, 10)), new Quaternion());
                        tile.GetComponent<TileClick>().islandID = worldMap[x][y].islandID;
                        tile.transform.SetParent(parent.transform, false);
                    }
                }
            }
        } else {
            Debug.LogWarning("Map not generated yet /!\\");
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
                yCin++;
            } else
            {
                yCin = 0;
                xCin++;
            }  
        }
    }
}
