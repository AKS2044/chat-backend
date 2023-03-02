using Chat.Data.Models;
using Chat.Logic.Interfaces;
using Chat.Logic.Models;

namespace Chat.Logic.Managers
{
    /// <inheritdoc cref="IChatManager"/>
    public class ChatManager : IChatManager
    {
        private readonly IRepositoryManager<Chatik> _chatRepository;

        public ChatManager(IRepositoryManager<Chatik> chatRepository)
        {
            _chatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
        }
        public async Task CreateAsync(ChatikDto chatikDto)
        {
            var chat = new Chatik()
            {
                NameChat = chatikDto.NameChat,
                ChatCreator = chatikDto.ChatCreator,
                DateCreat = chatikDto.DateCreat,
            };

            await _chatRepository.CreateAsync(chat);
            await _chatRepository.SaveChangesAsync();
        }
    }
}
