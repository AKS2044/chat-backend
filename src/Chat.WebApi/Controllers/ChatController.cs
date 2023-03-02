using Azure.Core;
using Chat.Data.Models;
using Chat.Logic.Interfaces;
using Chat.Logic.Models;
using Chat.WebApi.Attributes;
using Chat.WebApi.Shared.Models.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chat.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatManager _chatManager;
        public ChatController(IChatManager chatManager)
        {
            _chatManager = chatManager ?? throw new ArgumentNullException(nameof(chatManager));
        }

        [OwnAuthorize]
        [HttpPost("add")]
        public async Task<IActionResult> CreateAsync(ChatCreateRequest request)
        {
            DateTime dateReg = DateTime.Now;
            ChatikDto chatikDto = new()
            {
                NameChat = request.NameChat,
                ChatCreator = request.ChatCreator,
                DateCreat = dateReg.ToString("dd MMM yyy"),
            };

            if (ModelState.IsValid)
            {
                await _chatManager.CreateAsync(chatikDto);
            }

            return Ok();
        }
    }
}
