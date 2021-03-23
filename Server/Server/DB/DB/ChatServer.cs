using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace DB
{
    class ChatServer
    {
        List<Socket> clients = new List<Socket>();
        Socket server;

        string ipAddress;
        int port;

        public ChatServer(string ipAddress, int port)
        {
            this.ipAddress = ipAddress;
            this.port = port;
        }

        public void CreateSocket()
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(new IPEndPoint(IPAddress.Parse(ipAddress), port));

            server.Listen(10);
            Console.WriteLine("채팅서버 소켓생성 완료.");
        }

        public void ChatServerWait()
        {
            if (server.Poll(0, SelectMode.SelectRead))
            {
                Socket mySocket = server.Accept();
                byte[] buffer = new byte[1024];
                mySocket.Receive(buffer);

                clients.Add(mySocket);
            }
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Poll(0, SelectMode.SelectRead))
                {
                    try
                    {
                        byte[] buffer = new byte[1024];
                        clients[i].Receive(buffer);
                        string data = System.Text.Encoding.UTF8.GetString(buffer);

                        string[] data1 = data.Split('\0');
                        data1 = data1[0].Split(',');

                        ChatSend(data1[0], data1[1]);
                    }
                    catch
                    {
                        clients.Remove(clients[i]);
                        i--;
                    }
                }
            }
        }

        void ChatSend(string name, string chat)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                byte[] buffer = new byte[1024];

                buffer = System.Text.Encoding.UTF8.GetBytes($"{name},{chat}");

                clients[i].Send(buffer);
            }
        }
    }
}
