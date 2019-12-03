using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(ReloadCurrentScene);
    }

    public void ReloadCurrentScene()
    {
        GameManager.Instance.ManageLoadingScreen(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}
