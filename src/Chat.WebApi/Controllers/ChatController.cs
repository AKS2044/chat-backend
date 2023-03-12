using Azure.Core;
using Chat.Data.Models;
using Chat.Logic.Interfaces;
using Chat.Logic.Models;
using Chat.WebApi.Attributes;
using Chat.WebApi.Shared.Models.Request;
using Chat.WebApi.Shared.Models.Responses;
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
                    DateTime dateReg = DateTime.Now;

                    ChatikDto chatikDto = new()
                    {
                        NameChat = request.NameChat,
                        ChatCreator = user.Id,
                        DateCreat = dateReg.ToString("dd MMM yyy")
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
        [HttpGet("chatList")]
        public async Task<IActionResult> ChatListAsync()
        {
            var chats = await _chatManager.AllChatsAsync();
            return Ok(chats);
        }

        [OwnAuthorize]
        [HttpGet("mesList")]
        public async Task<IActionResult> MessageListAsync(int chatId)
        {
            try
            {
                var chats = await _chatManager.AllMessageChatAsync(chatId);
                return Ok(chats);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [OwnAuthorize]
        [HttpGet("getChat")]
        public async Task<IActionResult> GetChatByIdAsync(int chatId)
        {
            try
            {
                var chat = await _chatManager.GetChatByIdAsync(chatId);
                return Ok(chat);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [OwnAuthorize]
        [HttpGet("chatsUser")]
        public async Task<IActionResult> ChatListUserAsync()
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
                    var chats = await _chatManager.AllChatsUserAsync(user.Id);

                    if (chats is null)
                    {
                        return Ok(new { message =  "Chats not found" });
                    }
                    return Ok(chats);
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
        public async Task<IActionResult> SendMessageAsync(MessageSendRequest request)
        {
            var message = new MessagesDto()
            {
                UserName = request.UserName,
                DateWrite = request.DateWrite,
                ChatId = request.ChatId,
                Message = request.Message,
                PathPhoto = request.PathPhoto,
            };
            if (ModelState.IsValid)
            {
                await _chatManager.SendAsync(message);
            }

            return Ok();
        }

        [OwnAuthorize]
        [HttpDelete("messageDelete")]
        public async Task<IActionResult> MessageDeleteAsync(int messageId)
        {
            if (messageId > 0)
            {
                await _chatManager.DeleteMessageAsync(messageId);
            }

            return Ok();
        }
    }
}
