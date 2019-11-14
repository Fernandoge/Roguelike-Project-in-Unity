﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesRoom : RoomController
{
    public override void DrawRoomInteriors()
    {
        RandomTools.WeightedSizedObject obj = null;
        GameObject instance;
        for (int i = (int)roomRectangle.x; i < roomRectangle.xMax; i++)
        {
            for (int j = (int)roomRectangle.y; j < roomRectangle.yMax; j++)
            {
                obj = RandomTools.Instance.PickOneSized(auxDungeonRoomInteriors);
                if (obj.item != null && CheckAvailableSpace(i, j, obj.tilesAvailableAbove, obj.tilesAvailableBelow, obj.tilesAvailableLeft, obj.tilesAvailableRight))
                {
                    instance = Instantiate(obj.item, new Vector3(i, j, 0f), Quaternion.identity, roomInteriorsHolder);
                    roomInteriorsPosition[i, j] = instance;
                    foreach (Transform child in roomInteriorsPosition[i, j].transform)
                    {
                        roomInteriorsPosition[(int)child.transform.position.x, (int)child.transform.position.y] = instance;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            clsDungeonController.currentRoom = id;
            if (!isCompleted)
            {
                ActivateGateways();
                Invoke("SpawnEnemies", 2f);
            }
        }
    }
}
