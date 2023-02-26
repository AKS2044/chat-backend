using Microsoft.AspNetCore.SignalR;
using Chat.Models;
using Chat.Interface;

namespace Chat
{
    public class ChatHub : Hub<IChatClient>
    {
        public async Task SendMessage(ChatMessage message)
        {
            await Clients.All.ReceiveMessage(message);
        }
    }
}
