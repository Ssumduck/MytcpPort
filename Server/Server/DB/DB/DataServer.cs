using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace DB
{
    class DataServer
    {
        enum DataType
        {
            PlayerInit = 0,
            PlayerSave = 1,
            PlayerChange = 2,
        }

        Socket server;
        Socket client;
        string ipAddress;
        int port;

        public DataServer(string ip, int port)
        {
            ipAddress = ip;
            this.port = port;
        }

        public void CreateSocket()
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(new IPEndPoint(IPAddress.Parse(ipAddress), port));
            server.Listen(1);
            Console.WriteLine("데이터서버 소켓 생성완료");
        }

        // 플레이어 정보를 관리
        public void DataServerWait()
        {
            if (server.Poll(0, SelectMode.SelectRead))
            {
                try
                {
                    client = server.Accept();

                    byte[] buffer = new byte[1024];
                    client.Receive(buffer);

                    string str = System.Text.Encoding.UTF8.GetString(buffer).TrimEnd('\0');

                    string[] data = str.Split(',');

                    DataType type = (DataType)Enum.ToObject(typeof(DataType), int.Parse(data[0]));

                    switch (type)
                    {
                        case DataType.PlayerInit:
                            PlayerInfo(int.Parse(data[1]));
                            break;
                        case DataType.PlayerSave:
                            List<string> list = new List<string>();
                            for (int i = 1; i < 14; i++)
                            {
                                list.Add(data[i]);
                            }
                            PlayerSave(list);
                            break;
                        case DataType.PlayerChange:
                            break;
                    }
                }
                catch
                {
                    Console.WriteLine("연결이 되었습니다.");
                }
            }
        }

        void PlayerSave(List<string> list)
        {
            int idx = int.Parse(list[0]);
            SQL.MySQLCommand.Update(Database.Player, "playerinfo", idx, Data.gold, list[1]);
            SQL.MySQLCommand.Update(Database.Player, "playerinfo", idx, Data.diamond, list[2]);
            SQL.MySQLCommand.Update(Database.Player, "playerinfo", idx, Data.level, list[3]);
            SQL.MySQLCommand.Update(Database.Player, "playerinfo", idx, Data.exp, list[4]);
            SQL.MySQLCommand.Update(Database.Player, "playerinfo", idx, Data.name, list[12]);

            SQL.MySQLCommand.Update(Database.Player, "playerstat", idx, Data.hp, list[5]);
            SQL.MySQLCommand.Update(Database.Player, "playerstat", idx, Data.maxhp, list[6]);
            SQL.MySQLCommand.Update(Database.Player, "playerstat", idx, Data.dmg, list[7]);
            SQL.MySQLCommand.Update(Database.Player, "playerstat", idx, Data.defense, list[8]);
            SQL.MySQLCommand.Update(Database.Player, "playerstat", idx, Data.atkTime, list[9]);
            SQL.MySQLCommand.Update(Database.Player, "playerstat", idx, Data.critical, list[10]);
            SQL.MySQLCommand.Update(Database.Player, "playerstat", idx, Data.criticaldmg, list[11]);


            PlayerInfo(idx);
        }

        void PlayerInfo(int idx)
        {
            string gold = SQL.MySQLCommand.SELECT(Database.Player, "playerinfo", "idx", idx.ToString(), "gold");
            string diamond = SQL.MySQLCommand.SELECT(Database.Player, "playerinfo", "idx", idx.ToString(), "diamond");
            string playerName = SQL.MySQLCommand.SELECT(Database.Player, "playerinfo", "idx", idx.ToString(), "name");
            string level = SQL.MySQLCommand.SELECT(Database.Player, "playerinfo", "idx", idx.ToString(), "level");
            string currExp = SQL.MySQLCommand.SELECT(Database.Player, "playerinfo", "idx", idx.ToString(), "exp");
            string exp = SQL.MySQLCommand.SELECT(Database.GameData, "exp", "level", level, "exp");
            string stage = SQL.MySQLCommand.SELECT(Database.Player, "playerinfo", "idx", idx.ToString(), "stage");

            string hp = SQL.MySQLCommand.SELECT(Database.Player, "playerstat", "idx", idx.ToString(), "hp");
            string maxhp = SQL.MySQLCommand.SELECT(Database.Player, "playerstat", "idx", idx.ToString(), "maxhp");
            string dmg = SQL.MySQLCommand.SELECT(Database.Player, "playerstat", "idx", idx.ToString(), "dmg");
            string defense = SQL.MySQLCommand.SELECT(Database.Player, "playerstat", "idx", idx.ToString(), "defense");
            string atkTime = SQL.MySQLCommand.SELECT(Database.Player, "playerstat", "idx", idx.ToString(), "atktime");
            string critical = SQL.MySQLCommand.SELECT(Database.Player, "playerstat", "idx", idx.ToString(), "critical");
            string criticaldmg = SQL.MySQLCommand.SELECT(Database.Player, "playerstat", "idx", idx.ToString(), "criticaldmg");
            string name = SQL.MySQLCommand.SELECT(Database.Player, "playerstat", "idx", idx.ToString(), "name");

            byte[] buffer = new byte[1024];
            string str = $"{gold},{diamond},{playerName},{level},{currExp},{exp},{stage},{hp},{maxhp},{dmg},{defense},{atkTime},{critical},{criticaldmg},{name}";
            buffer = System.Text.Encoding.UTF8.GetBytes(str);
            client.Send(buffer);
        }
    }
}