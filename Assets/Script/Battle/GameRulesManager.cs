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
        this.guiAccess = GameObject.Find("Battle_UI").GetComponent<GuiAccess>();
    }

    public void init()
    {
        this.endOfTheGame = false;
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
