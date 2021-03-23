using System;
using MySql.Data;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Threading;
using System.Collections.Generic;
using DB;
using System.Threading.Tasks;

namespace DB
{
    class Program
    {
        static void Main(string[] args)
        {
            Start();

            Console.ReadLine();
        }

        static void Start()
        {
            LoginServer login = new LoginServer("49.163.25.60", 1666);
            DataServer data = new DataServer("49.163.25.60", 1667);
            ChatServer chat = new ChatServer("49.163.25.60", 1668);

            chat.CreateSocket();
            data.CreateSocket();
            login.CreateSocket();

            while (true)
            {
                login.LoginServerWait();
                data.DataServerWait();
                chat.ChatServerWait();
            }
        }

        public static Socket Accept(Socket server)
        {
            Socket socket = server.Accept();

            return socket;
        }
    }

}

