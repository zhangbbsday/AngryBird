using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelScene : SceneState
{
    private int levelIndex;
    private GameObject off;
    private GameObject pauseMenu;
    public LevelScene(SceneControl sceneControl, int level) : base(sceneControl)
    {
        levelIndex = level;
    }

    public override void IntoScene()
    {
        AddStringMethod();
        LinkButton();
        LinkOtherUI();
        StartAudio();
    }

    public override void OutScene()
    {
        if (GameManager.Instance.IsPasue)
            GameManager.Instance.RecoverGame();
    }

    public override void UpdateScene()
    {
        
    }
    protected override void AddStringMethod()
    {
        stringMethod = new string[] { "Pause", "ReGame", "ReGamePause", "ChooseLevel", "MainMenu", "Info", "Back", "Audio"};
    }

    protected override void LinkOtherUI()
    {
        UIContainer.Instacne.FindUI<Text>("Level").text = $"1 - {levelIndex}";
        UIContainer.Instacne.FindUI<Text>("BestScore").text = $"0";
        UIContainer.Instacne.FindUI<Text>("NowScore").text = $"0";

        off = UIContainer.Instacne.FindGameObject("Off");
        pauseMenu = UIContainer.Instacne.FindGameObject("PauseMenu");
    }
    private void Pause()
    {
        GameManager.Instance.PauseGame();
        pauseMenu.SetActive(true);
    }

    private void ReGame()
    {
        sceneControl.SetSceneState(new LevelScene(sceneControl, levelIndex), "Level" + levelIndex.ToString());
    }
    private void ReGamePause()
    {
        ReGame();
    }
    private void ChooseLevel()
    {
        sceneControl.SetSceneState(new ChooseLevelScene(sceneControl), "ChooseLevelScene");
    }
    private void MainMenu()
    {
        sceneControl.SetSceneState(new StartScene(sceneControl), "StartScene");
    }

    //未实现
    private void Info()
    {
        Debug.Log("帮助");
    }

    private void Back()
    {
        GameManager.Instance.RecoverGame();
        pauseMenu.SetActive(false);
    }

    private void Audio()
    {
        if (GameManager.Instance.AudioSystemControl.IsOpenMusic)
        {
            GameManager.Instance.AudioSystemControl.IsOpenMusic = false;
            off.gameObject.SetActive(true);
        }
        else
        {
            GameManager.Instance.AudioSystemControl.IsOpenMusic = true;
            off.gameObject.SetActive(false);
        }
    }

    private void StartAudio()
    {
        if (GameManager.Instance.AudioSystemControl.IsOpenMusic)
            off.gameObject.SetActive(false);
        else
            off.gameObject.SetActive(true);
    }
}
