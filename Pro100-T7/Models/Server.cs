using Pro100_T7.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Windows.System.Threading;

namespace Pro100_T7.Models
{
    public sealed class Server : IServer
    {
        public Timer CheckConnectTimer { get; set; } = new Timer(5);

        public Socket CurrentAddress { get; set; } = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        public Stack<Socket> Clients { get; private set; } = new Stack<Socket>();

        public Server()
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 5555);
            CurrentAddress.Bind(ipep);
            CurrentAddress.Listen(100);

            CheckConnectTimer.Elapsed += TryConnectClient;
            CheckConnectTimer.Start();
        }

        ~Server() => CheckConnectTimer.Stop();

        public bool IsHosting()
        {
            return Clients.Count() > 0;
        }

        public void SendData(byte[] data)
        {
            foreach (Socket s in Clients)
            {
                s.Send(data);
            }
        }

        public void TryConnectClient(object sender, ElapsedEventArgs e)
        {
            Socket newClient = CurrentAddress.Accept();
            if (newClient != null)
            {
                Clients.Push(newClient);
            }
        }
    }
}
