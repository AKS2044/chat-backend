namespace Chat.Logic.Models
{
    /// <summary>
    /// User chats.
    /// </summary>
    public class UserChatsDto
    {
        /// <summary>
        /// Identification.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Chat identification.
        /// </summary>
        public int ChatId { get; set; }

        /// <summary>
        /// User identification.
        /// </summary>
        public string? UserId { get; set; }
    }
}
