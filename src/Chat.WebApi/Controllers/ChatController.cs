using Chat.Data.Models;
using Chat.Logic.Interfaces;
using Chat.Logic.Models;
using Chat.WebApi.Attributes;
using Chat.WebApi.Shared.Models.Request;
using Chat.WebApi.Shared.Models.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
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
        public async Task<IActionResult> ChatListUserAsync(string userName)
        {
            if (userName != null)
            {
                try
                {
                    var user = await _userManager.FindByNameAsync(userName);

                    if (user != null)
                    {
                        var chats = await _chatManager.AllChatsUserAsync(user.Id);
                        return Ok(chats);
                    }
                    else
                    {
                        return NotFound(new { message = "User not found" });
                    }
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

        [OwnAuthorize]
        [HttpGet("usersChat")]
        public async Task<IActionResult> UsersInChatAsync(int chatId)
        {
            var usersChat = new List<UsersListInChatResponse>();

            if (chatId > 0)
            {
                var users = await _chatManager.AllUsersInChatAsync(chatId);
                foreach (var item in users)
                {
                    usersChat.Add(new UsersListInChatResponse
                    {
                        Id = item.Id,
                        Email = item.Email,
                        UserName = item.UserName,
                        PathPhoto = item.PathPhoto,
                        DateReg = item.DateReg,
                    });
                }
                return Ok(usersChat);
            }
            else
            {
                return BadRequest(new { message = "Wrong id" });
            }
        }

        [OwnAuthorize]
        [HttpGet("searchChat")]
        public async Task<IActionResult> SearchChatAsync(string chatName)
        {
            var response = new List<ChatListResponse>();

            if (chatName != null)
            {
                var chats = await _chatManager.SeacrchChatByNameAsync(chatName);
                foreach (var item in chats)
                {
                    response.Add(new ChatListResponse
                    {
                        Id = item.Id,
                        DateCreat = item.DateCreat,
                        ChatCreator = item.ChatCreator,
                        NameChat = item.NameChat
                    });
                }
                return Ok(response);
            }
            else
            {
                return Ok(new { message = "Not found" });
            }
        }

        [OwnAuthorize]
        [HttpPost("enter")]
        public async Task<IActionResult> EnterAsync(EnterLeaveTheChatRequest request)
        {
            var result = new UserChatsDto()
            {
                UserId = request.UserId,
                ChatId = request.ChatId
            };

            if (ModelState.IsValid)
            {
                await _chatManager.EnterTheChatAsync(result);

                return Ok();
            }

            return BadRequest(new {message = "BadRequest" });
        }

        [OwnAuthorize]
        [HttpDelete("leave")]
        public async Task<IActionResult> LeaveAsync([FromQuery]EnterLeaveTheChatRequest request)
        {
            var result = new UserChatsDto()
            {
                UserId = request.UserId,
                ChatId = request.ChatId
            };

            if (ModelState.IsValid)
            {
                await _chatManager.LeaveTheChatAsync(result);

                return Ok();
            }

            return BadRequest(new { message = "BadRequest" });
        }
    }
}
