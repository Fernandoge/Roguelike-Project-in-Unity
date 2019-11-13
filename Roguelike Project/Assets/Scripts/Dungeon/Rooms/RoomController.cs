using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public int id;
    public Rect roomRectangle;
    public bool isCompleted;
    public GameObject[,] roomInteriorsPosition;
    protected DungeonController clsDungeonController;
    protected GameObject[] roomGateways;
    protected Transform roomGatewaysHolder;
    public Transform roomInteriorsHolder;
    protected DungeonEnemy[] roomEnemies;
    public int enemiesAlive;

    private void Awake()
    {
        clsDungeonController = GameObject.FindGameObjectWithTag("Dungeon").GetComponent<DungeonController>();
        roomEnemies = clsDungeonController.enemies;
        roomGatewaysHolder = transform.GetChild(0);
        roomInteriorsHolder = transform.GetChild(3);
    }

    public virtual void DrawRoomInteriors(){}

    protected bool CheckAvailableSpace(int posX, int posY, int tilesAbove, int tilesBelow, int tilesBeside)
    {
        //Check above and below tiles
        for (int i = posY + tilesAbove; i >= posY - tilesBelow; i--)
        {
            if (i < 0 || i >= roomRectangle.yMax)
                return false;
            //Check beside tiles
            for (int j = posX - tilesBeside; j <= posX + tilesBeside; j++)
            {
                if (j < 0 || j >= roomRectangle.xMax)
                    return false;
                if (roomInteriorsPosition[j, i] != null || clsDungeonController.dungeonWallsPosition[j, i] != null)
                    return false;
            }
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
