using SecretFriendOrganizer.Application.DTOs;

namespace SecretFriendOrganizer.Application.Interfaces.Services
{
    public interface IKeycloakService
    {
        Task<string?> GetAccessTokenAsync();
        Task<KeycloakResponse<string>> CreateUserAsync(string username, string email, string password);
        Task<KeycloakResponse> AddUserToGroupAsync(string userId, string groupId);

        Task<KeycloakResponse<KeycloakToken>> AuthenticateAsync(string username, string password);
    }
}
