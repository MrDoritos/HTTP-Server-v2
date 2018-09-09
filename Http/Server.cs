using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Http.HttpMessage;

namespace Http
{
    public class Server
    {
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

        public Server(IPEndPoint Bind) { this.Bind = Bind; _listener = null; }
        public Server() { Bind = null; _listener = null; }

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
            var request = HttpRequest.Parse(buffer);
            InvokeAll(RequestRecieved.GetInvocationList(), request, client);
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
                recieved = recieved.Append(buffer);
            return recieved;
        }        
    }
    static class Extensions
    {
        public static byte[] Append(this byte[] source, byte[] second)
        {
            List<byte> vs = new List<byte>(source);
            vs.AddRange(second);
            return vs.ToArray();
        }
    }
}
