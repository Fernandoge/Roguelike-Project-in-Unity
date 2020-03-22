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

        //Spawn interiors
        //First we remove the objects that are not valid for this room
        List<RandomTools.SizeWeightedObject> auxDungeonRoomInteriors = ApplySizeConditionsToObjects(clsDungeonController.dungeonRoomInteriors);
        //Get the tile positions of the room in a List
        List<(int x, int y)> roomTilesPositions = GetTilePositions();
        //Instantiate room interiors checking if there is enough space to instantiate them
        int tilesListSize = roomTilesPositions.Count;
        for (int i = 0; i < tilesListSize; i++)
        {
            int randomInt = Random.Range(0, roomTilesPositions.Count);
            RandomTools.SizeWeightedObject obj = RandomTools.Instance.PickOneSized(auxDungeonRoomInteriors);
            Vector3 tilePosition = new Vector3(roomTilesPositions[randomInt].x, roomTilesPositions[randomInt].y, 0f);

            if (obj.item != null && CheckAvailableSpace(obj.item, tilePosition))
            {
                GameObject instance = Instantiate(obj.item, tilePosition, Quaternion.identity, roomInteriorsHolder);
                tiles[roomTilesPositions[randomInt].x, roomTilesPositions[randomInt].y] = instance;
                foreach (Transform child in tiles[roomTilesPositions[randomInt].x, roomTilesPositions[randomInt].y].transform)
                {
                    tiles[(int)child.transform.position.x, (int)child.transform.position.y] = child.gameObject;
                }
            }
            roomTilesPositions.RemoveAt(randomInt);
        }

        //Fill room tiles with floors
        base.DrawRoomInteriors();
    }

    public override void ActivateRoom()
    {
        base.ActivateRoom();
        if (!isCompleted)
        {
            SpawnEnemies();
            DisableGateways();
            if (isFirstRoom)
            {
                GameObject initialCorridor = roomGatewaysHolder.GetChild(0).gameObject;
                Instantiate(clsDungeonController.dungeonWalls.bottom, initialCorridor.transform.position, Quaternion.identity, roomWallsHolder);
                roomGateways.Remove(initialCorridor.GetComponent<GatewayPortal>());
                Destroy(initialCorridor);
            }
        }
    }

}
