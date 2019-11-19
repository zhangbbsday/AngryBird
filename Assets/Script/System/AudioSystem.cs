using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : BaseSystem
{
    public enum MusicName
    {
        Title,
        StartAnimation,
        LevelFinish,
        BirdSong
    }

    public enum SoundsName
    {

    }

    public bool IsOpenMusic { get; set; } = true;  //后面用持久化代替

    private AudioSource musicSource;
    private Dictionary<string, AudioClip> musicClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> soundClips = new Dictionary<string, AudioClip>();

    public AudioSystem(AudioClip[] music, AudioClip[] sounds, AudioSource audioSource)
    {
        AddAudioClip(music, sounds);
        musicSource = audioSource;
        Initialize();
    }

    protected override void Initialize()
    {
        IsRuning = true;
        IsOpenMusic = bool.Parse(PlayerPrefs.GetString("IsOpenMusic", "True"));
    }

    public override void Update()
    {
        if (musicSource == null)
            return;

        if (IsOpenMusic)
        {
            if (!musicSource.isPlaying && musicSource.loop)
                musicSource.Play();
        }
        else
            musicSource.Stop();
    }

    public override void Release()
    {
        IsRuning = false;
    }

    public void Play(MusicName name, bool isLoop = false)
    {
        if (!IsOpenMusic)
            return;

        musicSource.clip = musicClips[name.ToString()];
        musicSource.loop = isLoop;
        musicSource.Play();
    }

    public void Play(AudioSource audioSource, SoundsName name)
    {
        if (!IsOpenMusic)
            return;

        audioSource.PlayOneShot(soundClips[name.ToString()]);
    }

    private void AddAudioClip(AudioClip[] music, AudioClip[] sounds)
    {
        foreach (AudioClip audioClip in music)
        {
            musicClips[audioClip.name] = audioClip;
        }

        foreach (AudioClip audioClip in sounds)
        {
            musicClips[audioClip.name] = audioClip;
        }
    }
}
