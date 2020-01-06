using Common;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Sound
{
    public string name;
    public bool loop;
    public AudioClip clip;
    private AudioSource m_Source;

    [Range(0.0f, 1.0f)] public float volume = 0.5f;
    [Range(0.5f, 1.5f)] public float pitch = 1.0f;

    [Range(0.0f, 0.5f)] public float randomVolume = 0.1f;
    [Range(0.0f, 0.5f)] public float randomPitch = 0.1f;

    public void SetSource(AudioSource source, bool spatial)
    {
        m_Source = source;
        m_Source.spatialBlend = spatial ? 1 : 0;
        m_Source.clip = clip;
        m_Source.loop = loop;
        m_Source.playOnAwake = false;
    }

    public void Play()
    {
        SetSoundValues();
        m_Source.Play();
    }

    private void SetSoundValues()
    {
        m_Source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
        m_Source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
    }
}

public class AudioManager : SingletonDestroyable<AudioManager>
{
    public List<Sound> sounds;

    private void Start()
    {
        SetSounds();
        PlaySound(Sounds.idle);
    }

    private void SetSounds()
    {
        for (var i = 0; i < sounds.Count; i++)
        {
            var go = new GameObject("Sound " + i + ":" + sounds[i].name);
            sounds[i].SetSource(go.AddComponent<AudioSource>(), false);
            go.transform.parent = transform;
        }
    }
    public void PlaySound(string soundName)
    {
        foreach (var s in sounds)
        {
            if (s.name == soundName)
            {
                s.Play();
                return;
            }
        }
    }
}

public static class Sounds
{
    public static string idle = "Idle";
    public static string won = "Won";
    public static string lost = "Lost";
    public static string kick = "Kick";
    public static string bounce = "Bounce";
}