using LibraryManagementSystem.API.Infrastructure.Auth.JWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using LibraryManagementSystem.Application.Services.Users;
using LibraryManagementSystem.Application.Models.User;
using Swashbuckle.AspNetCore.Annotations;

namespace LibraryManagementSystem.API.Controllers
{
    [Route("api/auth")]
    [AllowAnonymous]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOptions<JWTConfig> _options;


        public UserController(IUserService userService, IOptions<JWTConfig> options)
        {
            _userService = userService;
            _options = options;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="user">User registration model.</param>
        /// <param name="cancellation">Cancellation token.</param>
        /// <returns>Confirmation message with new user ID.</returns>
        [HttpPost("register")]
        [SwaggerResponse(200, "User registered successfully", typeof(object))]
        [SwaggerResponse(400, "Invalid user data")]
        public async Task<IActionResult> Register(UserRegisterModel user, CancellationToken cancellation)
        {
            var result = await _userService.CreateAsync(user, cancellation);
            return Ok(new { Message = "User registered successfully", UserId = result });
        }

        /// <summary>
        /// Logs in a user and returns a JWT token.
        /// </summary>
        /// <param name="user">User login model.</param>
        /// <param name="cancellation">Cancellation token.</param>
        /// <returns>JWT token on successful authentication.</returns>
        [HttpPost("login")]
        [SwaggerResponse(200, "Login successful", typeof(object))]
        [SwaggerResponse(400, "Username and password are required.")]
        [SwaggerResponse(401, "Invalid username or password")]
        public async Task<IActionResult> LogIn(UserLoginModel user, CancellationToken cancellation)
        {
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest(new { Message = "Username and password are required." });
            }

            var result = await _userService.AuthenticationAsync(user.Username,user.Password, cancellation);

            if (result == null)
            {
                return Unauthorized(new { Message = "Invalid username or password" });
            }

            var role = result.Role;
            var token = JwtService.GenerateSecurityToken(user.Username, role, _options);
            return Ok(new { Token = token });
        }
    }
}
