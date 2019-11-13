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
        UIContainer.Instacne.FindUI<Button>("Back").onClick.AddListener(() => Back());
    }

    public override void OutScene()
    {

    }

    public override void UpdateScene()
    {

    }

    private void Back()
    {
        sceneControl.SetSceneState(new ChooseChapterScene(sceneControl), "ChooseChapterScene");
    }
}
