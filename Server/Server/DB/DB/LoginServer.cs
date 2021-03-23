using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using MySql;
using MySql.Data.MySqlClient;

namespace DB
{
    class LoginServer
    {
        SendType type;

        Socket server;
        Socket client;
        string ipAddress;
        int port;

        public LoginServer(string ip, int port)
        {
            ipAddress = ip;
            this.port = port;
        }

        public void CreateSocket()
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(new IPEndPoint(IPAddress.Parse(ipAddress), port));
            server.Listen(1);
            Console.WriteLine("로그인 소켓 생성완료");
        }

        public void LoginServerWait()
        {
            if (server.Poll(0, SelectMode.SelectRead))
            {
                client = server.Accept();
                byte[] buffer = new byte[1024];
                client.Receive(buffer);
                string[] data = System.Text.Encoding.UTF8.GetString(buffer).TrimEnd('\0').Split(',');

                type = (SendType)Enum.Parse(typeof(SendType), data[0]);

                try
                {
                    switch (type)
                    {
                        case SendType.Login:
                            string id = data[1], pw = data[2];
                            Login(id, pw);
                            break;
                        case SendType.Register:
                            id = data[1];
                            pw = data[2];
                            Register(id, pw);
                            break;
                        case SendType.Logout:
                            LogOut(int.Parse(data[1]));
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        void LogOut(int idx)
        {
            SQL.MySQLCommand.Update(Database.Player, "account", idx, Data.login, false);
        }

        void SendData(SendType _type, NetworkState _state)
        {
            byte[] buffer = new byte[1024];

            int data1 = (int)_type, data2 = (int)_state;

            string msg = $"{data1},{data2}";
            buffer = System.Text.Encoding.UTF8.GetBytes(msg);

            client.Send(buffer);
        }

        void SendData(SendType _type, NetworkState _state, int idx)
        {
            if (SQL.MySQLCommand.SELECT(Database.Player, "account", "idx", idx.ToString(), "login") == "True")
            {
                SendData(SendType.Login, NetworkState.Error);
                return;
            }

            byte[] buffer = new byte[1024];

            int data1 = (int)_type, data2 = (int)_state;

            string msg = $"{data1},{data2},{idx}";
            buffer = System.Text.Encoding.UTF8.GetBytes(msg);

            client.Send(buffer);

        }

        void Login(string id, string pw)
        {
            if (SQL.MySQLCommand.SELECT(Database.Player, "Account", "id", id, "password") == pw)
            {
                if (SQL.MySQLCommand.SELECT(Database.Player, "Account", "id", id, "login") == "False")
                {
                    int idx = int.Parse(SQL.MySQLCommand.SELECT(Database.Player, "Account", "id", id, "idx"));
                    SendData(SendType.Login, NetworkState.Success, idx);
                    SQL.MySQLCommand.Update(Database.Player, "account", idx, Data.login, true);
                }
                else
                    SendData(SendType.Login, NetworkState.Error);
            }
            else
            {
                SendData(SendType.Login, NetworkState.Error);
            }
            DisConnect();

        }

        void Register(string id, string pw)
        {
            if (SQL.MySQLCommand.SELECT(Database.Player, "Account", "id", id))
            {
                // 회원가입 실패
                Console.WriteLine("동일한 아이디가 존재합니다.");
                SendData(SendType.Register, NetworkState.Error);
                return;
            }

            try
            {
                if (SQL.MySQLCommand.Insert(Database.Player, "account", "id,password", $"'{id}','{pw}'"))
                {
                    int idx = int.Parse(SQL.MySQLCommand.SELECT(Database.Player, "Account", "id", id, "idx"));
                    SQL.MySQLCommand.Insert(Database.Player, "PlayerInfo", "idx", $"{idx}");

                    Console.WriteLine("회원가입완료");
                    SendData(SendType.Register, NetworkState.Success);
                    SQL.MySQLCommand.Insert(Database.Player, "playerstat", "idx,hp,maxhp,dmg,defense,atkTime,critical,criticaldmg",
                    $"{idx},30,30,5,0,1,0,10");
                }
                else
                {
                    //회원가입실패
                    Console.WriteLine("회원가입실패");
                    SendData(SendType.Register, NetworkState.Error);
                }
                DisConnect();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        void DisConnect()
        {
            client.Shutdown(SocketShutdown.Both);
            client.Close();
            client = null;
        }

    }
}
