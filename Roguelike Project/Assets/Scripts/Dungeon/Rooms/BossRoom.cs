using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : RoomController
{
    public GameObject firstBossFloor;
    public GameObject secondBossFloor;
    public GameObject roomClosedWall;

    public void Initialize(DungeonController dungeonController, GameObject[,] tiles, DungeonController.DungeonEnemy[] enemies, int id)
    {
        clsDungeonController = dungeonController;
        roomEnemies = enemies;
        this.tiles = tiles;
        this.id = id;
    }

    public override void DrawRoomInteriors()
    {
        Debug.Log("Drawing Boss Room Interior");
    }

    public override void ActivateRoom()
    {
        base.ActivateRoom();
        roomClosedWall.SetActive(true);
    }
}
