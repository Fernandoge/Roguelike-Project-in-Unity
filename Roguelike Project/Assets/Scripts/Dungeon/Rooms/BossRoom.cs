using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : RoomController
{
    public Transform bossInitialPosition;
    public GameObject firstPortalStop;
    public GameObject roomClosedWall;
    public GameObject firstTile;
    public GameObject lastTile;

    [SerializeField] private Transform _roomCorridorsHolder = default;
    
    private int[,] bossRoomLayers;
    
    public void Initialize(DungeonController dungeonController, GameObject[,] tiles, int id)
    {
        clsDungeonController = dungeonController;
        clsDungeonController.bossRoomInstance = this;
        this.tiles = tiles;
        this.id = id;
        isBossRoom = true;
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
        Instantiate(clsDungeonController.boss, bossInitialPosition.position, Quaternion.identity);
        GameManager.Instance.player.transform.position = firstPortalStop.transform.position;
        GameManager.Instance.tilesLayers = bossRoomLayers;
    }

    public void GetBossRoomLayers()
    {   
        bossRoomLayers = new int[(int)(lastTile.transform.position.x - firstTile.transform.position.x + 1), (int)(lastTile.transform.position.y - firstTile.transform.position.y + 1)];
        for (int i = 0; i < transform.childCount - 2; i++)  
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
