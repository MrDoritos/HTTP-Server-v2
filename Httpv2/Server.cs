using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace Httpv2
{
    public class Server
    {
        public Server(IPEndPoint Bind) { this.Bind = Bind; _listener = null; }
        public Server() { Bind = null; _listener = null; }

        public Func<HttpRequest, TcpClient, int> RequestRecieved;
        public IPEndPoint Bind { get; private set; }
        private TcpListener _listener;

        public Boolean Connected
        {
            get { return (_listener != null && _listener.Server.IsBound); }
        }

        public void Start(IPEndPoint bind)
        {
            Bind = bind;
            _listener = new TcpListener(Bind ?? throw new ArgumentNullException());
            _listener.Start();
            new Thread(Listen).Start();
        }

        public void Start()
        {
            _listener = new TcpListener(Bind ?? throw new ArgumentNullException());
            _listener.Start();
            new Thread(Listen).Start();
        }
        
        private void Listen()
        {
            while (true)
            {
                try
                {
                    var client = _listener.AcceptTcpClient();
                    new Thread(() => HandleClient(client)).Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception ({e.Message})\r\n{e.StackTrace}");
                }
            }
        }

        private void HandleClient(TcpClient client)
        {
            byte[] buffer = RecieveData(client);
            //var request = HttpRequest.Parse(buffer);
            //InvokeAll(RequestRecieved.GetInvocationList(), request, client);
        }
        
        private void InvokeAll(Delegate[] delegates, HttpRequest request, TcpClient client)
        {
            var handleTasks = new Thread[delegates.Length];
            for (int a = 0; a < delegates.Length; a++)
                ((Func<HttpRequest, TcpClient, int>)delegates[a])(request, client);
            if (client.Connected) client.Close();
        }


        private byte[] RecieveData(TcpClient client)
        {
            var networkstream = client.GetStream();
            byte[] buffer = new byte[4096];
            byte[] recieved = new byte[0];
            int i;
            while (networkstream.DataAvailable && (i = networkstream.Read(buffer, 0, buffer.Length)) != 0)
                recieved = Append(recieved, buffer);
            return recieved;
        }

        private static byte[] Append(byte[] arrayA, byte[] arrayB)
        {
            byte[] outputBytes = new byte[arrayA.Length + arrayB.Length];
            Buffer.BlockCopy(arrayA, 0, outputBytes, 0, arrayA.Length);
            Buffer.BlockCopy(arrayB, 0, outputBytes, arrayA.Length, arrayB.Length);
            return outputBytes;
        }
    }
}
