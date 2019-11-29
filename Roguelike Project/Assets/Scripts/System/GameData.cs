using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance = null;

    public PlayerMovement player;
    public bool playersTurn;
    private bool enemiesMoving;

    public List<EnemyMovement> enemies;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

}
