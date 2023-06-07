namespace SignalR.Pro.Repository.IRepository;

public interface IConnectionRepository
{
    void Add(string persistentConnectionId, string connectionId);
    void Remove(string connectionId);
    string Get(string persistentConnectionId);
}
