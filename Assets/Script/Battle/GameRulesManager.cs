using System;
using System.Collections.Generic;
using UnityEngine;

public class GameRulesManager
{

    private static GameRulesManager instance = null;

    public bool endOfTheGame;
    public GuiAccess guiAccess;

    protected GameRulesManager()
    {
        this.endOfTheGame = false;
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
}
