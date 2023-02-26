namespace Chat.Data.Models
{
    public class Messages
    {
        /// <summary>
        /// Message id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Message.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Date writing.
        /// </summary>
        public string? DateWrite { get; set; }

        /// <summary>
        /// User name.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Chat identification.
        /// </summary>
        public int ChatId { get; set; }

        /// <summary>
        /// Navigation property for Chatik.
        /// </summary>
        public Chatik? Chatik { get; set; }
    }
}
