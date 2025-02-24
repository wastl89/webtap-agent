using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using OpenTap.Diagnostic;
namespace webtap_agent.Controllers;

[Route("logs")]
[ApiController]
public class LogController : ControllerBase
{
    private static ConcurrentBag<WebSocket> _webSockets = new ConcurrentBag<WebSocket>();

    [HttpGet]
    public async Task Get() 
    {
        if(HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            _webSockets.Add(webSocket);
            await HandleWebSocketAsync(webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = 400;
        }
    }

    private async Task HandleWebSocketAsync(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];

            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine($"Received: {receivedMessage}");

                    // Echo zurück an den Client
                    var echoMessage = Encoding.UTF8.GetBytes($"Echo: {receivedMessage}");
                    await webSocket.SendAsync(new ArraySegment<byte>(echoMessage), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    _webSockets.TryTake(out _);
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);
                }
            }
        }

    public static async Task BroadcastAsync(string message)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        foreach (var socket in _webSockets)
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}