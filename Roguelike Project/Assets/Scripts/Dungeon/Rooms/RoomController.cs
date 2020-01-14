﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public int id;
    public Rect roomRectangle;
    private Rect _roomFloorsRectangle;
    public bool isCompleted;
    public bool isFirstRoom;
    protected GameObject[,] tiles;
    protected GameObject[] roomEnemyPack;
    protected DungeonController clsDungeonController;
    protected Transform roomGatewaysHolder;
    protected Transform roomInteriorsHolder;
    protected Transform roomWallsHolder;
    protected Transform roomFloorsHolder;
    protected Transform roomTreasureHolder;
    [System.NonSerialized] public List<GatewayPortal> roomGateways = new List<GatewayPortal>();
    [System.NonSerialized] public List<EnemyMovement> enemiesAlive = new List<EnemyMovement>();
    public int enemiesAliveCount;

    #region Dungeon Generation Methods
    private void Awake()
    {
        roomGatewaysHolder = transform.GetChild(0);
        roomFloorsHolder = transform.GetChild(1);
        roomWallsHolder = transform.GetChild(2);
        roomInteriorsHolder = transform.GetChild(3);
        roomTreasureHolder = transform.GetChild(4);
    }

    public void Initialize(DungeonController dungeonController, GameObject[,] tiles, int id, Rect roomRectangle, Rect roomFloorsRectangle)
    {
        clsDungeonController = dungeonController;
        this.tiles = tiles;
        this.id = id;
        this.roomRectangle = roomRectangle;
        _roomFloorsRectangle = roomFloorsRectangle;
    }

    public virtual void DrawRoomInteriors()
    {
        //Draw floors in tiles without them
        for (int i = (int)_roomFloorsRectangle.x + 1; i <= _roomFloorsRectangle.xMax; i++)
        {
            for (int j = (int)_roomFloorsRectangle.y + 1; j <= _roomFloorsRectangle.yMax; j++)
            {
                if (tiles[i, j] == null)
                {
                    tiles[i, j] = Instantiate(RandomTools.Instance.PickOne(clsDungeonController.dungeonRoomFloors), new Vector3(i, j, 0f), Quaternion.identity, roomFloorsHolder);
                }
                else if (tiles[i, j].gameObject.layer == LayerMask.NameToLayer("NoFloorTile"))
                {
                    Instantiate(RandomTools.Instance.PickOne(clsDungeonController.dungeonRoomFloors), new Vector3(i, j, 0f), Quaternion.identity, roomFloorsHolder);
                }
            }
        }
    }

    protected GameObject[] GetEnemyPack(List<DungeonController.DungeonEnemyPack> enemyPacks)
    {
        List<DungeonController.DungeonEnemyPack> newList = new List<DungeonController.DungeonEnemyPack>(enemyPacks);
        foreach (DungeonController.DungeonEnemyPack obj in enemyPacks)
        {
            if (_roomFloorsRectangle.width < obj.minRoomFloorsWidth || _roomFloorsRectangle.height < obj.minRoomFloorsHeight ||
                _roomFloorsRectangle.width > obj.maxRoomFloorsWidth || _roomFloorsRectangle.height > obj.maxRoomFloorsHeight)
            {
                newList.Remove(obj);
            }
        }

        int index = enemyPacks.IndexOf(newList[Random.Range(0, newList.Count)]);
        DungeonController.DungeonEnemyPack pack = enemyPacks[index];
        enemyPacks[index] = new DungeonController.DungeonEnemyPack(
            pack.enemies, pack.stock - 1, pack.minRoomFloorsWidth, pack.minRoomFloorsHeight, pack.maxRoomFloorsWidth, pack.maxRoomFloorsHeight);

        if (enemyPacks[index].stock == 0)
            enemyPacks.Remove(enemyPacks[index]);

        return pack.enemies;
    }

    protected List<RandomTools.SizeWeightedObject> ApplySizeConditionsToObjects(List<RandomTools.SizeWeightedObject> interiors)
    {
        List<RandomTools.SizeWeightedObject> newList = new List<RandomTools.SizeWeightedObject>(interiors);
        foreach (RandomTools.SizeWeightedObject obj in interiors)
        {
            if (_roomFloorsRectangle.width < obj.minRoomFloorsWidth || _roomFloorsRectangle.height < obj.minRoomFloorsHeight ||
                _roomFloorsRectangle.width > obj.maxRoomFloorsWidth || _roomFloorsRectangle.height > obj.maxRoomFloorsHeight)
            {
                newList.Remove(obj);
            } 
        }
        return RandomTools.Instance.CreateSizeWeightedObjectsList(newList);
    }

    protected bool CheckAvailableSpace(GameObject obj, Vector3 instancePosition)
    {
        Vector3 originalObjectPosition = obj.transform.position;
        if (!CheckInteriorTile((int)instancePosition.x, (int)instancePosition.y))
        {
            return false;
        }
        obj.transform.position = instancePosition;
        foreach (Transform child in obj.transform)
        {
            if (!CheckInteriorTile((int)child.transform.position.x, (int)child.transform.position.y))
            {
                obj.transform.position = originalObjectPosition;
                return false;
            }  
        }
        obj.transform.position = originalObjectPosition;
        return true;
    }

    private bool CheckInteriorTile(int x, int y)
    {
        if (x <= _roomFloorsRectangle.xMin || y <= _roomFloorsRectangle.yMin || x > _roomFloorsRectangle.xMax || y > _roomFloorsRectangle.yMax || tiles[x, y] != null)
        {
            return false;
        }
        return true;
    }

    public void GetGateways()
    {
        roomGateways.AddRange(roomGatewaysHolder.GetComponentsInChildren<GatewayPortal>());
    }

    public List<(int, int)> GetTilePositions()
    {
        List<(int, int)> tupleList = new List<(int, int)>();
        for (int i = (int)_roomFloorsRectangle.x + 1; i <= _roomFloorsRectangle.xMax; i++)
        {
            for (int j = (int)_roomFloorsRectangle.y + 1; j <= _roomFloorsRectangle.yMax; j++)
            {
                tupleList.Add((i, j));
            }
        }
        return tupleList;
    }

    public Vector3 DestroyRandomSideWall(int side)
    {
        Vector3 destroyedWallPosition = new Vector3();
        switch (side)
        {
            case 1:
                destroyedWallPosition = new Vector3((int)Random.Range(roomRectangle.xMin + 1, roomRectangle.xMax - 2), roomRectangle.yMax - 1);
                break;
            case 3:
                destroyedWallPosition = new Vector3((int)Random.Range(roomRectangle.xMin + 1, roomRectangle.xMax - 2), roomRectangle.yMin);
                break;
        }
        tiles[(int)destroyedWallPosition.x, (int)destroyedWallPosition.y].layer = 0;
        Destroy(tiles[(int)destroyedWallPosition.x, (int)destroyedWallPosition.y]);
        return destroyedWallPosition;
    }

    #endregion Dungeon Generation Methods

    #region Gameplay Methods

    public virtual void ActivateRoom()
    {
        clsDungeonController.previousRoom = clsDungeonController.currentRoom;
        clsDungeonController.currentRoom = this;
    }

    protected void DisableGateways()
    {
        foreach (GatewayPortal gateway in roomGateways)
        {
            GameManager.Instance.tilesLayers[(int)gateway.transform.position.x, (int)gateway.transform.position.y] = LayerMask.NameToLayer("Obstacle");
            if (gateway.animator != null)
                gateway.animator.enabled = false;
            if (gateway.spriteRender != null)
                gateway.spriteRender.sprite = gateway.disabledSprite;
        }
    }

    public void SpawnObject(GameObject obj, bool isEnemy = false)
    {
        bool objectSpawned = false;
        while (!objectSpawned)
        {
            int x = Random.Range((int)roomRectangle.x, (int)roomRectangle.xMax);
            int y = Random.Range((int)roomRectangle.y, (int)roomRectangle.yMax);

            //Spawn the object in a floor and not inside a wall or a trigger
            if (GameManager.Instance.tilesLayers[x,y] == LayerMask.NameToLayer("Floor") || GameManager.Instance.tilesLayers[x, y] == LayerMask.NameToLayer("NoFloorTile"))
            {
                obj = Instantiate(obj, new Vector3(x, y), Quaternion.identity);
                GameManager.Instance.tilesLayers[x, y] = obj.layer;

                if (isEnemy)
                {
                    EnemyMovement enemyMovement = obj.GetComponent<EnemyMovement>();
                    enemyMovement.currentPositionOriginalLayer = tiles[x, y].layer;
                    enemiesAlive.Add(enemyMovement);
                    enemiesAliveCount++;
                }

                objectSpawned = true;
            }
        }
    }

    public void SpawnEnemies()
    {
        foreach (GameObject enemy in roomEnemyPack)
        {
            SpawnObject(enemy, isEnemy: true);
        }
    }

    public void EnemyKilled(EnemyMovement enemy)
    {
        EnemySpriteManager enemySpriteManager = enemy.gameObject.GetComponent<EnemySpriteManager>();
        enemySpriteManager.Death();
        enemiesAlive.Remove(enemy);
        enemiesAliveCount--;
        if (enemiesAliveCount == 0)
        {
            CompleteRoom();
        }
    }

    public virtual void CompleteRoom()
    {
        isCompleted = true;
        clsDungeonController.roomsCompleted++;
        foreach (GatewayPortal gateway in roomGateways)
        {
            GameManager.Instance.tilesLayers[(int)gateway.transform.position.x, (int)gateway.transform.position.y] = 0;
            if (gateway.animator != null)
                gateway.animator.enabled = true;
        }
    }

    #endregion Gameplay Methods
}
