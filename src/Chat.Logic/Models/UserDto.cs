using Microsoft.AspNetCore.Identity;

namespace Chat.Logic.Models
{
    public class UserDto : IdentityUser
    {

        /// <summary>
        /// Path to file.
        /// </summary>
        public string? PathPhoto { get; set; }

        /// <summary>
        /// Photo name.
        /// </summary>
        public string? PhotoName { get; set; }

        /// <summary>
        /// Date registration.
        /// </summary>
        public string? DateReg { get; set; }
    }
}
