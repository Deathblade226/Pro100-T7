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
            Console.WriteLine("Hit enter to connect");
            Console.ReadLine();

            EndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555);
            Socket s = new Socket(SocketType.Stream, ProtocolType.Tcp);
            s.Connect(ep);

            byte[] words = Encoding.ASCII.GetBytes("Hello network...fuck you sockets");
            s.Send(words);

            Console.Read();
        }
    }
}
