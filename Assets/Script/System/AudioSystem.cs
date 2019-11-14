using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem
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

    private Dictionary<string, AudioClip> musicClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> soundClips = new Dictionary<string, AudioClip>();

    public AudioSystem(AudioClip[] music, AudioClip[] sounds)
    {
        AddAudioClip(music, sounds);
    }

    public void Play(AudioSource audioSource, MusicName name, bool isLoop = false)
    {
        if (!IsOpenMusic)
            return;

        audioSource.clip = musicClips[name.ToString()];
        audioSource.loop = isLoop;
        audioSource.Play();
    }

    public void Play(AudioSource audioSource, SoundsName name)
    {
        if (!IsOpenMusic)
            return;

        audioSource.PlayOneShot(soundClips[name.ToString()]);
    }

    public void AudioUpdate(AudioSource audioSource)
    {
        if (audioSource == null)
            return;

        if (IsOpenMusic)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
            audioSource.Stop();
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
