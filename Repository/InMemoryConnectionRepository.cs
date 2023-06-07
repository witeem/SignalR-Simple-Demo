using SignalR.Pro.Repository.IRepository;
using System.Collections.Concurrent;

namespace SignalR.Pro.Repository;

public class InMemoryConnectionRepository : IConnectionRepository
{
    private readonly ConcurrentDictionary<string, string> _connections = new ConcurrentDictionary<string, string>();

    public void Add(string persistentConnectionId, string connectionId)
    {
        _connections.TryAdd(persistentConnectionId, connectionId);
    }

    public void Remove(string connectionId)
    {
        _connections.TryRemove(connectionId, out _);
    }

    public string Get(string persistentConnectionId)
    {
        _connections.TryGetValue(persistentConnectionId, out string connectionId);
        return connectionId;
    }
}
