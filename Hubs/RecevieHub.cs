using Microsoft.AspNetCore.SignalR;

namespace SignalR.Pro.Hubs;

public class RecevieHub : Hub
{
    private readonly ILogger _logger;

    public RecevieHub(ILoggerFactory logger)
    {
        _logger = logger.CreateLogger<RecevieHub>();
    }

    public async Task SendDataAsync(string connectionId, string message)
    {
        await Clients.Client(connectionId).SendAsync("ReceiveData", message);
    }

    public async Task SendMsgAsync(string connectionId, string server, string message)
    {
        await Clients.Client(connectionId).SendAsync(server, message);
    }
}
