namespace Chat.Logic.Models
{
    public class MessagesDto
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
    }
}
