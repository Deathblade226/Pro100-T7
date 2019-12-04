using Pro100_T7.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Windows.UI.Xaml.Media.Imaging;

namespace Pro100_T7.Models
{
    public sealed class Client
    {
        public Timer UpdateTimer { get; set; } = new Timer(5);
        public Socket HostConnection { get; private set; } = new Socket(SocketType.Stream, ProtocolType.Tcp);

        ~Client() => UpdateTimer.Stop();
        public Client() 
        {
            UpdateTimer.Elapsed += TryReceiveData;
        }

        public async void TryReceiveData(object sender, ElapsedEventArgs e)
        {
            byte[] received = new byte[3200000];
            HostConnection.ReceiveAsync(received, SocketFlags.None);

            if (received.Length > 0) Debug.WriteLine(received.ToString());
        }

        public async void SendData(byte[] data)
        {
            await HostConnection.SendAsync(data, SocketFlags.None);
        }

        public bool TryConnectToServer(EndPoint server)
        {
            try { HostConnection.Connect(server); UpdateTimer.Start(); return true; }
            catch (Exception) { return false; }
        }
    }
}
