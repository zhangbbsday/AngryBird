using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioSystem AudioSystem { get; private set; }
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("GameManager不存在!");
            }

            return instance;
        }
    }

    private void Start()
    {
        instance = GetComponent<GameManager>();
        SystemInitialize();
    }

    private void SystemInitialize()
    {
        AudioSystem = new AudioSystem();
    }
}
