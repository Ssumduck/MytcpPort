using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class GameData : MonoBehaviour
{
    enum DataType
    {
        PlayerInit = 0,
        PlayerSave = 1,
    }

    Stat stat;
    static Socket socket;

    public static bool Stage;

    public int idx;
    public int gold;
    public int diamond;
    public string playerName;
    public int level;
    public int currExp;
    public int exp;
    public static int stage;
    public static int maxStage = 5;

    bool update = false;

    public void PlayerInit(string ipAddress, int port)
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(ipAddress, port);
        byte[] buffer = new byte[1024];
        buffer = System.Text.Encoding.UTF8.GetBytes($"{(int)DataType.PlayerInit},{idx.ToString()}");
        socket.Send(buffer);

        update = true;
    }

    public void PlayerSave(string ipAddress, int port)
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(ipAddress, port);
        byte[] buffer = new byte[1024];
        string str = $"{(int)DataType.PlayerSave},{idx},{gold},{diamond},{level},{currExp},{stat.hp},{stat.maxHp},{stat.dmg},{stat.defense},{stat.atkTime},{stat.critical},{stat.criticaldmg},{playerName}";
        buffer = System.Text.Encoding.UTF8.GetBytes(str);
        socket.Send(buffer);

        update = true;
    }

    void RecvData()
    {
        if (socket.Poll(0, SelectMode.SelectRead))
        {
            if (GameObject.FindObjectOfType<Player>() != null)
                stat = GameObject.FindObjectOfType<Player>().stat;
            else
                stat = new Stat();

            byte[] buffer = new byte[1024];
            socket.Receive(buffer);

            string str = System.Text.Encoding.UTF8.GetString(buffer).TrimEnd('\0');
            string[] data = str.Split(',');

            gold = int.Parse(data[0]);
            diamond = int.Parse(data[1]);
            playerName = data[2];
            level = int.Parse(data[3]);
            currExp = int.Parse(data[4]);
            exp = int.Parse(data[5]);
            stage = int.Parse(data[6]);

            stat.hp = int.Parse(data[7]);
            stat.maxHp = int.Parse(data[8]);
            stat.dmg = int.Parse(data[9]);
            stat.defense = int.Parse(data[10]);
            stat.atkTime = float.Parse(data[11]);
            stat.critical = float.Parse(data[12]);
            stat.criticaldmg = float.Parse(data[13]);
            stat.name = data[14];

            update = false;
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            socket = null;
        }
    }

    private void Update()
    {
        if (update)
            RecvData();

        if (Input.GetKeyDown(KeyCode.Return))
            PlayerSave(Define.IPADDRESS, (int)Define.Port.Data);
    }

    bool LevelUp()
    {
        if (exp <= currExp)
        {
            currExp -= exp;
            level += 1;

            if (GameObject.FindObjectOfType<Status>() != null)
                GameObject.FindObjectOfType<Status>().Init();

            return true;
        }
        return false;
    }
}
