using Microsoft.AspNetCore.SignalR;
using Chat.Logic.Models;
using Chat.Logic.Interfaces;

namespace Chat.WebApi
{
    public class ChatHub : Hub<IChatClient>
    {
        public async Task SendMessage(MessagesDto message)
        {
            await Clients.All.ReceiveMessage(message);
        }
    }
}
