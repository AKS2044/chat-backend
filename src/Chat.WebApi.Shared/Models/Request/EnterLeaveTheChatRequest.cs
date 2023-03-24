using System.ComponentModel.DataAnnotations;

namespace Chat.WebApi.Shared.Models.Request
{
    public class EnterLeaveTheChatRequest
    {
        /// <summary>
        /// User id.
        /// </summary>
        [Required]
        public string? UserId { get; set; }

        /// <summary>
        /// Chat id.
        /// </summary>
        [Required]
        public int ChatId{ get; set; }
    }
}
