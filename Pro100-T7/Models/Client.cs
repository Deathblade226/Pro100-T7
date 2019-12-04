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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace Pro100_T7.Models
{
    public sealed class Client
    {
        public DispatcherTimer UpdateTimer { get; set; } = new DispatcherTimer();
        public Socket HostConnection { get; private set; } = new Socket(SocketType.Stream, ProtocolType.Tcp);

        ~Client() => UpdateTimer.Stop();
        public Client() 
        {
            UpdateTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            UpdateTimer.Tick += TRD;
        }

        private void TRD<TEventArgs>(object sender, TEventArgs e)
        {
            TryReceiveData(null, null);
        }

        public void TryReceiveData(object sender, ElapsedEventArgs e)
        {
            byte[] received = new byte[3200000];
            HostConnection.ReceiveAsync(received, SocketFlags.None);

            if (received.Length > 0) Debug.WriteLine($"Received from {HostConnection.RemoteEndPoint.ToString()}: {received.ToString()}");
            if (Session.IsOnlineSession) Session.Tick(received);
        }

        public async void SendData(byte[] data)
        {
            await HostConnection.SendAsync(data, SocketFlags.None);
        }

        public bool TryConnectToServer(IPEndPoint server)
        {
            try { HostConnection.Connect(server); UpdateTimer.Start(); return true; }
            catch (Exception) { return false; }
        }
    }
}
