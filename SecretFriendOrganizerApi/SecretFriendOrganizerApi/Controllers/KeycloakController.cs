using Microsoft.AspNetCore.Mvc;
using SecretFriendOrganizer.Application.Interfaces.Services;

namespace SecretFriendOrganizer.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KeycloakController : ControllerBase
    {
        private readonly IKeycloakService _keycloakService;

        public KeycloakController(IKeycloakService keycloakService)
        {
            _keycloakService = keycloakService;
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser(string username, string email, string password)
        {
            var response = await _keycloakService.CreateUserAsync(username, email, password);
            return response.Success ? Ok(response) : StatusCode(500, response);
        }

        [HttpPost("add-to-group")]
        public async Task<IActionResult> AddUserToGroup(string userId, string groupId)
        {
            var response = await _keycloakService.AddUserToGroupAsync(userId, groupId);
            return response.Success ? Ok(response) : StatusCode(500, response);
        }
    }

}
