using Chat.Data.Models;
using Chat.WebApi.Shared.Models;

namespace Chat.WebApi.Contracts.Responses
{
    /// <summary>
    /// User authenticate response.
    /// </summary>
    public class AuthenticateResponse : UserAuthModel
    {
        /// <summary>
        /// Constructor with params.
        /// </summary>
        /// <param name="user">User database model.</param>
        /// <param name="token">Jwt token.</param>
        public AuthenticateResponse(User user, string token, IList<string> roles)
        {
            Id = user.Id;
            UserName = user.UserName;
            PathPhoto = user.PathPhoto;
            Email = user.Email;
            Token = token;
            Roles = roles;
        }
    }
}
