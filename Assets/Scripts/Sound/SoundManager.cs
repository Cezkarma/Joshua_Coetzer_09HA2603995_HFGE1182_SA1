using System;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public Sound[] soundList;
    public string[] playOnStart;
    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        foreach (Sound s in soundList)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            if (s.mixer != null)
            {
                s.source.outputAudioMixerGroup = s.mixer;
            }
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        foreach (string soundName in playOnStart)
        {
            Play(soundName);
        }
    }

    public void Play(string name)
    {
        
        Sound s = Array.Find(soundList, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound Not Found: " + name);
            return;
        }
        s.source.Play();
    }
}