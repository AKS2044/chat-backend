﻿namespace Chat.WebApi.Shared.Models.Responses
{
    public class ChatListResponse
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
    }
}
