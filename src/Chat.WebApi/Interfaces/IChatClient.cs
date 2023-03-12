using Chat.Logic.Models;

namespace Chat.WebApi.Interfaces
{
    public interface IChatClient
    {
        Task ReceiveMessage(MessagesDto message);
        Task SendAsync(string message);
    }
}
