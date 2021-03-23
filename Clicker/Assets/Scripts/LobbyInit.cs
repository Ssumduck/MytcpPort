using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyInit : MonoBehaviour
{
    [SerializeField]
    Canvas canvas;
    void Start()
    {
        Invoke("Init", 0.5f);
    }

    void Init()
    {
        GameData data = GameObject.FindObjectOfType<GameData>();
        if (data == null)
        {
            Invoke("Init", 0.2f);
            return;
        }

        if (String.IsNullOrEmpty(data.playerName))
        {
            GameObject nick = Instantiate(Resources.Load("Prefabs/NicknameCreate"), canvas.transform) as GameObject;
            Debug.Log(nick.name);
        }
    }
}
