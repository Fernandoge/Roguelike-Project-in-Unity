using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesRoom : RoomController
{
    private void Start()
    {
        roomEnemyPack = GetEnemyPack(clsDungeonController.enemyPacks);
    }

    public override void DrawRoomInteriors()
    {
        //First we remove the objects that are not valid for this room
        List<RandomTools.SizeWeightedObject> auxDungeonRoomInteriors = ApplySizeConditionsToObjects(clsDungeonController.dungeonRoomInteriors);
        //Get the tile positions of the room in a List
        List <(int x, int y)> roomTilesPositions = GetTilePositions();
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
        base.DrawRoomInteriors();
    }

    public override void ActivateRoom()
    {
        base.ActivateRoom();
        if (!isCompleted)
        {
            DisableGateways();
            SpawnEnemies();
            if (id == 1)
            {
                GameObject initialCorridor = roomGatewaysHolder.GetChild(0).gameObject;
                Instantiate(clsDungeonController.dungeonWalls.left, initialCorridor.transform.position, Quaternion.identity, roomWallsHolder);
                roomGateways.Remove(initialCorridor.GetComponent<GatewayPortal>());
                Destroy(initialCorridor);
            }
        }
    }
}
