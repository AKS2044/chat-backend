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
        /// Get all messages the chat.
        /// </summary>
        /// <param name="chatId">Chat id.</param>
        Task<IEnumerable<MessagesDto>> AllMessageChatAsync(int chatId);

        /// <summary>
        /// Delete message.
        /// </summary>
        /// <param name="messageId">Message id.</param>
        Task DeleteMessageAsync(int messageId);
    }
}
