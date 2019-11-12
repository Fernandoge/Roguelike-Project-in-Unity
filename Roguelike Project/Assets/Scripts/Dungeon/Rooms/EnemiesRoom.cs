using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesRoom : RoomController
{
    public override void DrawRoomInteriors()
    {
        RandomTools.WeightedSizedObject objectToInstantiate;
        //we loop the tiles to instantiate floors
        for (int i = (int)roomRectangle.x; i < roomRectangle.xMax; i++)
        {
            for (int j = (int)roomRectangle.y; j < roomRectangle.yMax; j++)
            {
                objectToInstantiate = RandomTools.Instance.PickOneSized(clsDungeonController.dungeonRoomInteriors);
                if (objectToInstantiate.item != null && CheckAvailableSpace(i, j, objectToInstantiate.tilesAvailableBelow, objectToInstantiate.tilesAvailableBeside))
                {
                    roomInteriorsPosition[i, j] = Instantiate(objectToInstantiate.item, new Vector3(i, j, 0f), Quaternion.identity, roomInteriorsHolder);
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
