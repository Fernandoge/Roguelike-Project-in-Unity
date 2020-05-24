using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class DebugController : MonoBehaviour
{
    public static DebugController Instance = null;
    public Stopwatch watch = new Stopwatch();
    public Button reloadButton;
    public Button killEnemies;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        reloadButton.onClick.AddListener(ReloadCurrentScene);
        killEnemies.onClick.AddListener(KillRoomEnemies);
    }

    [MenuItem("Dungeon/Reload Dungeon")]
    public static void ReloadCurrentScene()
    {
        GameManager.Instance.enemiesAlive = new List<MovingObject>();
        GameManager.Instance.ManageLoadingScreen(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    [MenuItem("Dungeon/Kill Room Enemies")]
    public static void KillRoomEnemies()
    {
        if (GameManager.Instance.currentDungeon.currentRoom == null)
            return;
        
        foreach (EnemyController enemy in GameManager.Instance.currentDungeon.currentRoom.enemiesAlive.ToList())
        {
            GameManager.Instance.currentDungeon.currentRoom.EnemyKilled(enemy);
        }
    }
    
    public void StartMeasuringMethod() => watch = Stopwatch.StartNew();

    public void StopMeasuringMethod(string message)
    {
        watch.Stop();
        long elapsedMs = watch.ElapsedMilliseconds;
        Debug.Log(message + " " + elapsedMs);
    }
}
