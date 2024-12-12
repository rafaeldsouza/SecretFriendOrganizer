using Microsoft.EntityFrameworkCore;
using SecretFriendOrganizer.Application.Interfaces.Repositories;
using SecretFriendOrganizer.Domain.Entities;
using SecretFriendOrganizer.Infrastructure.Persistence;

namespace SecretFriendOrganizer.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByKeycloakIdAsync(string keycloakId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.KeycloakId == keycloakId);
        }
    }
}
