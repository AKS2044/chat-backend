namespace Chat.WebApi.Shared.Models.Responses
{
    public class ProfileUserResponse
    {
        /// <summary>
        /// Identification.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Date registration.
        /// </summary>
        public string DateReg { get; set; }

        /// <summary>
        /// Roles.
        /// </summary>
        public IList<string> Roles { get; set; }

        /// <summary>
        /// Path to file.
        /// </summary>
        public string PathPhoto { get; set; }

        /// <summary>
        /// Photo name.
        /// </summary>
        public string PhotoName { get; set; }
    }
}
