WebSocketProxy ![build status](https://travis-ci.org/lifeemotions/websocketproxy.svg?branch=master) [![NuGet](https://img.shields.io/nuget/v/WebSocketProxy.svg?maxAge=2592000)]()
=======

WebSocketProxy is a lightweight C# library which allows you to connect to an HTTP server and websocket server through the same TCP port.
This library is independent on which web frameworks are used for handling websocket and http requests.

# Installation

Installation can be done using NuGet.

```
Install-Package patrikvalentiny-WebSocketProxy
```

# Sample Usage
The project "WebSocket.Sample" contains a sample usage using dotnet WebApi and [Fleck](https://github.com/statianzo/Fleck).

The first step is to build the configuration object in which you set the endpoints that are listening for HTTP (dotnet) and WebSocket requests (Fleck) and the public endpoint which will be listening to both kinds of requests. 
```csharp
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

```
Then, initialize Api and Fleck, followed by the WebSocketServer.

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add your api setup

builder.WebHost.UseUrls("http://*:5000");
var app = builder.Build();

using var websocketServer = new WebSocketServer("ws://0.0.0.0:8181");
using var tcpProxy = new TcpProxyServer(configuration);
websocketServer.Start(connection =>
{
    connection.OnOpen = () => Console.WriteLine("COnnection on open");
    connection.OnClose = () => Console.WriteLine("Connection on close");
    connection.OnMessage = message => Console.WriteLine("Message: " + message);
});

tcpProxy.Start();
app.Run();
```
By pointing the web browser to the port 8080, you will receive an html page which initializes a websocket connection.

# SSL Support
This library supports HTTPS and WSS. Simply pass the certificate path and password in the configuration object and all incoming requests will be unencrypted and routed to the corresponding library.

