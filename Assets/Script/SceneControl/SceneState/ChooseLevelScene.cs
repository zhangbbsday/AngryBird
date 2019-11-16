using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLevelScene : SceneState
{
    public ChooseLevelScene(SceneControl sceneControl) : base(sceneControl)
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
        stringMethod = new string[] { "Back", "Level1"};
    }
    private void Back()
    {
        sceneControl.SetSceneState(new ChooseChapterScene(sceneControl), "ChooseChapterScene");
    }

    private void Level1()
    {
        sceneControl.SetSceneState(new LevelScene(sceneControl, 1), "Level1");
    }
}
