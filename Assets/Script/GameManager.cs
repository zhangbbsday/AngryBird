using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Texture2D[] cursor;
    public AudioSystem AudioSystem { get; private set; }
    public MouseSystem MouseSystem { get; private set; }

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
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = GetComponent<GameManager>();
        SystemInitialize();
    }

    private void Update()
    {
        SystemUpdate();
    }

    private void SystemInitialize()
    {
        AudioSystem = new AudioSystem();
        MouseSystem = new MouseSystem(cursor[0], cursor[1], cursor[2]);
    }

    private void SystemUpdate()
    {
        MouseSystem.UpdateCursor();
    }
}
