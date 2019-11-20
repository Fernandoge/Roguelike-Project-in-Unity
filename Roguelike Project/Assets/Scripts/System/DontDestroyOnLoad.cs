using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private static DontDestroyOnLoad multitonInstance;

    void Awake()
    {
        DontDestroyOnLoad(this);
        if (multitonInstance == null)
        {
            multitonInstance = this;      
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

}
