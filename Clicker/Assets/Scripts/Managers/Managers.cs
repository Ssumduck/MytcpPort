using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers instance = new Managers();
    public static Managers Instance { get { instance.Init(); return instance; } }

    static SoundManager sound = new SoundManager();
    public static SoundManager Sound { get { instance.Init(); return sound; } }

    void Init()
    {
        if (instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject("@Managers");
                go.AddComponent<Managers>();
            }
            instance = go.GetComponent<Managers>();
            sound.Init();
        }
    }
}