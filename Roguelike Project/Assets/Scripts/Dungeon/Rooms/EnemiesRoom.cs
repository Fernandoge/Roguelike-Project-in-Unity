using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesRoom : RoomController
{
    public override void DrawRoomInteriors()
    {
        //First we remove the objects that are not valid for this room
        auxDungeonRoomInteriors = ApplySizeConditionsToObjects(clsDungeonController.dungeonRoomInteriors);
        //Instantiate room interiors checking if there is enough space to instantiate them
        for (int i = (int)roomRectangle.x; i < roomRectangle.xMax; i++)
        {
            for (int j = (int)roomRectangle.y; j < roomRectangle.yMax; j++)
            {
                RandomTools.SizeWeightedObject obj = RandomTools.Instance.PickOneSized(auxDungeonRoomInteriors);
                Vector3 tilePosition = new Vector3(i, j, 0f);
                if (obj.item != null && CheckAvailableSpace(obj.item, tilePosition))
                {
                    GameObject instance = Instantiate(obj.item, tilePosition, Quaternion.identity, roomInteriorsHolder);
                    tiles[i, j] = instance;
                    foreach (Transform child in tiles[i, j].transform)
                    {
                        tiles[(int)child.transform.position.x, (int)child.transform.position.y] = child.gameObject;
                    }
                }
            }
        }
        base.DrawRoomInteriors();
    }

    public void ActivateRoom()
    {
        clsDungeonController.currentRoom = id;
        if (!isCompleted)
        {
            ActivateGateways();
            SpawnObject(roomEnemies[0].enemyType);
            SpawnObject(roomEnemies[0].enemyType);
            SpawnObject(roomEnemies[0].enemyType);
        }
    }
}
