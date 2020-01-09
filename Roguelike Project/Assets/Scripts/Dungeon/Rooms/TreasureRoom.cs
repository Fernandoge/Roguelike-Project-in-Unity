using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureRoom : RoomController
{
    private void Start()
    {
        roomEnemyPack = GetEnemyPack(clsDungeonController.enemyPacks);
    }

    public override void DrawRoomInteriors()
    {
        base.DrawRoomInteriors();
        Vector3 treasurePosition = clsDungeonController.treasure.transform.position + new Vector3(Mathf.Floor(roomRectangle.center.x), Mathf.Floor(roomRectangle.center.y));
        GameObject treasureInstance = Instantiate(clsDungeonController.treasure, treasurePosition, Quaternion.identity, roomTreasureHolder);
        foreach (Transform child in treasureInstance.transform)
        {
            tiles[(int)child.position.x, (int)child.position.y] = child.gameObject;
        }
    }

    public override void ActivateRoom()
    {
        if (!isCompleted)
        {
            SpawnEnemies();
            DisableGateways();
        }
        base.ActivateRoom();
    }

}
