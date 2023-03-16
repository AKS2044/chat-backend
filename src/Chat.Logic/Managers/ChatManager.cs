using Chat.Data.Models;
using Chat.Logic.Interfaces;
using Chat.Logic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Chat.Logic.Managers
{
    /// <inheritdoc cref="IChatManager"/>
    public class ChatManager : IChatManager
    {
        private readonly IRepositoryManager<Chatik> _chatRepository;
        private readonly IRepositoryManager<Messages> _messageRepository;
        private readonly IRepositoryManager<UserChats> _userChatsRepository;
        private readonly UserManager<User> _userManager;

        public ChatManager(
            IRepositoryManager<Chatik> chatRepository, 
            IRepositoryManager<Messages> messageRepository,
            IRepositoryManager<UserChats> userChatsRepository,
            UserManager<User> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _chatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
            _messageRepository = messageRepository ?? throw new ArgumentNullException(nameof(messageRepository));
            _userChatsRepository = userChatsRepository ?? throw new ArgumentNullException(nameof(userChatsRepository));
        }
        public async Task CreateAsync(ChatikDto chatikDto)
        {
            DateTime dateReg = DateTime.Now;
            var chat = new Chatik()
            {
                NameChat = chatikDto.NameChat,
                ChatCreator = chatikDto.ChatCreator,
                DateCreat = dateReg.ToString("dd MMM yyy"),
            };

            await _chatRepository.CreateAsync(chat);
            await _chatRepository.SaveChangesAsync();

            var userChats = new UserChats()
            {
                ChatId = chat.Id,
                UserId = chat.ChatCreator
            };
            await _userChatsRepository.CreateAsync(userChats);
            await _userChatsRepository.SaveChangesAsync();
        }

        public async Task SendAsync(MessagesDto messagesDto)
        {
            var messages = new Messages()
            {
                ChatId = messagesDto.ChatId,
                Message = messagesDto.Message,
                DateWrite = messagesDto.DateWrite,
                UserName = messagesDto.UserName,
                PathPhoto = messagesDto.PathPhoto
            };

            await _messageRepository.CreateAsync(messages);
            await _messageRepository.SaveChangesAsync();
        }

        public async Task DeleteMessageAsync(int messageId)
        {
            var message = await _messageRepository.GetAll().SingleOrDefaultAsync( m => m.Id == messageId);
            _messageRepository.Delete(message);
            await _messageRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<MessagesDto>> AllMessageChatAsync(int chatId)
        {
            var MessagesDto = await _messageRepository
                .GetAll().Where(m => m.ChatId == chatId).Select(m => new MessagesDto
                {
                    Id = m.Id,
                    ChatId = m.ChatId,
                    UserName = m.UserName,
                    Message = m.Message,
                    PathPhoto = m.PathPhoto,
                    DateWrite=m.DateWrite,
                }).ToListAsync();

            return MessagesDto;
        }

        public async Task<IEnumerable<ChatikDto>> AllChatsAsync()
        {
            var chatsDto = await _chatRepository
                .GetAll().Select(c => new ChatikDto
                {
                    Id = c.Id,
                    NameChat = c.NameChat,
                    DateCreat = c.DateCreat,
                    ChatCreator = c.ChatCreator,
                }).ToListAsync();

            return chatsDto;
        }

        public async Task<IEnumerable<ChatikDto>> AllChatsUserAsync(string userId)
        {
            var userChats = await _userChatsRepository
                .GetAll()
                .Where(c => c.UserId == userId)
                .Select(r => r.ChatId)
                .ToListAsync();
            var chatsDto = await _chatRepository
                .GetAll()
                .Where(c => userChats.Contains(c.Id))
                .Select(c => new ChatikDto
                    {
                        Id = c.Id,
                        NameChat = c.NameChat,
                        DateCreat = c.DateCreat,
                        ChatCreator = c.ChatCreator,
                    })
                .ToListAsync();
            return chatsDto;
        }

        public async Task<ChatikDto> GetChatByIdAsync(int chatId)
        {
            var chatik = await _chatRepository.GetAll().SingleOrDefaultAsync( c => c.Id == chatId);

            var chatikDto = new ChatikDto()
            {
                Id = chatik.Id,
                DateCreat = chatik.DateCreat,
                NameChat = chatik.NameChat,
                ChatCreator = chatik.ChatCreator,
            };

            return chatikDto;
        }

        public async Task<IEnumerable<UserDto>> AllUsersInChatAsync(int chatId)
        {
            var usersChat = await _userChatsRepository
                .GetAll()
                .Where(c => c.ChatId == chatId)
                .Select(r => r.UserId)
                .ToListAsync();

            var usersDto = await _userManager.Users
                .Where(u => usersChat.Contains(u.Id))
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    DateReg = u.DateReg,
                    PathPhoto = u.PathPhoto,

                }).ToListAsync();

            return usersDto;
        }

        public async Task<List<ChatikDto>> SeacrchChatByNameAsync(string chatName)
        {
            var result = await _chatRepository.GetAll()
                .Where(c => c.NameChat.ToLower()
                .Contains(chatName.ToLower()))
                .Select(u => new ChatikDto
                {
                    Id = u.Id,
                    NameChat = u.NameChat,
                    ChatCreator = u.ChatCreator,
                    DateCreat = u.DateCreat

                }).ToListAsync();
            return result;
        }
    }
}
