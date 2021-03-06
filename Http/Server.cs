﻿using System;
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

        public void Start(string hostname)
        {
            var a = Dns.GetHostAddresses(hostname);

            for (int i = 0; i < a.Length; i++)
            {
                try
                {
                    _listener = new TcpListener(a[i], 80);
                    _listener.Start();
                    Console.WriteLine($"Bound to {a[i]} (attempt {i + 1})");
                    new Thread(Listen).Start();
                    return;
                }
                catch (Exception)
                {
                    Console.WriteLine($"Failed to bind to {a[i]} (attempt {i + 1})");
                }
            }
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
                    new Thread(() => HandleClientSafe(client)).Start();
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


        private void HandleClientSafe(TcpClient client)
        {
            try
            {
                byte[] buffer = RecieveAllDataSafe(client);
                var request = HttpRequest.Parse(buffer);
                InvokeAll(RequestRecieved.GetInvocationList(), request, client);
            }
            catch
            {
                
            }
        }

        private byte[] RecieveAllDataSafe(TcpClient client)
        {
            byte[] recieved = new byte[client.ReceiveBufferSize];
            client.Client.Receive(recieved, 0, client.ReceiveBufferSize, SocketFlags.None);
            var header = GetHeader(recieved);
            var torecieve = header.RequestParameters.ContentLength - client.ReceiveBufferSize;
            while (torecieve > 0)
            {
                if (torecieve >= client.ReceiveBufferSize)
                {
                    byte[] buffer = new byte[client.ReceiveBufferSize];
                    torecieve -= client.Client.Receive(buffer, 0, client.ReceiveBufferSize, SocketFlags.None);
                    recieved = Append(recieved, buffer);
                }
                else
                {
                    byte[] buffer = new byte[torecieve];
                    torecieve -= client.Client.Receive(buffer, 0, torecieve, SocketFlags.None);
                    recieved = Append(recieved, buffer);
                }
            }
            return recieved;
        }

        private static byte[] Append(byte[] arrayA, byte[] arrayB)
        {
            byte[] outputBytes = new byte[arrayA.Length + arrayB.Length];
            Buffer.BlockCopy(arrayA, 0, outputBytes, 0, arrayA.Length);
            Buffer.BlockCopy(arrayB, 0, outputBytes, arrayA.Length, arrayB.Length);
            return outputBytes;
        }

        private HttpMessage.Message.RequestHeader GetHeader(byte[] raw)
        {
            string ascii = Encoding.ASCII.GetString(raw);
            return HttpMessage.Message.RequestHeader.Parse(ascii.Substring(0, ascii.IndexOf("\r\n\r\n")));
        }

        private void InvokeAll(Delegate[] delegates, HttpRequest request, TcpClient client)
        {
            var handleTasks = new Thread[delegates.Length];
            for (int a = 0; a < delegates.Length; a++)
                ((Func<HttpRequest, TcpClient, int>)delegates[a])(request, client);
            if (client.Connected) { client.Close(); }
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
