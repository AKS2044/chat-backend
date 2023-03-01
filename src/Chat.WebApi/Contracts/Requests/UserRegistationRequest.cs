using System.ComponentModel.DataAnnotations;

namespace Chat.WebApi.Contracts.Requests
{
    public class UserRegistationRequest
    {
        /// <summary>
        /// Email.
        /// </summary>
        [Required]
        public string? Email { get; set; }

        /// <summary>
        /// User Name.
        /// </summary>
        [Required]
        public string? UserName { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        [Required]
        public string? Password { get; set; }

        /// <summary>
        /// Password Confirm.
        /// </summary>
        [Required]
        public string? PasswordConfirm { get; set; }

        /// <summary>
        /// Path to file.
        /// </summary>
        public string? PathPhoto { get; set; }

        /// <summary>
        /// Photo name.
        /// </summary>
        public string? PhotoName { get; set; }
    }
}
