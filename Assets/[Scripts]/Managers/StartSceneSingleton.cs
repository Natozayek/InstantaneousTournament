using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneSingleton : MonoBehaviour
{
    public static StartSceneSingleton Instance { get; private set; }

    public int time;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
