using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureRoom : RoomController
{
    public override void DrawRoomInteriors()
    {
        Vector3 treasurePosition = clsDungeonController.treasure.transform.position + new Vector3(Mathf.Floor(roomRectangle.center.x), Mathf.Floor(roomRectangle.center.y));
        Instantiate(clsDungeonController.treasure, treasurePosition, Quaternion.identity, roomTreasureHolder);
        base.DrawRoomInteriors();
    }

    public override void ActivateRoom()
    {
        if (!isCompleted)
        {
            SpawnObject(roomEnemies[0].enemyType, isEnemy: true);
            DisableGateways();
        }
        base.ActivateRoom();
    }

}
