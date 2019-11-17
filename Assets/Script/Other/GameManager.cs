using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Texture2D[] cursors;
    public AudioClip[] music;
    public AudioClip[] sounds;
    public bool IsPasue { get; private set; }
    public AudioSystem AudioSystemControl { get; private set; }
    public MouseSystem MouseSystemControl { get; private set; }

    private static GameManager instance;
    private AudioSource audioSource;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("GameManager不存在!");
            }

            return instance;
        }
    }

    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();

        SystemInitialize();
        ClearSystemArray();
    }

    private void Update()
    {
        SystemUpdate();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        IsPasue = true;
    }

    public void RecoverGame()
    {
        Time.timeScale = 1;
        IsPasue = false;
    }

    #region SystemControl
    private void SystemInitialize()
    {
        AudioSystemControl = new AudioSystem(music, sounds);
        MouseSystemControl = new MouseSystem(cursors);

        AudioSystemControl.Play(audioSource, AudioSystem.MusicName.Title, true);
    }

    /// <summary>
    /// 释放空间
    /// </summary>
    private void ClearSystemArray()
    {
        cursors = null;
        music = null;
        sounds = null;
    }

    private void SystemUpdate()
    {
        MouseSystemControl.UpdateCursor();
        AudioSystemControl.AudioUpdate(audioSource);
    }
    #endregion

}
