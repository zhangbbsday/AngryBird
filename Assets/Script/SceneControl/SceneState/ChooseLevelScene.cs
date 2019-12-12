using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLevelScene : SceneState
{
    private int chapter;
    private int trueLevel;
    private Sprite levelSprite;
    public ChooseLevelScene(SceneControl sceneControl, int chapterIndex = 1) : base(sceneControl)
    {
        chapter = chapterIndex;
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
        Transform buttons = GameObjectContainer.Instacne.FindGameObjectComponent<Transform>("Levels");

        for (int i = 0; i < buttons.childCount; i++)
        {
            Button b = buttons.GetChild(i).GetComponent<Button>();
            b.onClick.AddListener(() => Level(b));
            if (i == 0)
                levelSprite = b.GetComponent<Image>().sprite;
            else
            {
                if (int.Parse(b.name) <= trueLevel)
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

        if (level == 1)
            sceneControl.SetSceneState(new ChapterAnimation(sceneControl, chapter), "ChapterAnimation" + chapter.ToString());
        else if (level <= trueLevel)
            sceneControl.SetSceneState(new LevelScene(sceneControl, level), "Level" + button.name);
    }
}
