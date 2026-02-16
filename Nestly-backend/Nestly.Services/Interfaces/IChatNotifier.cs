using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IChatNotifier
    {
        Task NotifyUser(long userId, ChatMessageRealtimeDto message);
    }

}
