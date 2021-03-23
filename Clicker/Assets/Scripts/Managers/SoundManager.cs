using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    public enum AudioType
    {
        SFX,
        BGM,
    }

    AudioSource sfxSource;
    AudioSource bgmSource;
    Dictionary<string, AudioClip> sfxClip = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> bgmClip = new Dictionary<string, AudioClip>();

    public void Init()
    {
        if (sfxSource == null)
        {
            GameObject go = new GameObject("@Sound");
            sfxSource = go.AddComponent<AudioSource>();
            bgmSource = go.AddComponent<AudioSource>();

            Managers.DontDestroyOnLoad(go);

            DictionaryInit(AudioType.BGM);
            DictionaryInit(AudioType.SFX);
        }
    }

    void DictionaryInit(AudioType type)
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>($"Sound/{type.ToString()}");
        Dictionary<string, AudioClip> clip = new Dictionary<string, AudioClip>();
        switch (type)
        {
            case AudioType.SFX:
                clip = sfxClip;
                break;
            case AudioType.BGM:
                clip = bgmClip;
                break;
        }

        for (int i = 0; i < clips.Length; i++)
        {
            clip.Add($"{type.ToString()}/{clips[i].name}", clips[i]);
        }
    }

    public void SoundPlayer(AudioType type, string fileName)
    {
        AudioSource source = null;
        Dictionary<string, AudioClip> clip = new Dictionary<string, AudioClip>();

        switch (type)
        {
            case AudioType.SFX:
                source = sfxSource;
                clip = sfxClip;
                break;
            case AudioType.BGM:
                source = bgmSource;
                clip = bgmClip;
                break;
        }

        source.clip = clip[$"{type.ToString()}/{fileName}"];
        source.Play();
    }
}
