using Pro100_T7.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public Timer CheckReceiveTimer { get; set; } = new Timer(5);

        public Socket CurrentAddress { get; set; } = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        public List<Socket> Clients { get; private set; } = new List<Socket>();

        public Server()
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 5555);
            CurrentAddress.Bind(ipep);
            CurrentAddress.Listen(100);

            CheckReceiveTimer.Elapsed += TryReceiveData;
            CheckConnectTimer.Elapsed += TryConnectClient;
            CheckConnectTimer.Start();
            CheckReceiveTimer.Start();
        }

        ~Server() => CheckConnectTimer.Stop();

        public void SendData(byte[] data)
        {
            foreach (Socket s in Clients)
            {
                s.Send(data);
            }
        }

        public void TryReceiveData(object sender, ElapsedEventArgs e)
        {
            byte[] currentClientBuffer = new byte[320000];
            foreach (Socket s in Clients)
            {
                s.ReceiveAsync(currentClientBuffer, SocketFlags.None);

                if (currentClientBuffer.Length > 0) Debug.WriteLine($"{s.RemoteEndPoint.ToString()} - {currentClientBuffer.ToString()}");                
            }
            SendData(currentClientBuffer);
        }

        public void TryConnectClient(object sender, ElapsedEventArgs e)
        {
            Socket newClient = CurrentAddress.Accept();
            if (newClient != null)
            {
                Debug.WriteLine($"Client connected: {newClient.RemoteEndPoint.ToString()}");
                Clients.Add(newClient);
            }
        }
    }
}
