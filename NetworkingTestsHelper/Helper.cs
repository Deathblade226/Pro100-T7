﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkingTestsHelper
{
    public class Helper
    {
        public static void Main(string[] args)
        {
            EndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555);
            Socket s = new Socket(SocketType.Stream, ProtocolType.Tcp);
            s.Connect(ep);

            Console.WriteLine("Hit enter to connect");
            Console.ReadLine();

            Tester(s);
        }

        public static async void Tester(Socket s)
        {
            while (true)
            {
                byte[] words = Encoding.ASCII.GetBytes("Hello network...fuck you sockets");
                s.BeginSend(words, 0, 0, SocketFlags.None, null, null);

                Console.ReadLine();
            }
        }
    }
}
