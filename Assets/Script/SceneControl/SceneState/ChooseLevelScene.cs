using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLevelScene : SceneState
{
    private int trueLevel;
    private const int levelNumber = 5;
    private Sprite levelSprite;
    public ChooseLevelScene(SceneControl sceneControl) : base(sceneControl)
    {

    }

    public override void IntoScene()
    {
        trueLevel = PlayerPrefs.GetInt("TrueLevel", 1);

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

    //为了关卡有解锁效果而重写
    protected override void LinkButton()
    {
        base.LinkButton();

        for (int i = 1; i <= levelNumber; i++)
        {
            Button b = UIContainer.Instacne.FindUI<Button>(i.ToString());
            b.onClick.AddListener(() => Level(b));
            if (i == 1)
                levelSprite = b.GetComponent<Image>().sprite;
            else
            {
                if (i <= trueLevel)
                {
                    b.GetComponent<Image>().sprite = levelSprite;
                    b.transform.Find("Text").gameObject.SetActive(true);
                }
            }
        }
    }

    protected override void AddStringMethod()
    {
        stringMethod = new string[] { "Back" };
    }
    private void Back()
    {
        sceneControl.SetSceneState(new ChooseChapterScene(sceneControl), "ChooseChapterScene");
    }

    private void Level(Button button)
    {
        int level = int.Parse(button.name);
        if (level <= trueLevel)
            sceneControl.SetSceneState(new LevelScene(sceneControl, level), "Level" + button.name);
    }
}
