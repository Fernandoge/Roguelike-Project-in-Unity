using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public bool playersTurn;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

}
