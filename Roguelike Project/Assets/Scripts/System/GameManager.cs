using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [System.NonSerialized] public bool generationFailed;
    public DungeonController currentDungeon;
    public PlayerMovement player;
    public GameObject loadingScreen;
    public List<EnemyMovement> enemiesAlive = new List<EnemyMovement>();
    public int enemiesAliveCount;
    private bool _enemiesMoving;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Update()
    {
        if (!_enemiesMoving)
            StartCoroutine(MoveEnemies());
    }

    private IEnumerator MoveEnemies()
    {
        _enemiesMoving = true;
        foreach (EnemyMovement enemy in enemiesAlive)
        {
            if (enemy.canMove)
            {
                enemy.AttemptMove();
                yield return new WaitForSeconds(0.02f);
            }
        }
        _enemiesMoving = false;
    }

    public void EnemyKilled(EnemyMovement enemy)
    {
        enemiesAlive.Remove(enemy);
        enemiesAliveCount--;
        if (enemiesAliveCount == 0)
        {
            currentDungeon.currentRoom.CompleteRoom();
        }
    }

    public void ManageLoadingScreen(bool state)
    {
        loadingScreen.SetActive(state);
    }
}
