using System.ComponentModel.DataAnnotations;

namespace Chat.WebApi.Shared.Models.Request
{
    public class UserLogoutRequest
    {
        /// <summary>
        /// User Name.
        /// </summary>
        [Required]
        public string? UserName { get; set; }
    }
}
