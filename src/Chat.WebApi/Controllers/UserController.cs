using Chat.Data.Models;
using Chat.Logic.Interfaces;
using Chat.WebApi.Attributes;
using Chat.WebApi.Contracts.Requests;
using Chat.WebApi.Contracts.Responses;
using Chat.WebApi.Settings;
using Chat.WebApi.Shared.Models.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace Chat.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        public readonly IWebHostEnvironment _appEnvironment;
        private readonly IJwtService _jwtService;
        private readonly AppSettings _appSettings;

        public UserController(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IJwtService jwtService,
            IOptions<AppSettings> appSettings,
            IWebHostEnvironment appEnvironment)
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
            _appEnvironment = appEnvironment ?? throw new ArgumentNullException(nameof(appEnvironment));

            if (appSettings is null)
            {
                throw new ArgumentNullException(nameof(appSettings));
            }
            _appSettings = appSettings.Value;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(UserLoginRequest model)
        {
            var errorMessage = new List<object>();
            var checkName = await _userManager.FindByNameAsync(model.UserName);

            if (checkName == null) errorMessage.Add(new { message = "Invalid login!" });

            if (checkName != null)
            { 
                try
                {
                    var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

                    if (!result.Succeeded)
                    {
                        errorMessage.Add(new { message = "Invalid password!" });
                        return BadRequest(errorMessage);
                    }

                    var user = await _userManager.FindByNameAsync(model.UserName);
                    var token = _jwtService.GenerateJwtToken(user.Id, _appSettings.Secret);
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var response = new AuthenticateResponse(user, token, userRoles);

                    return Ok(response);
                }
                catch (Exception error)
                {
                    errorMessage.Add(new { message = error.Message });
                    return BadRequest(errorMessage);
                }
            }

            return BadRequest(errorMessage);
        }

        [HttpPost("registration")]
        public async Task<IActionResult> RegistrationAsync(UserRegistationRequest request)
        {
            var checkName = await _userManager.FindByNameAsync(request.UserName);
            var checkEmail = await _userManager.FindByEmailAsync(request.Email);
            var errorMessage = new List<object>();

            if (checkName == null) errorMessage.Add(new { message = "Данный логин занят, придумай другой." });
            if (checkEmail == null) errorMessage.Add(new { message = "Данный E-mail занят, придумай другой." });
            if (request?.Password?.Length < 6) errorMessage.Add(new { message = "Пароль слишком короткий. Он должен состоять минимум из 6 символов." });
            if (request?.Password != request?.PasswordConfirm) errorMessage.Add(new { message = "Пароли не совпадают!." });
            if (errorMessage.Count > 0) return BadRequest(errorMessage);

            try
            {
                DateTime dateReg = DateTime.Now;
                var user = new User
                {
                    Email = request?.Email,
                    DateReg = dateReg.ToString("dd MMM yyy"),
                    UserName = request?.UserName,
                    PathPhoto = "/UserPhoto/Rick.png",
                    PhotoName = "Rick.png"
                };

                if (request?.Password == request?.PasswordConfirm)
                {
                    await _userManager.CreateAsync(user, request?.Password);
                    await _userManager.AddToRoleAsync(user, "USER");
                }
                var token = _jwtService.GenerateJwtToken(user.Id, _appSettings.Secret);
                var userRoles = await _userManager.GetRolesAsync(user);
                var response = new AuthenticateResponse(user, token, userRoles);

                return Ok(response);
            }
            catch (Exception error)
            {
                errorMessage.Add(new { message = error.Message });
                return BadRequest(errorMessage);
            }
        }

        [HttpPost("uploadPhoto")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            string token = Request.Headers["Authorization"];

            if (token is not null)
            {
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    token = token.Replace("Bearer ", "");
                    var jsonToken = handler.ReadToken(token);
                    var tokenS = handler.ReadToken(token) as JwtSecurityToken;
                    var id = tokenS?.Claims.First(claim => claim.Type == "id").Value;
                    var user = await _userManager.FindByIdAsync(id);

                    string path2 = _appEnvironment.WebRootPath;
                    bool directory = Directory.Exists($"{path2}/UserPhoto/{user.UserName}");
                    if (!directory)
                    Directory.CreateDirectory($"{path2}/UserPhoto/{user.UserName}");

                    string path = $"/UserPhoto/{user.UserName}/" + file.FileName;

                    if (user.PathPhoto != path)
                    {
                        user.PhotoName = file.FileName;
                        user.PathPhoto = path;
                        await _userManager.UpdateAsync(user);
                    }
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);

                        return Ok(path);
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = "Не удалось загрузить файл: " + ex.Message });
                }
            }
            else
            {
                return NotFound(new { message = "User is not found" });
            };
        }

        [HttpPost("logout")]
        public async Task<OkResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpGet("auth")]
        public async Task<IActionResult> Auth()
        {
            string token = Request.Headers["Authorization"];

            if (token != null)
            {
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    token = token.Replace("Bearer ", "");
                    var jsonToken = handler.ReadToken(token);
                    var tokenS = handler.ReadToken(token) as JwtSecurityToken;
                    var id = tokenS.Claims.First(claim => claim.Type == "id").Value;
                    
                    var user = await _userManager.FindByIdAsync(id);
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var response = new AuthenticateResponse(user, token, userRoles);

                    return Ok(response);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }
            else
            {
                return Unauthorized(new { message = "Вы не авторизованы" });
            }
        }

        //[OwnAuthorize]
        [HttpGet("profile")]
        public async Task<IActionResult> ProfileAsync(string userName)
        {
            if (userName != null)
            {
                try
                {
                    var user = await _userManager.FindByNameAsync(userName);
                    if (user != null)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);

                        var result = new ProfileUserResponse
                        {
                            DateReg = user.DateReg,
                            Email = user.Email,
                            Roles = userRoles,
                            UserName = user.UserName,
                            PathPhoto = user.PathPhoto,
                            PhotoName = user.PhotoName
                        };
                        return Ok(result);
                    }
                    return NotFound(new { message = "User is not found" });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }
            else
            {
                return NotFound(new { message = "User is not found" });
            }
        }

        [OwnAuthorizeAdmin]
        [HttpGet("allUsers")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userResponses = new List<ProfileUserResponse>();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                userResponses.Add(new ProfileUserResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    PathPhoto = user.PathPhoto,
                    PhotoName = user.PhotoName,
                    DateReg = user.DateReg,
                    Roles = userRoles,
                });
            }

            return Ok(userResponses);
        }

        [OwnAuthorizeAdmin]
        [HttpDelete("")]
        public async Task<IActionResult> DeleteUsersAsync(string id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            await _userManager.DeleteAsync(user);
            return Ok();
        }

        [OwnAuthorizeAdmin]
        [HttpPost("addRole")]
        public async Task<IActionResult> AddRoleAsync(string roleName)
        {
            if (roleName == null)
                return BadRequest(new { message = "The role field cannot be empty!" });

            await _roleManager.CreateAsync(new IdentityRole(roleName?.ToUpper()));

            return Ok(new { message = $"Role: '{roleName}' successfully created" });
        }

        [OwnAuthorizeAdmin]
        [HttpGet("listRole")]
        public async Task<IActionResult> ListRoleAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            return Ok(roles);
        }
    }
}