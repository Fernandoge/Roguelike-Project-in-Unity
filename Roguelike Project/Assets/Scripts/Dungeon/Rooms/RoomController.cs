using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public int id;
    public Rect roomRectangle;
    private Rect _roomFloorsRectangle;
    public bool isCompleted;
    protected GameObject[,] tiles;
    protected DungeonController clsDungeonController;
    protected List<RandomTools.SizeWeightedObject> auxDungeonRoomInteriors;
    protected GatewayPortal[] roomGateways;
    protected Transform roomGatewaysHolder;
    protected Transform roomInteriorsHolder;
    protected Transform roomWallsHolder;
    protected Transform roomFloorsHolder;
    protected Transform roomTreasureHolder;
    protected DungeonController.DungeonEnemy[] roomEnemies;
    public List<EnemyMovement> enemiesAlive = new List<EnemyMovement>();
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

    public void Initialize(DungeonController dungeonController, GameObject[,] tiles, DungeonController.DungeonEnemy[] enemies, int id, Rect roomRectangle, Rect roomFloorsRectangle)
    {
        clsDungeonController = dungeonController;
        roomEnemies = enemies;
        this.tiles = tiles;
        this.id = id;
        this.roomRectangle = roomRectangle;
        _roomFloorsRectangle = roomFloorsRectangle;
    }

    public virtual void DrawRoomInteriors()
    {
        //Draw floors in tiles without them
        for (int i = (int)roomRectangle.x + 1; i < roomRectangle.xMax - 1; i++)
        {
            for (int j = (int)roomRectangle.y + 1; j < roomRectangle.yMax - 3; j++)
            {
                if (tiles[i, j] == null || tiles[i, j].gameObject.layer == LayerMask.NameToLayer("NoFloorTile"))
                {
                    tiles[i, j] = Instantiate(RandomTools.Instance.PickOne(clsDungeonController.dungeonRoomFloors), new Vector3(i, j, 0f), Quaternion.identity, roomFloorsHolder);
                }
            }
        }
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
    
    public Vector3 DestroyRandomRightWall()
    {
        Vector3 rightWallPosition = new Vector3(roomRectangle.xMax - 1, (int)Random.Range(roomRectangle.yMin + ((roomRectangle.yMax - roomRectangle.yMin) / 2), roomRectangle.yMax - 3));
        tiles[(int)rightWallPosition.x, (int)rightWallPosition.y].layer = 0;
        Destroy(tiles[(int)rightWallPosition.x, (int)rightWallPosition.y]);
        return rightWallPosition;
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
        roomGateways = roomGatewaysHolder.GetComponentsInChildren<GatewayPortal>();

        foreach (GatewayPortal gateway in roomGateways)
        {
            gateway.spriteRender.sprite = gateway.disabledSprite;
            gateway.gameObject.layer = LayerMask.NameToLayer("Obstacle");
        }
    }

    public void SpawnObject(GameObject obj, bool isPlayer = false, bool isEnemy = false)
    {
        bool objectSpawned = false;
        while (!objectSpawned)
        {
            int x = Random.Range((int)roomRectangle.x, (int)roomRectangle.xMax);
            int y = Random.Range((int)roomRectangle.y, (int)roomRectangle.yMax);

            //Spawn the object in a floor and not inside a wall or a trigger
            if (tiles[x, y].layer == LayerMask.NameToLayer("Floor"))
            {
                if (!isPlayer)
                    obj = Instantiate(obj, new Vector3(x, y), Quaternion.identity);
                else
                    obj.transform.position = new Vector3(x, y);

                if (isEnemy)
                {
                    enemiesAlive.Add(obj.GetComponent<EnemyMovement>());
                    enemiesAliveCount++;
                }
                objectSpawned = true;
            }
        }
        /*
        foreach (DungeonEnemy enemy in roomEnemies)
        {  
            for (int i = 0; i < enemy.quantity; i++)
            {
                enemiesAlive++;
                enemy.enemyType.transform.GetChild(0).GetComponent<HitpointsManager>().SetRoomController(this);
                GameObject enemyInstance = Instantiate(enemy.enemyType, transform.position, Quaternion.identity);
            }    
        }
        */
    }

    public void SpawnEnemies()
    {
        foreach (DungeonController.DungeonEnemy enemy in roomEnemies)
        {
            for (int i = 0; i < enemy.quantity; i++)
                SpawnObject(enemy.enemyType, isEnemy: true);
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
            gateway.spriteRender.sprite = gateway.activeSprite;
            gateway.gameObject.layer = 0;
        }
    }

    #endregion Gameplay Methods
}
