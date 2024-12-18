using Microsoft.Extensions.Logging;
using SecretFriendOrganizer.Application.DTOs;
using SecretFriendOrganizer.Application.Interfaces.Repositories;
using SecretFriendOrganizer.Application.Interfaces.Services;
using SecretFriendOrganizer.Domain.Entities;
using System.Net.Http;
using System.Text.Json;

namespace SecretFriendOrganizer.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IKeycloakService _keycloakService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger;

        public UserService(IKeycloakService keycloakService, IUnitOfWork unitOfWork, ILogger<UserService> logger)
        {
            _keycloakService = keycloakService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ServiceResponse> CreateUserAsync(string username, string email, string password)
        {
            // Criar usuário no Keycloak
            var keycloakResponse = await _keycloakService.CreateUserAsync(username, email, password);
            if (!keycloakResponse.Success)
            {
                return new ServiceResponse { Success = false, Message = keycloakResponse.Message };
            }

            var user = new User
            {
                KeycloakId = keycloakResponse.Data,
                Email = email,
                Username = username
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return new ServiceResponse { Success = true, Message = "User created successfully." };
        }

        public async Task<ServiceResponse<AuthenticatedUserDto>> AuthenticateUserAsync(string username, string password)
        {

            var tokenResponse = await _keycloakService.AuthenticateAsync(username, password);
            return await ProcessKeycloackResponse(tokenResponse);
        }

        private async Task<ServiceResponse<AuthenticatedUserDto>> ProcessKeycloackResponse(KeycloakResponse<KeycloakToken> tokenResponse)
        {
            if (!tokenResponse.Success)
            {
                return new ServiceResponse<AuthenticatedUserDto> { Success = false, Message = tokenResponse.Message };
            }

            var user = await _unitOfWork.Users.GetByKeycloakIdAsync(tokenResponse.Data!.UserId);
            if (user == null)
            {
                return new ServiceResponse<AuthenticatedUserDto> { Success = false, Message = "User not found in database." };
            }

            var authenticatedUserDto = new AuthenticatedUserDto
            {
                UserId = user.Id.ToString(),
                Username = user.Username,
                Email = user.Email,
                AccessToken = tokenResponse.Data.AccessToken,
                RefreshToken = tokenResponse.Data.RefreshToken,
                TokenType = tokenResponse.Data.TokenType,
                ExpiresIn = tokenResponse.Data.ExpiresIn
            };

            return new ServiceResponse<AuthenticatedUserDto>
            {
                Success = true,
                Message = "Authentication successful.",
                Data = authenticatedUserDto
            };
        }

        public async Task<ServiceResponse<AuthenticatedUserDto>> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var tokenResponse = await _keycloakService.RefreshTokenAsync(refreshToken);
                return await ProcessKeycloackResponse(tokenResponse);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token");
                return new ServiceResponse<AuthenticatedUserDto>
                {
                    Success = false,
                    Message = "An error occurred while refreshing the token"
                };
            }
        }
    }
}
