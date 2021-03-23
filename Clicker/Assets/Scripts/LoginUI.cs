using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    [SerializeField]
    InputField id, pw;
    [SerializeField]
    Button login, register;
    [SerializeField]
    GameObject[] texts;

    private void Awake()
    {
        LoginClient client = GameObject.FindObjectOfType<LoginClient>();
        login.onClick.AddListener(() => client.Connect(Define.LoginSendType.Login, "49.163.25.60", (int)Define.Port.Login, id.text, pw.text));
        register.onClick.AddListener(() => client.Connect(Define.LoginSendType.Register, "49.163.25.60", (int)Define.Port.Login, id.text, pw.text));
    }

    void ButtonActive(bool value)
    {
        login.interactable = value;
        register.interactable = value;
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].SetActive(!value);
        }
    }

    private void Update()
    {
        if(id.text.Length < 4 || pw.text.Length < 4)
            ButtonActive(false);
        else
            ButtonActive(true);
    }
}