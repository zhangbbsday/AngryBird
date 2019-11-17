using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    private SceneControl sceneControl;

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        UpdateForLoop();
    }

    private void Initialize()
    {
        sceneControl = new SceneControl();
        sceneControl.SetSceneState(new StartScene(sceneControl), "");
        DontDestroyOnLoad(gameObject);
    }

    private void UpdateForLoop()
    {
        sceneControl.UpdateScene();
    }
}
