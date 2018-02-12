using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Text;
using System.IO;
using MapColumn = System.Collections.Generic.List<MapTile>;
using IslandGraphic = System.Collections.Generic.List<MapGenerator.Column>;
using SeaGraphic = System.Collections.Generic.List<MapGenerator.Column>;
using WorldMap = System.Collections.Generic.List<MapGenerator.Column>;
using Assets.Script.Battle.Tools;

public class MapGenerator {

    public WorldMap worldMap = new WorldMap();
    int worldMapXSize = 200;
    int worldMapYSize = 100;
    bool isMapGenerated = false;
    public int islandsAmount = 8;
    int islandXMaxRange = 25;
    int islandYMaxRange = 25;

    [Serializable]
    public class Column
    {
        public MapColumn column = new MapColumn();
    }

    [Serializable]
    public class WorldMapSave
    {
        public List<Column> worldMap = new List<Column>();
    }

    public WorldMapSave worldMapSave = new WorldMapSave();

    public bool Save()
    {
        PlayerPrefs.SetString("MapSave", JsonUtility.ToJson(worldMapSave));
        PlayerPrefs.Save();
        //try
        //{
        //    StreamWriter writer = new StreamWriter("Assets/Resources/PlayerJson/MapSave.json", false);
        //    writer.Write(JsonUtility.ToJson(worldMapSave));
        //    writer.Close();
        //}
        //catch (Exception e)
        //{
        //    Debug.Log("SAVE MAP : " + e.Message);
        //    return false;
        //}
        return true;
    }

    public void loadMap(int xSize, int ySize)
    {
        worldMapSave = JsonUtility.FromJson<WorldMapSave>(PlayerPrefs.GetString("MapSave"));
        worldMap = worldMapSave.worldMap;
        isMapGenerated = true;
        worldMapXSize = xSize;
        worldMapYSize = ySize;
    }

    public IslandGraphic generateIsland()
    {
        IslandGraphic island = new IslandGraphic();

        int xMaxRange = UnityEngine.Random.Range(5, islandXMaxRange - 10);
        int yMaxRange = islandYMaxRange;

        island.Add(new Column());
        for (int y = 0; y < yMaxRange; y++)
        {
            island[0].column.Add(new WaterTile());
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

            island.Add(new Column());
            for (int y = 0; y < startCell; y++)
            {
                island[x].column.Add(new WaterTile());
            }
            for (int y = 0; y < yRange; y++)
            {
                island[x].column.Add(new IslandTile());
            }
            for (int y = island[x].column.Count; y < yMaxRange; y++)
            {
                island[x].column.Add(new WaterTile());
            }
        }

        for (int x = xMaxRange; x < islandXMaxRange; x++)
        {
            island.Add(new Column());
            for (int y = 0; y < yMaxRange; y++)
            {
                island[x].column.Add(new WaterTile());
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
            islandAlt.Add(new Column());
            for (int y = 0; y < island[x].column.Count; y++)
            {
                islandAlt[x].column.Add(null);
            }
        }
        for (int x = 0; x < island.Count; x++)
        {
            for (int y = 0; y < island[x].column.Count; y++)
            {
                islandAlt[y].column[x] = island[x].column[y];
            }
        }
        return islandAlt;
    }

    public void generateWorldMap()
    {
        for (int x = 0; x < worldMapXSize; x++)
        {
            worldMap.Add(new Column());
            for (int y = 0; y < worldMapYSize; y++)
            {
                worldMap[x].column.Add(new WaterTile());
            }
        }
    }

    public void addIslandToMap(int x, int y, IslandGraphic island)
    {
        for (int xIndex = 0; xIndex < island.Count; xIndex++)
        {
            for (int yIndex = 0; yIndex < island[xIndex].column.Count; yIndex++)
            {
                worldMap[xIndex + x].column[yIndex + y] = island[xIndex].column[yIndex];
            }
        }
    }

    bool checkLocationForIsland(int x, int y)
    {
        for (int xIndex = x; xIndex < x + islandXMaxRange; xIndex++)
        {
            for (int yIndex = y; yIndex < y + islandYMaxRange; yIndex++)
            {
                if (worldMap[xIndex].column[yIndex].tileType == "Sand")
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
            foreach (Column column in island)
            {
                foreach (MapTile tile in column.column)
                {
                    if (tile.tileType == "Sand")
                    {
                        tile.islandID = i;
                    }
                }
            }
            int xSpawn = UnityEngine.Random.Range(1, worldMapXSize - 25);
            int ySpawn = UnityEngine.Random.Range(1, worldMapYSize - 25);
            int repeat = 0;
            while (checkLocationForIsland(xSpawn, ySpawn) && repeat < 10) {
                xSpawn = UnityEngine.Random.Range(1, worldMapXSize - 25);
                ySpawn = UnityEngine.Random.Range(1, worldMapYSize - 25);
                repeat++;
            }
            IslandManager.GetInstance().islands[i].x = xSpawn;
            IslandManager.GetInstance().islands[i].y = ySpawn;
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
        worldMapSave.worldMap = worldMap;
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
                        GameObject tile = GameObject.Instantiate(worldMap[x].column[y].getGraphicAsset("", "", "", ""), (new Vector3(x * 50, y * 50, 10)), new Quaternion());
                        tile.GetComponent<TileClick>().islandID = worldMap[x].column[y].islandID;
                        tile.transform.SetParent(parent.transform, false);
                    }
                    else
                    {
                        GameObject tile = GameObject.Instantiate(worldMap[x].column[y].getGraphicAsset(worldMap[x].column[y + 1].tileType, worldMap[x].column[y - 1].tileType, worldMap[x - 1].column[y].tileType, worldMap[x + 1].column[y].tileType), (new Vector3(x * 50, y * 50, 10)), new Quaternion());
                        tile.GetComponent<TileClick>().islandID = worldMap[x].column[y].islandID;
                        tile.transform.SetParent(parent.transform, false);
                    }
                }
            }
            for (int i = 0; i < islandsAmount; i++)
            {
                Island island = IslandManager.GetInstance().islands[i];
                GameObject islandName = GameObject.Instantiate(Resources.Load("Tiles/IslandName") as GameObject, (new Vector3(island.x * 50, island.y * 50, 10)), new Quaternion());
                TextMeshProUGUI textMeshProUGUI = islandName.GetComponentInChildren<TextMeshProUGUI>();
                textMeshProUGUI.SetText(island.name);
                islandName.transform.SetParent(parent.transform, false);
            }
        } else {
            Debug.LogWarning("Map not generated yet /!\\");
        }
        
    }
}
