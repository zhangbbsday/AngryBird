using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelScene : SceneState
{
    private int levelIndex;
    private Transform stars;
    private Transform pigs;
    private GameObject off;
    private GameObject pauseMenu;
    private GameObject clearMenu;
    private GameObject failMenu;
    private float waitTime = 2.0f;

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
        RunLevelSystem();
    }

    public override void OutScene()
    {
        if (GameManager.Instance.IsPasue)
            GameManager.Instance.RecoverGame();

        StopLevelSystem();
    }

    public override void UpdateScene()
    {
        
    }
    protected override void AddStringMethod()
    {
        stringMethod = new string[] { "Pause", "ReGame", "ReGamePause", "ChooseLevel", "MainMenu", "Info", 
            "Back", "Audio", "ReGameClear", "ReGameFail", "ChooseLevelClear", "ChooseLevelFail", "NextLevel"};
    }

    protected override void LinkOtherUI()
    {
        GameObjectContainer.Instacne.FindGameObjectComponent<Text>("Level").text = $"1 - {levelIndex}";
        GameObjectContainer.Instacne.FindGameObjectComponent<Text>("TitleClear1").text = $"1 - {levelIndex}";
        GameObjectContainer.Instacne.FindGameObjectComponent<Text>("TitleFail1").text = $"1 - {levelIndex}";
        pigs = GameObjectContainer.Instacne.FindGameObjectComponent<Transform>("Pigs");
        stars = GameObjectContainer.Instacne.FindGameObjectComponent<Transform>("Stars");


        off = GameObjectContainer.Instacne.FindGameObject("Off");
        pauseMenu = GameObjectContainer.Instacne.FindGameObject("PauseMenu");
        clearMenu = GameObjectContainer.Instacne.FindGameObject("LevelClear");
        failMenu = GameObjectContainer.Instacne.FindGameObject("LevelFail");
        
    }

    private void JudgeVictory()
    {
        GameObject obj = new GameObject("SceneMono");
        obj.AddComponent<Noone>().StartCoroutine(JudgePrepare(GameManager.Instance.JudgeSystemControl.JudgeState));
    }

    private IEnumerator JudgePrepare(JudgeSystem.JudgeStateType judgeState)
    {
        yield return new WaitForSeconds(waitTime);
        if (judgeState == JudgeSystem.JudgeStateType.Clear)
        {
            GameManager.Instance.AudioSystemControl.Play(AudioSystem.MusicName.LevelClear);
            GameManager.Instance.CameraSystemControl.SetStartCamera();
            yield return GameManager.Instance.BirdControlSystemControl.AddBirdScore();
        }
        else
        {
            for (int i = 0; i < pigs.childCount; i++)
            {
                pigs.GetChild(i).GetComponent<Pig>()?.Smile();
            }
            yield return new WaitForSeconds(waitTime);
        }
        GameManager.Instance.PauseGame();
        switch (judgeState)
        {
            case JudgeSystem.JudgeStateType.Clear:
                clearMenu.SetActive(true);
                ShowStar();
                GameManager.Instance.AudioSystemControl.Play(AudioSystem.MusicName.LevelFinish);
                break;
            case JudgeSystem.JudgeStateType.Fail:
                failMenu.SetActive(true);
                GameManager.Instance.AudioSystemControl.Play(AudioSystem.MusicName.LevelFail);
                break;
        }

        if (PlayerPrefs.GetInt("TrueLevel", 1) < levelIndex + 1)
            PlayerPrefs.SetInt("TrueLevel", levelIndex + 1);
    }

    #region UI相关
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
    private void ReGameClear()
    {
        ReGame();
    }
    private void ReGameFail()
    {
        ReGame();
    }
    private void ChooseLevel()
    {
        sceneControl.SetSceneState(new ChooseLevelScene(sceneControl), "ChooseLevelScene");
    }
    private void ChooseLevelClear()
    {
        ChooseLevel();
    }

    private void ChooseLevelFail()
    {
        ChooseLevel();
    }

    private void MainMenu()
    {
        sceneControl.SetSceneState(new StartScene(sceneControl), "StartScene");
    }

    private void NextLevel()
    {
        if (levelIndex + 1 > GameManager.LevelNumber)
            return;

        sceneControl.SetSceneState(new LevelScene(sceneControl, levelIndex + 1), "Level" + (levelIndex + 1).ToString());
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

    private void ShowStar()
    {
        for (int i = 0; i < stars.childCount; i++)
        {
            if (((float)GameManager.Instance.ScoreSystemControl.NowScore / 
                GameManager.Instance.ScoreSystemControl.EveryLevelScore[levelIndex - 1]) >= GameManager.Instance.ScoreSystemControl.StarPercent[i])
            {
                stars.GetChild(i).gameObject.SetActive(true);
            }
        }    
    }

    #endregion

    #region 系统相关

    private void RunLevelSystem()
    {
        GameManager.Instance.InputSystemControl.IsRuning = true;
        GameManager.Instance.SlingSystemControl.IsRuning = true;
        GameManager.Instance.CameraSystemControl.IsRuning = true;
        GameManager.Instance.BirdControlSystemControl.IsRuning = true;
        GameManager.Instance.ScoreSystemControl.IsRuning = true;
        GameManager.Instance.JudgeSystemControl.IsRuning = true;

        GameManager.Instance.AudioSystemControl.Play(AudioSystem.MusicName.LevelStart, false);
        GameManager.Instance.SlingSystemControl.GetSling(GameObjectContainer.Instacne.FindGameObject("Sling"));
        GameManager.Instance.BirdControlSystemControl.GetBird(GameObjectContainer.Instacne.FindGameObjectComponent<Transform>("Birds"));
        GameManager.Instance.CameraSystemControl.SetLevelCamera(GameObjectContainer.Instacne.FindGameObjectComponent<Transform>("Edges"));
        GameManager.Instance.ScoreSystemControl.SetScore(levelIndex, GameObjectContainer.Instacne.FindGameObjectComponent<Text>("BestScore")
            , GameObjectContainer.Instacne.FindGameObjectComponent<Text>("NowScore"), GameObjectContainer.Instacne.FindGameObjectComponent<Text>("BestScoreClear")
            , GameObjectContainer.Instacne.FindGameObjectComponent<Text>("NowScoreClear"));
        GameManager.Instance.JudgeSystemControl.SetJudge(pigs, JudgeVictory);
    }

    private void StopLevelSystem()
    {
        GameManager.Instance.InputSystemControl.IsRuning = false;
        GameManager.Instance.CameraSystemControl.IsRuning = false;
        GameManager.Instance.BirdControlSystemControl.IsRuning = false;
        GameManager.Instance.ScoreSystemControl.IsRuning = false;
        GameManager.Instance.JudgeSystemControl.IsRuning = false;

        GameManager.Instance.ScoreSystemControl.SaveScore();
        GameManager.Instance.AudioSystemControl.Play(AudioSystem.MusicName.Title, true);
    }
    #endregion
}
