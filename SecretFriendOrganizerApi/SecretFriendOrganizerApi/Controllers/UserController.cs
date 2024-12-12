using Microsoft.AspNetCore.Mvc;
using SecretFriendOrganizer.Application.DTOs;
using SecretFriendOrganizer.Application.Interfaces.Services;

namespace SecretFriendOrganizer.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Creates a new user in the system and Keycloak.
        /// </summary>
        /// <param name="request">The user creation request</param>
        /// <returns>Result of the operation</returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.CreateUserAsync(request.Username, request.Email, request.Password);
            if (!result.Success)
            {
                return BadRequest(new { Message = result.Message });
            }

            return Ok(new { Message = result.Message });
        }

        /// <summary>
        /// Authenticates a user using Keycloak.
        /// </summary>
        /// <param name="request">The authentication request</param>
        /// <returns>Authentication result</returns>
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateUser([FromBody] AuthenticateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.AuthenticateUserAsync(request.Username, request.Password);
            if (!result.Success)
            {
                return Unauthorized(new { Message = result.Message });
            }

            return Ok(new { Message = result.Message, Data = result.Data });
        }
    }
}
