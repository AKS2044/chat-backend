using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Chat.Data.Models
{
    public class User : IdentityUser
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

        /// <summary>
        /// Navigation property for UserChats.
        /// </summary>
        public ICollection<UserChats>? UserChats { get; set; }
    }
}
