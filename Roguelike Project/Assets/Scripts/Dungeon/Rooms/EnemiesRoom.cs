using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesRoom : RoomController
{
    private void Start()
    {
        roomEnemyPack = GetEnemyPack(clsDungeonController.enemyPacks);
    }

    public override void DrawRoomInteriors()
    {
        base.DrawRoomInteriors();
    }

    public override void ActivateRoom()
    {
        base.ActivateRoom();
    }
}
