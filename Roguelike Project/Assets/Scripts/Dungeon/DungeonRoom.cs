using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoom
{
    public GameObject room;
    public int id;
    public Rect roomRectangle;

    public DungeonRoom(GameObject room, int id, Rect roomRectangle)
    {
        this.room = room;
        this.id = id;
        this.roomRectangle = roomRectangle;
    }
}
