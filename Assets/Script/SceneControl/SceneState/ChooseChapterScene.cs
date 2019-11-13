using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseChapterScene : SceneState
{
    public ChooseChapterScene(SceneControl sceneControl) : base(sceneControl)
    {

    }

    public override void IntoScene()
    {
        UIContainer.Instacne.FindUI<Button>("Back").onClick.AddListener(() => Back());
        UIContainer.Instacne.FindUI<Button>("Chapter1").onClick.AddListener(() => Chapter1());
    }

    public override void OutScene()
    {
        
    }

    public override void UpdateScene()
    {
        
    }

    private void Chapter1()
    {
        sceneControl.SetSceneState(new ChooseLevelScene(sceneControl), "ChooseLevelScene");
    }

    private void Back()
    {
        sceneControl.SetSceneState(new StartScene(sceneControl), "StartScene");
    }
}
