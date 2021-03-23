using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatUI : MonoBehaviour
{
    [SerializeField]
    ChatClient chat;
    [SerializeField]
    InputField input;
    [SerializeField]
    Button btn;
    [SerializeField]
    Transform content;

    void Start()
    {
        btn.onClick.AddListener(() => chat.SendMsg(input.text));
    }

    public void AddText(string str)
    {
        input.text = string.Empty;
        Text text = Instantiate(Resources.Load<Text>("Prefabs/ChatText"), content);

        text.text = str;
    }
}
