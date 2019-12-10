using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public DungeonController currentDungeon;
    public PlayerMovement player;
    public GameObject loadingScreen;
    [System.NonSerialized] public bool generationFailed;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void ManageLoadingScreen(bool state)
    {
        loadingScreen.SetActive(state);
    }
}
