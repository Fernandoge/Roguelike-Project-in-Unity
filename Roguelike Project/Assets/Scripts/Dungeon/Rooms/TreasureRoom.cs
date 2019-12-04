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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            clsDungeonController.currentRoom = id;
            if (!isCompleted)
            {
                //spawn treasure
            }
        }
    }
}
