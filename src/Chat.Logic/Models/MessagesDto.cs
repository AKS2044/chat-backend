namespace Chat.Logic.Models
{
    public class MessagesDto
    {
        /// <summary>
        /// Message id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Chat identification.
        /// </summary>
        public int ChatId { get; set; }

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
        /// Path to file.
        /// </summary>
        public string? PathPhoto { get; set; }
    }
}
