using System;
using System.Collections.Generic;

public class GameRulesManager
{

    private static GameRulesManager instance = null;
    public bool endOfTheGame;

    protected GameRulesManager()
    {
        this.endOfTheGame = false;
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
