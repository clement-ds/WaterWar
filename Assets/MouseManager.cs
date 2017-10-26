using UnityEngine;
using System.Collections.Generic;

public enum ECursor
{
    BASIC,
    SEARCH_TARGET,
    FOCUS_TARGET,
    ATTACK,
    REPAIR
}

public class MouseManager
{
    private Dictionary<ECursor, Texture2D> textures;
    public CursorMode cursorMode;
    public Vector2 hotSpot;
    private ECursor current;

    private MouseManager()
    {
        this.cursorMode = CursorMode.Auto;
        this.hotSpot = new Vector2(30, 30);
        this.textures = new Dictionary<ECursor, Texture2D>();
        this.textures.Add(ECursor.BASIC, null);
        this.textures.Add(ECursor.SEARCH_TARGET, Resources.Load("Sprites/Mouse/target1a") as Texture2D);
        this.textures.Add(ECursor.FOCUS_TARGET, Resources.Load("Sprites/Mouse/target2a") as Texture2D);
    }

    public void setCursor(ECursor id)
    {
        Cursor.SetCursor(textures[id], hotSpot, cursorMode);
        this.current = id;
    }

    public ECursor getCursorTexture()
    {
        return this.current;
    }

    private static MouseManager instance = null;
    public static MouseManager getInstance()
    {
        if (instance == null)
        {
            instance = new MouseManager();
        }
        return instance;
    }
}
