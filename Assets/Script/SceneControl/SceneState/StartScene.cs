using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class StartScene : SceneState
{

    private RectTransform[] moveGroud = new RectTransform[2];
    private RectTransform[] moveBackgroud = new RectTransform[2];
    private GameObject optionPress;
    private GameObject morePress;
    private Transform birdsContainer;
    private Rigidbody2D[] birds = new Rigidbody2D[5];

    private float moveSpeed = 150f;
    private float moveBackgroundSpeed = 50f;
    private const float moveGroudOffest = -96f;
    private const float moveBackgroudOffest = -482f;
    private Vector2 moveGroudStart = new Vector2(2191, -159);
    private Vector2 moveGroudNew = new Vector2(2227, -159);
    private Vector2 moveBackgroudStart = new Vector2(2742, 0);
    private Vector2 moveBackgroudNew = new Vector2(2258, 0);
    private bool isClickedOption = false;
    private bool isClickedMore = false;
    private int BirdType { get => Mathf.Min(PlayerPrefs.GetInt("TrueLevel", 1), 5); }
    private float birdExitTime = 5f;

    public StartScene(SceneControl sceneControl): base(sceneControl)
    {

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
        
    }

    public override void UpdateScene()
    {
        MoveGround();
        MoveBackground();
        CreateRandomBird();
    }

    protected override void AddStringMethod()
    {
        stringMethod = new string[] { "StartGame", "Option", "Quit", "More", "Audio", "Info" };
    }
    protected override void LinkOtherUI()
    {
        moveGroud[0] = GameObjectContainer.Instacne.FindGameObjectComponent<RectTransform>("GroundGroup");
        moveGroud[1] = GameObject.Instantiate(moveGroud[0].gameObject, moveGroud[0].transform.parent).GetComponent<RectTransform>();
        moveGroud[1].localPosition = moveGroudStart;

        moveBackgroud[0] = GameObjectContainer.Instacne.FindGameObjectComponent<RectTransform>("MoveGroud");
        moveBackgroud[1] = GameObject.Instantiate(moveBackgroud[0].gameObject, moveBackgroud[0].transform.parent).GetComponent<RectTransform>();
        moveBackgroud[1].localPosition = moveBackgroudStart;

        optionPress = GameObjectContainer.Instacne.FindGameObject("OptionPress");
        morePress = GameObjectContainer.Instacne.FindGameObject("MorePress");
        birdsContainer = GameObjectContainer.Instacne.FindGameObjectComponent<Transform>("Birds");
        birds[0] = GameObjectContainer.Instacne.FindGameObjectComponent<Rigidbody2D>("RedBird");
        birds[1] = GameObjectContainer.Instacne.FindGameObjectComponent<Rigidbody2D>("BlueBird");
        birds[2] = GameObjectContainer.Instacne.FindGameObjectComponent<Rigidbody2D>("YellowBird");
        birds[3] = GameObjectContainer.Instacne.FindGameObjectComponent<Rigidbody2D>("BlackBird");
        birds[4] = GameObjectContainer.Instacne.FindGameObjectComponent<Rigidbody2D>("WhiteBird");
    }

    #region 动画相关
    private void MoveGround()
    {
        if (moveGroud[1].localPosition.x > moveGroudOffest)
        {
            moveGroud[1].localPosition += Vector3.left * moveSpeed * Time.deltaTime;
        }
        else
        {
            GameObject.Destroy(moveGroud[0].gameObject);
            moveGroud[0] = moveGroud[1];
            moveGroud[1] = GameObject.Instantiate(moveGroud[0].gameObject, moveGroud[0].transform.parent).GetComponent<RectTransform>();
            moveGroud[1].localPosition = moveGroudNew;
        }
        moveGroud[0].localPosition += Vector3.left * moveSpeed * Time.deltaTime;
    }
    private void MoveBackground()
    {
        if (moveBackgroud[1].localPosition.x > moveBackgroudOffest)
        {
            moveBackgroud[1].localPosition += Vector3.left * moveBackgroundSpeed * Time.deltaTime;
        }
        else
        {
            GameObject.Destroy(moveBackgroud[0].gameObject);
            moveBackgroud[0] = moveBackgroud[1];
            moveBackgroud[1] = GameObject.Instantiate(moveBackgroud[0].gameObject, moveBackgroud[0].transform.parent).GetComponent<RectTransform>();
            moveBackgroud[1].localPosition = moveBackgroudNew;
        }
        moveBackgroud[0].localPosition += Vector3.left * moveBackgroundSpeed * Time.deltaTime;
    }
    private void CreateRandomBird()
    {
        if (Random.Range(0, 1000) > 8)
            return;

        float x = Random.Range(-12.0f, 8.5f);
        float y;
        if (x <= -8)
            y = Random.Range(-6f, 6f);
        else
            y = Random.Range(-6f, -4f);

        Rigidbody2D bird = GameObject.Instantiate(birds[Random.Range(0, BirdType)], new Vector2(x, y), Quaternion.identity, birdsContainer);
        bird.velocity = new Vector2(Random.Range(1, 10), Random.Range(5, 10));
        GameObject.Destroy(bird.gameObject, birdExitTime);
    }
    #endregion

    #region ButtonMethod
    private void StartGame()
    {
        sceneControl.SetSceneState(new ChooseChapterScene(sceneControl), "ChooseChapterScene");
    }
    private void Quit()
    {
        Application.Quit();
    }
    private void Option()
    {
        if (isClickedOption)
            optionPress.SetActive(false);
        else
            optionPress.SetActive(true);
        isClickedOption = !isClickedOption;
    }
    private void Info()
    {
        Debug.Log("信息!");
    }
    private void More()
    {
        if (isClickedMore)
            morePress.SetActive(false);
        else
            morePress.SetActive(true);
        isClickedMore = !isClickedMore;
    }
    private void Audio()
    {
        if (GameManager.Instance.AudioSystemControl.IsOpenMusic)
        {
            GameManager.Instance.AudioSystemControl.IsOpenMusic = false;
            optionPress.transform.Find("Off").gameObject.SetActive(true);
        }
        else
        {
            GameManager.Instance.AudioSystemControl.IsOpenMusic = true;
            optionPress.transform.Find("Off").gameObject.SetActive(false);
        }
    }
    #endregion

    private void StartAudio()
    {
        if (GameManager.Instance.AudioSystemControl.IsOpenMusic)
            optionPress.transform.Find("Off").gameObject.SetActive(false);
        else
            optionPress.transform.Find("Off").gameObject.SetActive(true);
    }
}
