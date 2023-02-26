namespace Chat.Data.Models
{
    /// <summary>
    /// User chats.
    /// </summary>
    public class UserChats
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
        /// Navigation property for Chatik.
        /// </summary>
        public Chatik? Chatik { get; set; }

        /// <summary>
        /// User identification.
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Navigation property for User.
        /// </summary>
        public User? User { get; set; }
    }
}
