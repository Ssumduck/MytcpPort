using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NicknameCreate : MonoBehaviour
{
    [SerializeField]
    InputField input;
    [SerializeField]
    Button btn;

    private void Start()
    {
        TimeScale(0);
        GameData gd = GameObject.FindObjectOfType<GameData>();

        btn.onClick.AddListener(() => Init(gd));
    }

    void Init(GameData gd)
    {
        gd.playerName = input.text;
        gd.PlayerSave(Define.IPADDRESS, (int)Define.Port.Data);
        TimeScale(1);
        Debug.Log(gd);
        Debug.Log(gd.playerName);
        Debug.Log(input.text);
    }

    void TimeScale(int value)
    {
        Time.timeScale = value;
        Time.fixedDeltaTime = value;

        if (value == 1)
            Destroy(gameObject);
    }

}
