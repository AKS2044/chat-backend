using Microsoft.AspNetCore.SignalR;
using Chat.Logic.Models;
using Chat.WebApi.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Chat.Data.Models;

namespace Chat.WebApi
{
    public class ChatHub : Hub<IChatClient>
    {
        private readonly UserManager<User> _userManager;

        public ChatHub(UserManager<User> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task SendMessage(MessagesDto message)
        {
            //await Clients.All.ReceiveMessage(message);
            await Clients.All.ReceiveMessage(message);
        }

        public async Task<User> CheckUser(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            token = token.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;
            var id = tokenS?.Claims.First(claim => claim.Type == "id").Value;

            var user = await _userManager.FindByIdAsync(id);
            return user;
        }

        public override async Task OnConnectedAsync()
        {
            var token = Context.GetHttpContext()?.Request.Query["access_token"].ToString();

            if (token is not null && token != "Unauthorized")
            {
                var user = await CheckUser(token);
                await Clients.All.ConnectedAsync($"{user.UserName} has entered the chat");
                await base.OnConnectedAsync();
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var token = Context.GetHttpContext()?.Request.Query["access_token"].ToString();

            if (token is not null && token != "Unauthorized")
            {
                var user = await CheckUser(token);
                await Clients.All.DisconnectedAsync($"{user.UserName} has left the chat");
                await base.OnDisconnectedAsync(exception);
            }
        }
    }
}
