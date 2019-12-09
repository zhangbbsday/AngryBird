using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChapterAnimation : SceneState
{
    private int chapterIndex;
    private Transform animation;
    private float moveSpeed = 1.5f;
    private float waitTimeStart = 4.0f;
    private float waitTimeEnd = 5.0f;
    private readonly float rightEdge = -1022;
    public ChapterAnimation(SceneControl sceneControl, int chapter) : base(sceneControl)
    {
        chapterIndex = chapter;
    }

    public override void IntoScene()
    {
        LinkOtherUI();
        GameObject obj = new GameObject("SceneMono");
        obj.AddComponent<Noone>().StartCoroutine(Move());

        GameManager.Instance.AudioSystemControl.Play(AudioSystem.MusicName.StartAnimation);
    }

    public override void OutScene()
    {
        
    }

    public override void UpdateScene()
    {
        if (Input.GetMouseButtonDown(0))
            ChangeScene();
    }

    protected override void LinkOtherUI()
    {
        animation = GameObjectContainer.Instacne.FindGameObjectComponent<Transform>("Animation");
    }

    private IEnumerator Move()
    {
        yield return new WaitForSeconds(waitTimeStart);
        while (animation.localPosition.x > rightEdge + moveSpeed * Time.deltaTime)
        {
            animation.position += Vector3.left * moveSpeed * Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(waitTimeEnd);
        ChangeScene();
    }

    private void ChangeScene()
    {
        sceneControl.SetSceneState(new LevelScene(sceneControl, 1), "Level1");
    }
}
