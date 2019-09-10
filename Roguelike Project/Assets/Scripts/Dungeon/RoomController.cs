using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public int id;
    public Rect roomRectangle;
    public bool isCompleted;
    private DungeonController clsDungeonController;
    private GameObject[] roomGateways;
    private Transform roomGatewaysHolder;
    private GameObject[] roomEnemies;
    public int enemiesAlive;

    private void Start()
    {
        clsDungeonController = GameObject.FindGameObjectWithTag("Dungeon").GetComponent<DungeonController>();
        roomEnemies = clsDungeonController.enemies;
        roomGatewaysHolder = transform.GetChild(0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCompleted && collision.tag == "Player")
        {
            ActivateGateways();
            Invoke("SpawnEnemies", 2f);
        }
    }

    private void ActivateGateways()
    {
        roomGateways = new GameObject[roomGatewaysHolder.childCount];

        for (int i = 0; i < roomGatewaysHolder.childCount; i++)
        {
            roomGateways[i] = roomGatewaysHolder.GetChild(i).gameObject;
        }

        foreach (GameObject gateway in roomGateways)
        {
            gateway.SetActive(true);
        }
    }

    private void SpawnEnemies()
    {
        foreach (GameObject enemy in roomEnemies)
        {
            enemiesAlive++;
            enemy.transform.GetChild(0).GetComponent<HitpointsManager>().SetRoomController(this);
            GameObject enemyInstance = Instantiate(enemy, transform.position, Quaternion.identity);
        }
    }

    public void EnemyKilled()
    {
        enemiesAlive--;
        if (enemiesAlive == 0)
        {
            isCompleted = true;
            RemoveGateways();
        }
    }

    private void RemoveGateways()
    {
        foreach (GameObject gateway in roomGateways)
        {
            Destroy(gateway);
        }
    }


}
