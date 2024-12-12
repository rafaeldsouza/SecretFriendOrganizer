using SecretFriendOrganizer.Application.DTOs;
using SecretFriendOrganizer.Application.Interfaces.Repositories;
using SecretFriendOrganizer.Application.Interfaces.Services;
using SecretFriendOrganizer.Domain.Entities;

namespace SecretFriendOrganizer.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IKeycloakService _keycloakService;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IKeycloakService keycloakService, IUnitOfWork unitOfWork)
        {
            _keycloakService = keycloakService;
            _unitOfWork = unitOfWork;
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
            // Autenticar no Keycloak
            var tokenResponse = await _keycloakService.AuthenticateAsync(username, password);
            if (!tokenResponse.Success)
            {
                return new ServiceResponse<AuthenticatedUserDto> { Success = false, Message = tokenResponse.Message };
            }

            // Buscar dados do usuário no banco
            var user = await _unitOfWork.Users.GetByKeycloakIdAsync(tokenResponse.Data!.UserId);
            if (user == null)
            {
                return new ServiceResponse<AuthenticatedUserDto> { Success = false, Message = "User not found in database." };
            }

            // Preencher o DTO com os dados necessários
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
    }
}
