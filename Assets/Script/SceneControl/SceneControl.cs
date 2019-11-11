using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl
{
    private SceneState scene;
    private AsyncOperation asyncOperation;
    private bool isBegan;

    public void SetSceneState(SceneState newScene, string sceneName)
    {
        if (scene != null)
            scene.OutScene();

        scene = newScene;
        isBegan = false;
        LoadScene(sceneName);
    }

    public void UpdateScene()
    {
        if (asyncOperation != null && !asyncOperation.isDone || scene == null)
            return;

        if (!isBegan)
        {
            isBegan = true;
            scene.IntoScene();
        }

        scene.UpdateScene();
    }

    private void LoadScene(string name)
    {
        if (name == null || name.Length == 0)
            return;

        asyncOperation = SceneManager.LoadSceneAsync(name);
    }
}
