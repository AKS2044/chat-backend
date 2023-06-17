using Microsoft.AspNetCore.SignalR;
using Chat.Logic.Models;
using Chat.WebApi.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Chat.Data.Models;
using Chat.WebApi.Shared.Models;
using Azure.Core;

namespace Chat.WebApi
{
    public class ChatHub : Hub<IChatClient>
    {
        private readonly UserManager<User> _userManager;
        private static List<UserConnection> users = new List<UserConnection>();

        public ChatHub(UserManager<User> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task SendMessage(MessagesDto message)
        {
            await Clients.Group("chat" + message.ChatId.ToString()).ReceiveMessage(message);
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

        public async Task OnConnectedAsync(string chatName)
        {
            string token = Context.GetHttpContext()?.Request.Query["access_token"].ToString();
            if(token?.Length == 0 && token != "Unauthorized")
            {
                token = Context.GetHttpContext()?.Request.Headers["Authorization"];
                if(token?.Length > 0) token = token.Replace("Bearer ", "");
            }
            
            if (token != null && token != "Unauthorized")
            {
                var user = await CheckUser(token);

                if (user != null)
                {
                    users.Add(new UserConnection
                    {
                        ConnectedId = Context.ConnectionId,
                        UserName = user.UserName,
                        ChatName = chatName,
                    });

                    await Groups.AddToGroupAsync(Context.ConnectionId, chatName);
                    await Clients.Group(chatName).ConnectedAsync($"{user.UserName} has entered the chat");
                    await Clients.Group(chatName).SendCheckUsers(users);
                    await base.OnConnectedAsync();
                }
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string token = Context.GetHttpContext()?.Request.Query["access_token"].ToString();
            if (token?.Length == 0 && token != "Unauthorized")
            {
                token = Context.GetHttpContext()?.Request.Headers["Authorization"];
                if (token?.Length > 0) token = token.Replace("Bearer ", "");
            }

            if (token != null && token != "Unauthorized")
            {
                var user = await CheckUser(token);

                if (user != null)
                {
                    var userOut = users.FirstOrDefault(
                        u => u.ConnectedId == Context.ConnectionId);

                    if (userOut != null)
                        users.Remove(userOut);

                    if(userOut != null)
                    {
                        await Groups.RemoveFromGroupAsync(Context.ConnectionId, userOut.ChatName);
                        await Clients.Group(userOut.ChatName).DisconnectedAsync($"{user.UserName} has left the chat");
                        await Clients.Group(userOut.ChatName).SendCheckUsers(users);
                        await base.OnDisconnectedAsync(exception);
                    }                }
            }
        }
    }
}
