using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : RoomController
{
    [SerializeField] Transform _roomCorridorsHolder;
    public GameObject firstPortalStop;
    public GameObject secondPortalStop;
    public GameObject roomClosedWall;
    public GameObject firstTile;
    public GameObject lastTile;
    private int[,] bossRoomLayers;

    public void Initialize(DungeonController dungeonController, GameObject[,] tiles, DungeonController.DungeonEnemy[] enemies, int id)
    {
        clsDungeonController = dungeonController;
        clsDungeonController.bossRoomInstance = this;
        roomEnemies = enemies;
        this.tiles = tiles;
        this.id = id;
    }

    public override void ActivateRoom()
    {
        base.ActivateRoom();
        roomClosedWall.SetActive(true);
        Destroy(clsDungeonController.dungeonRoomsParent.gameObject);
        Destroy(clsDungeonController.dungeonCorridorsParent.gameObject);
        Destroy(_roomCorridorsHolder.gameObject);
        transform.position = new Vector3(0f, 0f, 0f);
        transform.position -= firstTile.transform.position;
        GameManager.Instance.player.transform.position = secondPortalStop.transform.position;
        GameManager.Instance.tilesLayers = bossRoomLayers;
    }

    public void GetBossRoomLayers()
    {   
        bossRoomLayers = new int[(int)(lastTile.transform.position.x - firstTile.transform.position.x + 1), (int)(lastTile.transform.position.y - firstTile.transform.position.y + 1)];
        for (int i = 0; i < transform.childCount - 1; i++)  
        {
            Transform currentChild = transform.GetChild(i);
            foreach (Transform child in currentChild)
            {
                bossRoomLayers[(int)(child.position.x - firstTile.transform.position.x), (int)(child.position.y - firstTile.transform.position.y)] = child.gameObject.layer;
                //We add this extra loop in case a tile have objects inside it, like upper walls for example
                for (int j = 0; j < child.childCount; j++)
                {
                    Transform currentChildOfChild = child.GetChild(j);
                    bossRoomLayers[(int)(currentChildOfChild.position.x - firstTile.transform.position.x), (int)(currentChildOfChild.position.y - firstTile.transform.position.y)] = currentChildOfChild.gameObject.layer;
                }
            }
        }
    }
}
