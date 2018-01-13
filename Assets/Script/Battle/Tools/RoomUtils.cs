using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RoomUtils
{
    public static List<RoomElement> Rooms;

    public static void reset()
    {
        Rooms = new List<RoomElement>();
    }

    public static RoomElement getRoom(string id)
    {
        foreach (RoomElement room in Rooms)
        {
            if (room.getId() == id)
            {
                return room;
            }
        }
        return null;
    }

    public static bool hasRoute(RoomElement start, RoomElement end)
    {
        return findRoute(start, end, new List<string>());
    }

    public static List<Vector3> getRoute(RoomElement start, RoomElement end)
    {
        List<string> path = new List<string>();
        List<Vector3> route = new List<Vector3>();

        //Debug.Log(start.getId() + " to " + end.getId() + " : " + path.Count);
        if (findRoute(start, end, path))
        {
            foreach (string id in path)
            {
                //Debug.Log("->" + id);
                route.Add(getRoom(id).transform.localPosition);
            }
        }
        return route;
    }

    private static bool findRoute(RoomElement start, RoomElement end, List<string> history)
    {
        List<string> newHistory = new List<string>();
        history.Add(start.getId());


        newHistory.AddRange(history);
        int historyCount = history.Count;

        if (start.getId() == end.getId())
        {
            return true;
        }

        bool result = false;
        foreach (string id in start.getLinks())
        {
            if (newHistory.FirstOrDefault(stringToCheck => stringToCheck.Contains(id)) == null)
            {
                RoomElement current = getRoom(id);

                if (current)
                {
                    List<string> tmp = new List<string>();
                    tmp.AddRange(newHistory);

                    if (findRoute(current, end, tmp))
                    {
                        if (history.Count == historyCount || tmp.Count < history.Count)
                        {
                            history.RemoveRange(0, history.Count);
                            history.AddRange(tmp);
                        }
                        result = true;
                    }
                }
            }
        }
        return result;
    }
}
