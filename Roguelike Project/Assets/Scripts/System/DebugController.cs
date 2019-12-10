using System.Collections;
using System.Collections.Generic;
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
        GameManager.Instance.currentDungeon.currentRoom.KillAllEnemies();
    }
}
