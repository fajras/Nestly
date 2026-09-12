using System.Collections.Concurrent;

namespace Nestly.Services.Messaging
{
    public interface IUserConnectionTracker
    {
        void AddConnection(long userId, string connectionId);
        void RemoveConnection(long userId, string connectionId);
        bool IsOnline(long userId);
        int OnlineUserCount { get; }
    }

    // In-memory presence tracker shared by ChatHub and NotificationHub.
    // A user can have several live SignalR connections at once (multiple
    // devices/tabs), so we keep a set of connection ids per user and only
    // consider them offline once the last one disconnects. This lets the
    // notifiers below skip pushing to a group with no active connections
    // instead of firing SendAsync into the void every time.
    //
    // Single-process, in-memory store: fine for this deployment, but would
    // need a shared backing store (e.g. Redis) behind a SignalR backplane
    // if the API ever scales out to multiple instances.
    public class UserConnectionTracker : IUserConnectionTracker
    {
        private readonly ConcurrentDictionary<long, ConcurrentDictionary<string, byte>> _connections = new();

        public void AddConnection(long userId, string connectionId)
        {
            var set = _connections.GetOrAdd(userId, _ => new ConcurrentDictionary<string, byte>());
            set[connectionId] = 0;
        }

        public void RemoveConnection(long userId, string connectionId)
        {
            if (_connections.TryGetValue(userId, out var set))
            {
                set.TryRemove(connectionId, out _);

                if (set.IsEmpty)
                {
                    _connections.TryRemove(userId, out _);
                }
            }
        }

        public bool IsOnline(long userId)
        {
            return _connections.TryGetValue(userId, out var set) && !set.IsEmpty;
        }

        public int OnlineUserCount => _connections.Count;
    }
}
