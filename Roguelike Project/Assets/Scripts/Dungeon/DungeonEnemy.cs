using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonEnemy
{
    public GameObject enemyType;
    public int quantity;

    public DungeonEnemy(GameObject enemyType, int quantity)
    {
        this.enemyType = enemyType;
        this.quantity = quantity;
    }
}
