using System;
using System.Net;
using Fleck;

namespace WebSocketProxy.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpProxyConfiguration configuration = new TcpProxyConfiguration()
            {
                PublicHost = new Host()
                {
                    IpAddress = IPAddress.Parse("0.0.0.0"),
                    Port = 8080
                },
                HttpHost = new Host()
                {
                    IpAddress = IPAddress.Loopback,
                    Port = 8081
                },
                WebSocketHost = new Host()
                {

                    IpAddress = IPAddress.Loopback,
                    Port = 8082
                }
            };


            using var websocketServer = new WebSocketServer("ws://0.0.0.0:8082");
            using var tcpProxy = new TcpProxyServer(configuration);
            websocketServer.Start(connection =>
            {
                connection.OnOpen = () => Console.WriteLine("COnnection on open");
                connection.OnClose = () => Console.WriteLine("Connection on close");
                connection.OnMessage = message => Console.WriteLine("Message: " + message);
            });

            tcpProxy.Start();

            Console.WriteLine("Press [Enter] to stop");
            Console.ReadLine();
        }
    }
}
