using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesRoom : RoomController
{
    public override void DrawRoomInteriors()
    {
        //First we remove the objects that are not valid for this room
        auxDungeonRoomInteriors = ApplySizeConditionsToObjects(clsDungeonController.dungeonRoomInteriors);
        for (int i = (int)roomRectangle.x; i < roomRectangle.xMax; i++)
        {
            for (int j = (int)roomRectangle.y; j < roomRectangle.yMax; j++)
            {
                RandomTools.SizeWeightedObject obj = RandomTools.Instance.PickOneSized(auxDungeonRoomInteriors);
                Vector3 tilePosition = new Vector3(i, j, 0f);
                if (obj.item != null && CheckAvailableSpace(obj.item, tilePosition))
                {
                    GameObject instance = Instantiate(obj.item, tilePosition, Quaternion.identity, roomInteriorsHolder);
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
