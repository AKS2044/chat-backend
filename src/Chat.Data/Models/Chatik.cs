namespace Chat.Data.Models
{
    public class Chatik
    {
        /// <summary>
        /// Chat id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name chat.
        /// </summary>
        public string? NameChat { get; set; }

        /// <summary>
        /// Chat creator.
        /// </summary>
        public string? ChatCreator { get; set; }

        /// <summary>
        /// Date of creation.
        /// </summary>
        public string? DateCreat { get; set; }

        /// <summary>
        /// Navigation property for Messages.
        /// </summary>
        public ICollection<Messages>? Messages { get; set; }

        /// <summary>
        /// Navigation property for UserChats.
        /// </summary>
        public ICollection<UserChats>? UserChats { get; set; }
    }
}
