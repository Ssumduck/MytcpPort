using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class ChatClient : MonoBehaviour
{
    public ChatUI ui;
    public Socket socket;

    string playerName;

    public void Start()
    {
        ui = GameObject.FindObjectOfType<ChatUI>();

        if (socket == null)
            CreateSocket();
    }

    void CreateSocket()
    {

        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(Define.IPADDRESS, (int)Define.Port.Chat);
        byte[] buffer = new byte[1024];

        string data = ((int)Define.Chat.Connect).ToString();

        buffer = System.Text.Encoding.UTF8.GetBytes(data);

        socket.Send(buffer);
    }

    public void SendMsg(string msg)
    {
        if (string.IsNullOrEmpty(msg))
            return;
        if (string.IsNullOrEmpty(playerName))
            playerName = GameObject.FindObjectOfType<GameData>().playerName;

        byte[] buffer = new byte[1024];

        string data = $"{playerName},{msg}";

        buffer = System.Text.Encoding.UTF8.GetBytes(data);

        socket.Send(buffer);
    }

    void Update()
    {
        if(socket.Poll(0, SelectMode.SelectRead))
        {
            byte[] buffer = new byte[1024];
            socket.Receive(buffer);
            string msg = System.Text.Encoding.UTF8.GetString(buffer).TrimEnd('\0');
            msg.Replace(',', ':');
            ui.AddText(msg);
        }
    }
}