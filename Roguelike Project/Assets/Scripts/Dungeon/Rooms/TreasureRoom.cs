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
        //Spawn Treasure
        Vector3 treasurePosition = clsDungeonController.treasure.transform.position + new Vector3(Mathf.Floor(roomRectangle.center.x), Mathf.Floor(roomRectangle.center.y) - 1);
        GameObject treasureInstance = Instantiate(clsDungeonController.treasure, treasurePosition, Quaternion.identity, roomTreasureHolder);
        tiles[(int)treasureInstance.transform.position.x, (int)treasureInstance.transform.position.y] = treasureInstance;
        /*
        foreach (Transform child in treasureInstance.transform)
        {
            tiles[(int)child.position.x, (int)child.position.y] = child.gameObject;
        }
        */

        //Fill room tiles with floors
        base.DrawRoomInteriors();
    }

    public override void ActivateRoom()
    {
        base.ActivateRoom();
    }

}
