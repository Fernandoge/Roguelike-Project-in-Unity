using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public PlayerMovement player;
    public bool playersTurn;
    private bool enemiesMoving;

    public List<EnemyMovement> enemies;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Update()
    {
        if (playersTurn || enemiesMoving)
            return;

        StartCoroutine(MoveEnemies());
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(player.moveTime);

        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(player.moveTime * 0.5f);
        }
        else 
        {
            for (int i = 0; i < enemies.Count; i++)
                enemies[i].validMove = enemies[i].CheckEnemyMovement();

            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].validMove)
                    enemies[i].Move();
            }
            yield return new WaitForSeconds(enemies[0].moveTime);
        }
        
        playersTurn = true;
        enemiesMoving = false;
    }

}
