using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginClient : MonoBehaviour
{
    public Socket socket;

    public void Connect(Define.LoginSendType type, string ipAddress, int port, string id, string pw)
    {
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ipAddress, port);

            byte[] buffer = new byte[1024];

            string data = $"{type.ToString()},{id},{pw}";

            buffer = System.Text.Encoding.UTF8.GetBytes(data);

            socket.Send(buffer);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void Update()
    {
        if (socket == null)
            return;
        if (socket.Poll(0, SelectMode.SelectRead))
        {
            byte[] buffer = new byte[1024];
            socket.Receive(buffer);

            string[] data = System.Text.Encoding.UTF8.GetString(buffer).Split('\0');
            data = data[0].Split(',');
            int data1 = int.Parse(data[0]), data2 = int.Parse(data[1]);

            Debug.Log(data[0]);

            if (data1 == 0)
            {
                if (data.Length == 3)
                    Login(data2, int.Parse(data[2]));
                else
                    Login(data2, -1);
            }
            else
            {
                Register(data2);
            }
            socket = null;

            return;
        }
    }

    static void PopupCreate(string msg)
    {
        Popup pop = Popup.Instantiate(Resources.Load("Prefabs/Popup") as GameObject, GameObject.FindObjectOfType<Canvas>().transform).GetComponent<Popup>();
        pop.Init(msg, "확인");
    }

    static void Login(int n, int idx)
    {
        if (n == 1)
        {
            PopupCreate("로그인 실패");
            return;
        }

        PopupCreate("로그인성공");

        if (!SceneManager.GetActiveScene().name.Contains("Lobby"))
        {
            Popup popup = GameObject.FindObjectOfType<Popup>();
            popup.btn.onClick.AddListener(() => SceneManager.LoadSceneAsync("Lobby"));
        }
        GameObject.FindObjectOfType<GameData>().idx = idx;
        GameObject.FindObjectOfType<GameData>().PlayerInit(Define.IPADDRESS, (int)Define.Port.Data);
    }

    public void LogOut(int idx)
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(Define.IPADDRESS, (int)Define.Port.Login);
        byte[] buffer = new byte[1024];
        string str = $"{Define.LoginSendType.Logout},{idx}";

        buffer = System.Text.Encoding.UTF8.GetBytes(str);

        socket.Send(buffer);
    }

    static void Register(int n)
    {
        switch (n)
        {
            case 0:
                PopupCreate("회원가입 성공");
                break;
            case 1:
                PopupCreate("회원가입 실패");
                break;
        }
    }

    private void OnApplicationQuit()
    {
        LogOut(GameObject.FindObjectOfType<GameData>().idx);
    }

}
