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
        GameManager.Instance.enemiesAlive = new List<MovingObject>();
        GameManager.Instance.ManageLoadingScreen(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void KillRoomEnemies()
    {
        foreach (EnemyController enemy in GameManager.Instance.currentDungeon.currentRoom.enemiesAlive.ToList())
        {
            GameManager.Instance.currentDungeon.currentRoom.EnemyKilled(enemy);
        }
    }
}
