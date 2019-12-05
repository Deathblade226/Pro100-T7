using Pro100_T7.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Pro100_T7.Models
{
    public sealed class Client : IClient
    {
        public Socket HostConnection { get; private set; } = new Socket(SocketType.Stream, ProtocolType.Tcp);

        public Client() {}

        public void ReceiveData(out byte[] data)
        {
            //byte[] old = BitmapData.PixelBuffer.ToArray();
            //byte[] newBytes = new byte[old.Length];
            //old.CopyTo(newBytes, 0);
            //History.EndAction(new Action(newBytes));
            //BitmapData.PixelBuffer.AsStream().Write(old, 0, old.Length);
            //BitmapData.Invalidate();

            byte[] incoming = null;
            HostConnection.Receive(incoming);

            data = incoming;
        }

        public void SendData(byte[] data)
        {
            HostConnection.Send(data);
        }

        public bool TryConnectToServer(EndPoint server)
        {
            try { HostConnection.Connect(server); return true; }
            catch (Exception) { return false; }
        }
    }
}
