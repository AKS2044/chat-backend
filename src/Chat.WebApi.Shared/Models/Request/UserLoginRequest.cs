using System.ComponentModel.DataAnnotations;

namespace Chat.WebApi.Shared.Models.Request
{
    public class UserLoginRequest
    {
        /// <summary>
        /// User name.
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Remember me.
        /// </summary>
        public bool RememberMe { get; set; }
    }
}
