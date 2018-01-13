using System;
using System.Collections.Generic;
using UnityEngine;

public class GameRulesManager
{

    private static GameRulesManager instance = null;

    public bool endOfTheGame;
    public GuiAccess guiAccess;

    public List<Battle_Ship> ships;

    protected GameRulesManager()
    {
        this.endOfTheGame = false;
        this.ships = new List<Battle_Ship>();
    }

    public void init()
    {
        this.endOfTheGame = false;
    }

    public void initializeGui()
    {
        this.guiAccess = GameObject.Find("Battle_UI").GetComponent<GuiAccess>();
    }

    public static GameRulesManager GetInstance()
    {
        if (instance == null)
        {
            instance = new GameRulesManager();
        }
        return instance;
    }
    
    public Battle_Ship getShip(string id)
    {

        foreach (var ship in this.ships)
        {
            if (ship.getId() == id)
            {
                return ship;
            }
        }
        return null;
    }
}
