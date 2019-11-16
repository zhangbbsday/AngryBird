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
        AddStringMethod();
        LinkButton();
        LinkOtherUI();
    }

    public override void OutScene()
    {
        
    }

    public override void UpdateScene()
    {
        
    }

    protected override void AddStringMethod()
    {
       stringMethod = new string[] { "Back", "Chapter1" };
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
