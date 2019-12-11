using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugController : MonoBehaviour
{
    public static DebugController Instance = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void ReloadCurrentScene()
    {
        GameManager.Instance.ManageLoadingScreen(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void KillRoomEnemies()
    {
        foreach (EnemyMovement enemy in GameManager.Instance.enemiesAlive.ToList())
        {
            EnemySpriteManager enemySpriteManager = enemy.gameObject.GetComponent<EnemySpriteManager>();
            enemySpriteManager.Death();
            GameManager.Instance.EnemyKilled(enemy);
        }
        GameManager.Instance.enemiesMoving = false;
    }
}
