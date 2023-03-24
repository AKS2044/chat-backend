using Chat.Logic.Models;

namespace Chat.Logic.Interfaces
{
    /// <summary>
    /// Chat manager.
    /// </summary>
    public interface IChatManager
    {
        /// <summary>
        /// Create chat.
        /// </summary>
        /// <param name="chatikDto">State data transfer object.</param>
        Task CreateAsync(ChatikDto chatikDto);

        /// <summary>
        /// Send message to chat.
        /// </summary>
        /// <param name="chatikDto">State data transfer object.</param>
        Task SendAsync(MessagesDto messagesDto);

        /// <summary>
        /// Get all chats.
        /// </summary>
        Task<IEnumerable<ChatikDto>> AllChatsAsync();

        /// <summary>
        /// Get all chats user.
        /// </summary>
        /// <param name="userId">User id.</param>
        Task<IEnumerable<ChatikDto>> AllChatsUserAsync(string userId);

        /// <summary>
        /// All users in the chat.
        /// </summary>
        /// <param name="chatId">Chat id.</param>
        Task<IEnumerable<UserDto>> AllUsersInChatAsync(int chatId);

        /// <summary>
        /// Get all messages the chat.
        /// </summary>
        /// <param name="chatId">Chat id.</param>
        Task<IEnumerable<MessagesDto>> AllMessageChatAsync(int chatId);

        /// <summary>
        /// Delete message.
        /// </summary>
        /// <param name="messageId">Message id.</param>
        Task DeleteMessageAsync(int messageId);

        /// <summary>
        /// Get chat by id.
        /// </summary>
        /// <param name="chatId">Chat id.</param>
        Task<ChatikDto> GetChatByIdAsync(int chatId);

        /// <summary>
        /// Get chat by name.
        /// </summary>
        /// <param name="chatName">Chat name.</param>
        Task<List<ChatikDto>> SeacrchChatByNameAsync(string chatName);

        /// <summary>
        /// Enter the chat.
        /// </summary>
        /// <param name="userChatsDto">State data transfer object.</param>
        Task EnterTheChatAsync(UserChatsDto userChatsDto);

        /// <summary>
        /// Leave the chat.
        /// </summary>
        /// <param name="userChatsDto">State data transfer object.</param>
        Task LeaveTheChatAsync(UserChatsDto userChatsDto);
    }
}
