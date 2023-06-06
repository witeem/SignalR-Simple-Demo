using Microsoft.AspNetCore.SignalR;

namespace SignalR.Pro.Hubs;

public class ChatHub : Hub
{
    private readonly ILogger _logger;

    public ChatHub(ILoggerFactory logger)
    { 
        _logger = logger.CreateLogger<ChatHub>();
    }

    public async Task SendMessage(string room, string message)
    {
        await Clients.Group(room).SendAsync("ReceiveMessage", $"{Context.ConnectionId} Say: {message} {DateTime.Now}" );
    }

    public Task JoinRoom(string roomName)
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, roomName);
    }

    public Task LeaveRoom(string connectionId, string roomName)
    {
        return Groups.RemoveFromGroupAsync(connectionId, roomName);
    }
}
