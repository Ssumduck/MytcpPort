using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
    [SerializeField]
    Text nicknameTxt, levelText;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        Invoke("Init", 0.5f);
        GameData data = GameObject.FindObjectOfType<GameData>();
        if (data != null)
        {
            nicknameTxt.text = $"닉네임 : {data.playerName}";
            levelText.text = $"레벨 : {data.level}";
        }
    }
}
