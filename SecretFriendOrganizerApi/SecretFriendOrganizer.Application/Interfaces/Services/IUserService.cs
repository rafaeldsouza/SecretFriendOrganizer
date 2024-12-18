using SecretFriendOrganizer.Application.DTOs;
using SecretFriendOrganizer.Domain.Entities;

namespace SecretFriendOrganizer.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<ServiceResponse> CreateUserAsync(string username, string email, string password);
        Task<ServiceResponse<AuthenticatedUserDto>> AuthenticateUserAsync(string username, string password);
        Task<ServiceResponse<AuthenticatedUserDto>> RefreshTokenAsync(string refreshToken);
    }
}
