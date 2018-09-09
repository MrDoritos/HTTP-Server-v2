using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Http;
using Http.HttpMessage;
using System.Net;
using System.Net.Sockets;

namespace Http.Tests
{
    class Program
    {
        static Server Server;
        static string hostname = null; 
        static int port = -1; 

        static void Main(string[] args)
        {            
            while (true)
            {
                Server = new Server();
                Console.Write("IP Address: ");
                hostname = Console.ReadLine();

                Console.Write("Port: ");
                while (port == -1)
                {
                    if (ushort.TryParse(Console.ReadLine(), out ushort portt))
                        port = portt;
                    else
                        Console.Write("\r\nInvalid Port\r\n");
                }
                try
                {
                    var addresses = Dns.GetHostAddresses(hostname);
                    try
                    {
                        Server.RequestRecieved += RequestRecieved;
                        Server.Start(new IPEndPoint(addresses[0], port));
                        while (Server.Connected) { Task.Delay(100).Wait(); }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Server stopped ({e.Message})\r\n{e.StackTrace}");
                    }
                } catch(Exception e)
                {
                    Console.WriteLine($"{e.Message}");
                }
            }
        }

        static public async Task RequestRecieved(HttpRequest httpRequest, TcpClient client)
        {
            Console.WriteLine($"{httpRequest.Method} {httpRequest.RequestURI}");
            HttpResponse httpResponse = new HttpResponse(new HttpMessage.Message.ResponseHeader(), new HttpMessage.Message.Content(Encoding.UTF8.GetBytes("<html><head><title>LOL!</title></head><body><form action=\"/\" method=\"POST\"><input type=\"text\" id=\"a\" name=\"a\"><input type=\"submit\"><output for=\"a\"></form></body></html>")));
            httpResponse.ResponseCode = HttpMessage.Message.ResponseHeader.ResponseCodes.OK;
            client.Client.Send(httpResponse.Serialize());
        }
    }
}
