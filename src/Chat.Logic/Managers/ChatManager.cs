using Chat.Data.Models;
using Chat.Logic.Interfaces;
using Chat.Logic.Models;

namespace Chat.Logic.Managers
{
    /// <inheritdoc cref="IChatManager"/>
    public class ChatManager : IChatManager
    {
        private readonly IRepositoryManager<Chatik> _chatRepository;
        private readonly IRepositoryManager<Messages> _messageRepository;

        public ChatManager(
            IRepositoryManager<Chatik> chatRepository, 
            IRepositoryManager<Messages> messageRepository)
        {
            _chatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
            _messageRepository = messageRepository ?? throw new ArgumentNullException(nameof(messageRepository));
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
    }
}
