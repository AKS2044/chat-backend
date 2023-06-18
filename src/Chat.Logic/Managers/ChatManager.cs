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
            var checkChat = await _chatRepository.GetAll().FirstOrDefaultAsync(c => c.NameChat == chatikDto.NameChat);

            if (checkChat == null)
            {
                var chat = new Chatik()
                {
                    NameChat = chatikDto.NameChat,
                    ChatCreator = chatikDto.ChatCreator,
                    DateCreat = chatikDto.DateCreat,
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
            if (message != null)
            {
                _messageRepository.Delete(message);
                await _messageRepository.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<MessagesDto>> AllMessageChatAsync(int chatId)
        {
            var response = new List<MessagesDto>();
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

            foreach (var item in MessagesDto)
            {
                var user = await _userManager.FindByNameAsync(item.UserName);
                response.Add(new MessagesDto
                {
                    Id = item.Id,
                    ChatId = item.ChatId,
                    UserName = item.UserName,
                    Message = item.Message,
                    PathPhoto = user.PathPhoto,
                    DateWrite = item.DateWrite,
                });
                if (user != null) item.PathPhoto = user.PathPhoto;
            }

            return response;
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

            if (chatik != null)
            {
                var chatikDto = new ChatikDto()
                {
                    Id = chatik.Id,
                    DateCreat = chatik.DateCreat,
                    NameChat = chatik.NameChat,
                    ChatCreator = chatik.ChatCreator,
                };

                return chatikDto;
            }
            return null;
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
            if (chatName != null)
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
            return null;
        }

        public async Task EnterTheChatAsync(UserChatsDto userChatsDto)
        {
            if (userChatsDto != null)
            {
                var enter = new UserChats()
                {
                    ChatId = userChatsDto.ChatId,
                    UserId = userChatsDto.UserId
                };
                await _userChatsRepository.CreateAsync(enter);
                await _userChatsRepository.SaveChangesAsync();
            }
        }

        public async Task LeaveTheChatAsync(UserChatsDto userChatsDto)
        {
            if (userChatsDto != null)
            {
                var delete = await _userChatsRepository
                    .GetAll()
                    .FirstOrDefaultAsync(d => d.UserId == userChatsDto.UserId && d.ChatId == userChatsDto.ChatId);
                if (delete != null)
                {
                    _userChatsRepository.Delete(delete);
                    await _userChatsRepository.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteChatAsync(int chatId)
        {
            var chat = await _chatRepository.GetAll().FirstOrDefaultAsync(c => c.Id == chatId);

            if(chat != null)
            {
                var messages = await _messageRepository.GetAll().Where(m => m.ChatId == chatId).ToListAsync();
                if (messages != null)
                {
                    _messageRepository.DeleteRange(messages);
                    await _messageRepository.SaveChangesAsync();
                } 

                var userChats = await _userChatsRepository.GetAll().Where(c => c.ChatId == chatId).ToListAsync();
                if (userChats != null)
                {
                    _userChatsRepository.DeleteRange(userChats);
                    await _userChatsRepository.SaveChangesAsync();
                } 

                var chatDelete = await _chatRepository.GetAll().FirstOrDefaultAsync(c => c.Id == chatId);
                if (chatDelete != null)
                {
                    _chatRepository.Delete(chatDelete);
                    await _chatRepository.SaveChangesAsync();
                }
            }
        }
    }
}
