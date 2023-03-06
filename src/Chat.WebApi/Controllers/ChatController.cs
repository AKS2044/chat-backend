using Azure.Core;
using Chat.Data.Models;
using Chat.Logic.Interfaces;
using Chat.Logic.Models;
using Chat.WebApi.Attributes;
using Chat.WebApi.Shared.Models.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Chat.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatManager _chatManager;
        private readonly UserManager<User> _userManager;
        public ChatController(IChatManager chatManager, UserManager<User> userManager)
        {
            _chatManager = chatManager ?? throw new ArgumentNullException(nameof(chatManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [OwnAuthorize]
        [HttpPost("addChat")]
        public async Task<IActionResult> CreateAsync(ChatCreateRequest request)
        {
            string? token = Request.Headers["Authorization"];
            if (token is not null)
            {
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    token = token.Replace("Bearer ", "");
                    var jsonToken = handler.ReadToken(token);
                    var tokenS = handler.ReadToken(token) as JwtSecurityToken;
                    var id = tokenS?.Claims.First(claim => claim.Type == "id").Value;
                    var user = await _userManager.FindByIdAsync(id);

                    ChatikDto chatikDto = new()
                    {
                        NameChat = request.NameChat,
                        ChatCreator = user.Id,
                    };

                    if (ModelState.IsValid)
                    {
                        await _chatManager.CreateAsync(chatikDto);
                    }
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }
            else
            {
                return BadRequest(new { message = "Не авторизованы" });
            }
        }

        [OwnAuthorize]
        [HttpPost("message")]
        public async Task<IActionResult> SendMessageAsync(MessagesDto messagesDto)
        {
            if (ModelState.IsValid)
            {
                await _chatManager.SendAsync(messagesDto);
            }

            return Ok();
        }
    }
}
