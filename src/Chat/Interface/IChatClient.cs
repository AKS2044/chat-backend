using Chat.Models;

namespace Chat.Interface
{
    public interface IChatClient
    {
        Task ReceiveMessage(ChatMessage message);
    }
}
