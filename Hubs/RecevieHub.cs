using Microsoft.AspNetCore.SignalR;
using SignalR.Pro.Models;
using System.Collections.Concurrent;

namespace SignalR.Pro.Hubs;

public class RecevieHub : Hub
{
    private static ConcurrentDictionary<string, string> _connections = new ConcurrentDictionary<string, string>();
    private static ConcurrentDictionary<string, ChatUserDto> _userInfo = new ConcurrentDictionary<string, ChatUserDto>();
    private readonly ILogger _logger;

    public RecevieHub(ILoggerFactory logger)
    {
        _logger = logger.CreateLogger<RecevieHub>();
    }

    public override Task OnConnectedAsync()
    {
        string connectionId = Context.ConnectionId;
        string persistentConnectionId = Context.GetHttpContext().Request.Query["connectionId"];
        _connections.AddOrUpdate(persistentConnectionId, connectionId, (k, v) => {
            return connectionId;
        });

        ChatUserDto chatUserDto = new ChatUserDto() { UserId = persistentConnectionId, UserName = "Tim", ConnectionId = connectionId };
        _userInfo.AddOrUpdate(persistentConnectionId, chatUserDto, (k, v) =>
        {
            return chatUserDto;
        });

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        string persistentConnectionId = Context.GetHttpContext().Request.Query["connectionId"];
        var chatUserDto = new ChatUserDto();
        if (_userInfo.TryRemove(persistentConnectionId, out chatUserDto))
        {
            _logger.LogInformation($"{chatUserDto.UserName} 下线了");
        }

        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string message)
    {
        // string connectionId = _connectionRepository.Get(Context.GetHttpContext().Request.Query["connectionId"]); // 获取持久化的连接ID
        string connectionId = string.Empty;
        string persistentConnectionId = Context.GetHttpContext().Request.Query["connectionId"];
        _connections.TryGetValue(persistentConnectionId, out connectionId);
        await Clients.Client(connectionId).SendAsync("ReceiveMessage", "ReceiveMessage: " + message); // 向指定连接推送数据
        await Task.Delay(10000);

        _connections.TryGetValue(persistentConnectionId, out connectionId);
        await Clients.Client(connectionId).SendAsync("ReceiveData", "ReceiveData: " + message); // 向指定连接推送数据
    }
}
