using System.ComponentModel.DataAnnotations;

namespace Chat.WebApi.Shared.Models.Request
{
    public class MessageSendRequest
    {
        /// <summary>
        /// Chat identification.
        /// </summary>
        [Required]
        public int ChatId { get; set; }

        /// <summary>
        /// Message.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Date writing.
        /// </summary>
        [Required]
        public string? DateWrite { get; set; }

        /// <summary>
        /// User name.
        /// </summary>
        [Required]
        public string? UserName { get; set; }

        /// <summary>
        /// Path to file.
        /// </summary>
        [Required]
        public string? PathPhoto { get; set; }
    }
}
