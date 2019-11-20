using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public int id;
    public Rect roomRectangle;
    public Rect roomFloorsRectangle;
    public bool isCompleted;
    protected GameObject[,] tiles;
    protected DungeonController clsDungeonController;
    protected List<RandomTools.SizeWeightedObject> auxDungeonRoomInteriors;
    protected GameObject[] roomGateways;
    protected Transform roomInteriorsHolder;
    protected DungeonController.DungeonEnemy[] roomEnemies;
    public int enemiesAlive;

    private void Awake()
    {
        roomInteriorsHolder = transform.GetChild(3);
    }

    public void Initialize(DungeonController dungeonController, GameObject[,] tiles, DungeonController.DungeonEnemy[] enemies, int id, Rect roomRectangle, Rect roomFloorsRectangle)
    {
        clsDungeonController = dungeonController;
        roomEnemies = enemies;
        this.tiles = tiles;
        this.id = id;
        this.roomRectangle = roomRectangle;
        this.roomFloorsRectangle = roomFloorsRectangle;
    }

    public virtual void DrawRoomInteriors(){}

    protected List<RandomTools.SizeWeightedObject> ApplySizeConditionsToObjects(List<RandomTools.SizeWeightedObject> interiors)
    {
        List<RandomTools.SizeWeightedObject> newList = new List<RandomTools.SizeWeightedObject>(interiors);
        foreach (RandomTools.SizeWeightedObject obj in interiors)
        {
            if (roomFloorsRectangle.width < obj.minRoomFloorsWidth || roomFloorsRectangle.height < obj.minRoomFloorsHeight ||
                roomFloorsRectangle.width > obj.maxRoomFloorsWidth || roomFloorsRectangle.height > obj.maxRoomFloorsHeight)
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
        if (x <= roomFloorsRectangle.xMin || y <= roomFloorsRectangle.yMin || x > roomFloorsRectangle.xMax || y > roomFloorsRectangle.yMax || 
            (tiles[x, y] != null && tiles[x, y].tag != "Floor"))
        {
            return false;
        }
        return true;
    }

    protected void ActivateGateways()
    {
        Debug.Log("Activate gateways is disabled");
        /*
        roomGateways = new GameObject[roomGatewaysHolder.childCount];

        for (int i = 0; i < roomGatewaysHolder.childCount; i++)
        {
            roomGateways[i] = roomGatewaysHolder.GetChild(i).gameObject;
        }

        foreach (GameObject gateway in roomGateways)
        {
            gateway.SetActive(true);
        }
        */
    }

    protected void SpawnEnemies()
    {
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

    public void EnemyKilled()
    {
        enemiesAlive--;
        if (enemiesAlive == 0)
        {
            isCompleted = true;
            CompleteRoom();
        }
    }

    protected void CompleteRoom()
    {
        clsDungeonController.roomsCompleted++;
        foreach (GameObject gateway in roomGateways)
        {
            Destroy(gateway);
        }
    }


}
