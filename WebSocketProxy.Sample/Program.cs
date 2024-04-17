using System;
using System.Net;
using Fleck;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using WebSocketProxy;


var builder = WebApplication.CreateBuilder(args);

// Add your api setup

builder.WebHost.UseUrls("http://*:5000");
var app = builder.Build();


var configuration = new TcpProxyConfiguration
{
    PublicHost = new Host
    {
        IpAddress = IPAddress.Parse("0.0.0.0"),
        Port = 8080
    },
    HttpHost = new Host
    {
        IpAddress = IPAddress.Loopback,
        Port = 5000
    },
    WebSocketHost = new Host
    {
        IpAddress = IPAddress.Loopback,
        Port = 8181
    }
};


using var websocketServer = new WebSocketServer("ws://0.0.0.0:8181");
using var tcpProxy = new TcpProxyServer(configuration);
websocketServer.Start(connection =>
{
    connection.OnOpen = () => Console.WriteLine("Connection on open");
    connection.OnClose = () => Console.WriteLine("Connection on close");
    connection.OnMessage = message => Console.WriteLine("Message: " + message);
});

tcpProxy.Start();
app.Run();