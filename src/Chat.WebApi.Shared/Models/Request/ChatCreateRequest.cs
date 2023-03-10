using System.ComponentModel.DataAnnotations;

namespace Chat.WebApi.Shared.Models.Request
{
    public class ChatCreateRequest
    {
        /// <summary>
        /// Name chat.
        /// </summary>
        [Required]
        public string? NameChat { get; set; }

        /// <summary>
        /// Chat creator.
        /// </summary>
        public string? ChatCreator { get; set; }
    }
}
