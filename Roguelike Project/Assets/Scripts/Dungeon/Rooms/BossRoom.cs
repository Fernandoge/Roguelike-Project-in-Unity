using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : RoomController
{
    public override void DrawRoomInteriors()
    {
        Debug.Log("Drawing Boss Room Interior");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            clsDungeonController.currentRoom = id;
            if (!isCompleted)
            {
                ActivateGateways();
                //spawn boss
            }
        }
    }
}
