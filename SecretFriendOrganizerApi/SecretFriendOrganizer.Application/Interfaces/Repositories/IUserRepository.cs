using SecretFriendOrganizer.Domain.Entities;

namespace SecretFriendOrganizer.Application.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetByKeycloakIdAsync(string keycloakId);
    }
}
