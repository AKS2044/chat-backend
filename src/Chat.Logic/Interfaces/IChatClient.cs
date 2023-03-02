using Chat.Logic.Models;

namespace Chat.Logic.Interfaces
{
    public interface IChatClient
    {
        Task ReceiveMessage(MessagesDto message);
    }
}
