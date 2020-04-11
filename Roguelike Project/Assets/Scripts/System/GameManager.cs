using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [System.NonSerialized] public bool generationFailed;
    public DungeonController currentDungeon;
    public int[,] tilesLayers;
    public PlayerMovement player;
    public Vector3 playerDestinyPosition;
    public GameObject loadingScreen;
    public List<MovingObject> enemiesAlive;

    private void Awake()
    {
	Application.targetFrameRate = 60;
        if (Instance == null)
            Instance = this;
    }

    public void InitializeDungeon(DungeonController dungeon)
    {
        currentDungeon = dungeon;
        int xLength = dungeon.tilesPosition.GetLength(0);
        int yLength = dungeon.tilesPosition.GetLength(1);
        tilesLayers = new int[xLength, yLength];
        for (int i = 0; i < xLength; i++)
        {
            for (int j = 0; j < yLength; j++)
            {
                if (dungeon.tilesPosition[i, j] != null)
                    tilesLayers[i, j] = dungeon.tilesPosition[i, j].layer;
            }
        }
        dungeon.bossRoomInstance.GetBossRoomLayers();
    }

    public void ManageLoadingScreen(bool state)
    {
        loadingScreen.SetActive(state);
    }

    public void ResetPathfinding(MovingObject thisEnemy)
    {
        foreach (EnemyMovement enemy in enemiesAlive)
        {
            if (thisEnemy != enemy)
            {
                enemy.targetPosition = playerDestinyPosition;
                enemy.pathCalculated = false;
            }    
        }
    }

}
