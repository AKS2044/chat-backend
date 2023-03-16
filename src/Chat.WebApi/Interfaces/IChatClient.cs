using Chat.Logic.Models;

namespace Chat.WebApi.Interfaces
{
    public interface IChatClient
    {
        /// <summary>
        /// Send message in chat.
        /// </summary>
        /// <param name="message">Modal message</param>
        Task ReceiveMessage(MessagesDto message);

        /// <summary>
        /// Message about connected a person
        /// </summary>
        /// <param name="message">Message</param>
        Task ConnectedAsync(string message);

        /// <summary>
        /// Message about disconnected a person
        /// </summary>
        /// <param name="message">Message</param>
        Task DisconnectedAsync(string message);
    }
}
