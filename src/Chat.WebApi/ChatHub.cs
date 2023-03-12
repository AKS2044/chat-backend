using Microsoft.AspNetCore.SignalR;
using Chat.Logic.Models;
using Chat.WebApi.Interfaces;
using Chat.WebApi.Attributes;
using Chat.WebApi.Shared.Models;
using Microsoft.AspNetCore.Authentication;

namespace Chat.WebApi
{
    public class ChatHub : Hub<IChatClient>
    {
        public async Task SendMessage(MessagesDto message)
        {
            //await Clients.All.ReceiveMessage(message);
            await Clients.All.ReceiveMessage(message);
        }

        public override async Task OnConnectedAsync()
        {
            var context = Context?.GetHttpContext();
            await Clients.All.SendAsync($"{Context?.ConnectionId} вошел в чат");
            await base.OnConnectedAsync();
        }
    }
}
